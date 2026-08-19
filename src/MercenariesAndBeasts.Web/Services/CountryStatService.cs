using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MercenariesAndBeasts.Web.Services;

public class CountryStatService
{
    private readonly string _baseDir;

    public CountryStatService(IWebHostEnvironment env)
    {
        _baseDir = Path.Combine(env.ContentRootPath, "country_stats");
        Directory.CreateDirectory(_baseDir);
    }

    private string FilePath(string userId)
    {
        var hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(userId)))[..8];
        return Path.Combine(_baseDir, $"country_stats_{hash}.json");
    }

    private async Task<Dictionary<string, long>> LoadAsync(string userId)
    {
        var path = FilePath(userId);
        if (!File.Exists(path)) return new();
        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<Dictionary<string, long>>(json) ?? new();
    }

    private async Task SaveAsync(string userId, Dictionary<string, long> data)
    {
        await File.WriteAllTextAsync(FilePath(userId), JsonSerializer.Serialize(data));
    }

    public async Task<long> IncrementAndGetAsync(string userId, string key, long by = 1)
    {
        var data = await LoadAsync(userId);
        data[key] = data.GetValueOrDefault(key) + by;
        await SaveAsync(userId, data);
        return data[key];
    }

    public async Task<long> AddAndGetAsync(string userId, string key, long amount)
        => await IncrementAndGetAsync(userId, key, amount);

    public async Task<long> GetCurrentAsync(string userId, string key)
    {
        var data = await LoadAsync(userId);
        return data.GetValueOrDefault(key);
    }
}
