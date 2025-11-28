using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Units;
using Microsoft.AspNetCore.Identity;

namespace MercenariesAndBeasts.Infrastructure;

public static class GameSeed
{
    public static async Task SeedBaseContentAsync(GameDbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // ROLE: Admin
        const string adminRoleName = "Admin";
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
        }

        // USER: admin@local
        const string adminEmail = "admin@local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                IsAdmin = true
            };

            // heslo si pak změň, toto je jen dev
            var result = await userManager.CreateAsync(admin, "Admin123.");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, adminRoleName);
            }
        }

        // ... tady už máš seed lokací a dungeonů z minula ...
        // jen změň signaturu metody a předej userManager / roleManager

        // příklad: if (!await db.Locations.AnyAsync()) { ... } atd.

        await db.SaveChangesAsync();
    }
}