using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MercenariesAndBeasts.Infrastructure.Players;

public class AdminUserService
{
    private readonly IDbContextFactory<MercenariesAndBeastsDbContext> _dbFactory;
    private readonly PlayerOnboardingService _onboarding;

    // pojistka proti paralelním voláním z UI (double-click / render overlap)
    private readonly SemaphoreSlim _gate = new(1, 1);

    public AdminUserService(
        PlayerOnboardingService onboarding,
        IDbContextFactory<MercenariesAndBeastsDbContext> dbFactory,
        UserManager<AppUser> userManager) // může zůstat kvůli DI, ale v listingu ho nepoužíváme
    {
        _dbFactory = dbFactory;
        _onboarding = onboarding;
    }

    public async Task<List<AdminUserRow>> GetUsersAsync()
    {
        await _gate.WaitAsync();
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            // 1) Users přes stejný DbContext (ne přes UserManager.Users)
            var users = await db.Users
                .AsNoTracking()
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.UserName
                })
                .ToListAsync();

            // 2) Profiles
            var profiles = await db.Players
                .AsNoTracking()
                .Select(p => new { p.UserId, p.CreatedUtc })
                .ToListAsync();

            // 3) join v paměti
            var profileByUserId = profiles.ToDictionary(x => x.UserId, x => x.CreatedUtc);

            return users.Select(u =>
            {
                var has = profileByUserId.TryGetValue(u.Id, out var createdUtc);

                return new AdminUserRow
                {
                    UserId = u.Id,
                    Email = u.Email ?? u.UserName ?? "(no email)",
                    HasProfile = has,
                    ProfileCreatedUtc = has ? createdUtc : null
                };
            }).ToList();
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task CreateProfileAsync(string userId)
    {
        await _gate.WaitAsync();
        try
        {
            await _onboarding.EnsurePlayerInitializedAsync(userId);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task RecreateProfileAsync(string userId)
    {
        await _gate.WaitAsync();
        try
        {
            await _onboarding.RecreateProfileAsync(userId);
        }
        finally
        {
            _gate.Release();
        }
    }
}

public class AdminUserRow
{
    public string UserId { get; set; } = "";
    public string Email { get; set; } = "";
    public bool HasProfile { get; set; }
    public DateTime? ProfileCreatedUtc { get; set; }
}