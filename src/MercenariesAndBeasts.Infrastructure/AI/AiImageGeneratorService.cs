using System.Text.RegularExpressions;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI.Images;
using OpenAI;

namespace MercenariesAndBeasts.Infrastructure.AI;

public class AiImageGeneratorService : IAiImageGenerator
{
    // Můžeš klidně sdílet stejný GameTheme jako v AiUnitGeneratorService,
    // nebo si tady držet vlastní variantu.
  private string GameTheme =>
    "A future-fantasy role-playing universe where ancient magic, forbidden rites, and rare arcane-tech coexist. " +
    "Aesthetic: atmospheric, enigmatic, slightly grim, with echoes of lost civilizations, sealed sanctums, relic cults, and ritual machinery. " +
    "World tone: immersive, mysterious, premium RPG feeling. " +
    "Arcane-tech is allowed only as ancient or enigmatic technology (resonant engines, glyph-forges, sealed mechanisms), " +
    "never modern, digital, or cyberpunk in nature. " +
    "Avoid explicit modern or IT terminology such as interface, UI, database, smartphone, drone, android, chatbot, nanotech, cybernetics, or laser weaponry. " +
    "Naming style: unique, evocative, lore-friendly, and easy to pronounce; never generic or repetitive. " +
    "Names must not include numbers, hyphens, or MMO-style tags. " +
    "Avoid overused clichés such as 'of Doom', 'of the Ancients', 'Ultimate', 'Extreme', or 'Legendary'. " +
    "Descriptions should feel grounded in mystery, decay, sealed spaces, and forgotten power, consistent with a dark future-fantasy tone.";
    
    private readonly ChatGptAsker _asker;
    private readonly IHostEnvironment _env;
    private readonly ILogger<AiImageGeneratorService> _logger;

    public AiImageGeneratorService(
        ChatGptAsker asker,
        IHostEnvironment env,
        ILogger<AiImageGeneratorService> logger)
    {
        _asker = asker;
        _env = env;
        _logger = logger;
    }

    // ---------- PUBLIC API ----------

    public Task<string?> GenerateLocationImageAsync(Location location, CancellationToken ct = default)
    {
        var prompt =
            $"Wide isometric environmental concept art of a travel location called \"{location.NameEn}\" " +
            $"in {GameTheme}. " +
            $"Dominant element theme: {location.Element}. " +
            "Show terrain, landmarks and atmosphere, no UI, no text, no logos, no characters in the foreground.";

        var filePrefix = $"loc_{SafeSlug(location.Code ?? location.NameEn)}";

        return GenerateAndSaveAsync(
            prompt,
            subfolder: "locations",
            filePrefix: filePrefix,
            ct: ct);
    }

    public Task<string?> GenerateDungeonImageAsync(Dungeon dungeon, CancellationToken ct = default)
    {
        var prompt =
            $"Isometric key art of a dungeon called \"{dungeon.NameEn}\" in {GameTheme}. " +
            $"Elemental flavor: {dungeon.Element}. " +
            "Show the dungeon entrance or a key interior hall with traps/architecture, " +
            "no UI, no text, no logos.";

        var filePrefix = $"dun_{SafeSlug(dungeon.Code ?? dungeon.NameEn)}";

        return GenerateAndSaveAsync(
            prompt,
            subfolder: "dungeons",
            filePrefix: filePrefix,
            ct: ct);
    }

    public Task<string?> GenerateMonsterImageAsync(MonsterTemplate monster, CancellationToken ct = default)
    {
        var prompt =
            $"Full-body concept art of a monster named \"{monster.NameEn}\" in {GameTheme}. " +
            $"Element: {monster.Element}. " +
            "Dynamic pose, clear silhouette, no background UI, no text, no stats, no card frame.";

        var filePrefix = $"mon_{SafeSlug(monster.Code ?? monster.NameEn)}";

        return GenerateAndSaveAsync(
            prompt,
            subfolder: "monsters",
            filePrefix: filePrefix,
            ct: ct);
    }

    public Task<string?> GenerateMercenaryImageAsync(MercenaryTemplate mercenary, CancellationToken ct = default)
    {
        var prompt =
            $"Full-body character concept art of a mercenary named \"{mercenary.NameEn}\" in {GameTheme}. " +
            $"Element: {mercenary.Element}. " +
            "Heroic but grounded pose, light fantasy armor/gear, no UI, no text, no card frame.";

        var filePrefix = $"merc_{SafeSlug(mercenary.Code ?? mercenary.NameEn)}";

        return GenerateAndSaveAsync(
            prompt,
            subfolder: "mercenaries",
            filePrefix: filePrefix,
            ct: ct);
    }

    // ---------- INTERNAL SHARED LOGIC ----------

    private async Task<string?> GenerateAndSaveAsync(
        string prompt,
        string subfolder,
        string filePrefix,
        CancellationToken ct)
    {
        try
        {
           var options = new ImageGenerationOptions
            {
                Quality = GeneratedImageQuality.High,
                Size = GeneratedImageSize.W1024xH1024,
                Style = GeneratedImageStyle.Vivid,
                ResponseFormat = GeneratedImageFormat.Bytes
            };

            var bytes = await _asker.GenerateImageBytesAsync(
                prompt,
                ct);
            if (bytes is null || bytes.ToArray().Length == 0)
                return null;

            var fileName = $"{filePrefix}_{DateTime.UtcNow:yyyyMMddHHmmss}.png";

            // relativní cesta pod wwwroot
            var relativePath = Path.Combine("images", subfolder, fileName);

            // fyzická cesta
            var webRoot = string.IsNullOrWhiteSpace(_env.ContentRootPath)
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
                : _env.ContentRootPath;

            var physicalPath = Path.Combine(webRoot, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath)!);

            await File.WriteAllBytesAsync(physicalPath, bytes.ToArray(), ct);

            // cesta pro <img src="...">
            var webPath = "/" + relativePath.Replace("\\", "/");
            return webPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error generating image. Subfolder={Subfolder}, Prefix={Prefix}",
                subfolder, filePrefix);
            return null;
        }
    }

    private static string SafeSlug(string? raw)
    {
        raw ??= "ENTITY";
        var s = raw.Trim();

        // odstraníme všechno krom A-Z, a-z, 0-9 a mezer
        s = Regex.Replace(s, @"[^A-Za-z0-9 ]+", " ");
        // sloučíme vícenásobné mezery
        s = Regex.Replace(s, @"\s+", " ").Trim();
        if (string.IsNullOrWhiteSpace(s))
            s = "ENTITY";

        // nahradíme mezery podtržítky
        s = s.Replace(' ', '_');

        return s.ToUpperInvariant();
    }
}