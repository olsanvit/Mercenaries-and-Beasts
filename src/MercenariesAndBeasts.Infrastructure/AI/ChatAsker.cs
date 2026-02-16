using OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MercenariesAndBeasts.Infrastructure.AI
{
    public sealed class QuotaExceededException : Exception
    {
        public QuotaExceededException(string message) : base(message) { }
    }

    /// <summary>
    /// Nízkourovňový wrapper nad OpenAI Chat API.
    /// Řeší:
    /// - výběr modelu
    /// - throttling paralelismu
    /// - retry s exponenciálním backoffem
    /// - detekci insufficient_quota
    /// 
    /// Pro hru používáš hlavně: AskJsonAsync&lt;T&gt;(...)
    /// </summary>
    public class ChatGptAsker
    {
        public static volatile bool HasQuota = true; // když spadne na insufficient_quota -> false

        private const string ModelGpt4 = "gpt-4o-mini";
        private const string ModelGpt5 = "gpt-5.1-mini"; // nebo jiný, podle toho co máš přístupné

        private readonly ChatClient _client;
        private readonly SemaphoreSlim _throttle;
        private readonly int _maxParallelism;
        private readonly int _maxRetries;
        private readonly int _baseDelayMs;
        private readonly ImageClient _imageClient;

        private static readonly JsonSerializerOptions LlmJsonOpts = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        public ChatGptAsker(
            string apiKey,
            bool isSimple,
            int maxParallelism = 3,
            int maxRetries = 2,
            int baseDelayMs = 750)
        {
            
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");

            var model = isSimple ? ModelGpt4 : ModelGpt5;
            
            var options = new OpenAIClientOptions
            {
                NetworkTimeout = TimeSpan.FromMinutes(8)
            };

            var credential = new ApiKeyCredential(apiKey);
            var openAi = new OpenAIClient(credential, options);

            _client = openAi.GetChatClient(model);

            _imageClient = openAi.GetImageClient("gpt-image-1");
            _maxParallelism = Math.Max(1, maxParallelism);
            _throttle = new SemaphoreSlim(_maxParallelism);
            _maxRetries = Math.Max(0, maxRetries);
            _baseDelayMs = Math.Max(100, baseDelayMs);
                LlmJsonOpts.Converters.Add(new JsonStringEnumConverter());
        }

        private static bool LooksLikeInsufficientQuota(Exception ex)
        {
            var msg = ex.Message?.ToLowerInvariant() ?? "";
            return msg.Contains("insufficient_quota") ||
                   msg.Contains("you exceeded your current quota");
        }

        private static int? TryParseRetryAfterMs(Exception ex)
        {
            // pokud si někdy doplníš vlastní handler, můžeš z ex.Data["Retry-After"] tahat ms
            return null;
        }

        private static int NextDelayMs(int current) => Math.Min(current * 2, 8000);
        private static int Jitter(int baseMs) => baseMs + Random.Shared.Next(0, baseMs / 3 + 1);

        // --------------------------
        // CENTRÁLNÍ ODESLÁNÍ + RETRY
        // --------------------------
        private async Task<ChatCompletion> SendAsync(ChatMessage[] messages, CancellationToken ct)
        {
            if (!HasQuota)
                throw new QuotaExceededException("OpenAI quota already exceeded (short-circuited).");

            await _throttle.WaitAsync(ct);
            try
            {
                int delay = _baseDelayMs;

                for (int attempt = 0; ; attempt++)
                {
                    try
                    {
                        return await _client.CompleteChatAsync(messages, cancellationToken: ct);
                    }
                    catch (ClientResultException cre) when (cre.Status == 429)
                    {
                        if (LooksLikeInsufficientQuota(cre))
                        {
                            HasQuota = false;
                            throw new QuotaExceededException("OpenAI: insufficient_quota (429). Check billing.");
                        }

                        if (attempt >= _maxRetries) throw;

                        var retryAfter = TryParseRetryAfterMs(cre);
                        await Task.Delay(retryAfter ?? Jitter(delay), ct);
                        delay = NextDelayMs(delay);
                    }
                    catch (HttpRequestException hre) when (hre.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        if (attempt >= _maxRetries) throw;
                        await Task.Delay(Jitter(delay), ct);
                        delay = NextDelayMs(delay);
                    }
                    catch (HttpRequestException hre) when (hre.StatusCode is null || (int)hre.StatusCode >= 500)
                    {
                        if (attempt >= _maxRetries) throw;
                        await Task.Delay(Jitter(delay), ct);
                        delay = NextDelayMs(delay);
                    }
                }
            }
            finally
            {
                _throttle.Release();
            }
        }

        private static string SanitizeJsonResponse(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            var cleaned = raw.Trim();

            if (cleaned.StartsWith("```", StringComparison.Ordinal))
            {
                cleaned = Regex.Replace(cleaned, @"^```(?:json)?\s*", "", RegexOptions.IgnoreCase);
                cleaned = Regex.Replace(cleaned, @"\s*```$", "", RegexOptions.IgnoreCase);
            }

            // pro debugging – klidně zakomentuj
            Console.WriteLine("Raw GPT output:");
            Console.WriteLine(raw);

            return cleaned.Trim();
        }

        // --------------------------
        // OBECNÉ JSON VOLÁNÍ PRO HERNÍ GENERÁTOR
        // --------------------------
        public async Task<T?> AskJsonAsync<T>(
            string systemPrompt,
            string userPrompt,
            CancellationToken ct = default)
        {
            var response = await SendAsync(
                new ChatMessage[]
                {
                    ChatMessage.CreateSystemMessage(systemPrompt),
                    ChatMessage.CreateUserMessage(userPrompt)
                },
                ct
            );

            var json = SanitizeJsonResponse(response.Content.FirstOrDefault()?.Text);
            if (string.IsNullOrWhiteSpace(json))
                return default;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

           
            return JsonSerializer.Deserialize<T>(json, options);
        }
        public async Task<byte[]?> GenerateImageBytesAsync(
    string prompt,
    CancellationToken ct = default)
            {
                var options = new ImageGenerationOptions
                {
                    //Quality        = GeneratedImageQuality.High,
                    //Size           = GeneratedImageSize.W1024xH1024
                };

                try
                {
                    GeneratedImage image = await _imageClient.GenerateImageAsync(prompt, options, ct);
                    var bytes = image.ImageBytes;
                    return bytes?.ToArray();
                }
                catch (Exception ex)
                {
                    // tady můžeš případně logovat – ChatGptAsker nemá logger, takže jen:
                    Console.WriteLine($"Image generation failed: {ex.Message}");
                    return null;
                }
            }
    }
}