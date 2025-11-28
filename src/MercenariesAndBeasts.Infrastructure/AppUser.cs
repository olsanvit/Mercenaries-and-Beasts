using Microsoft.AspNetCore.Identity;

namespace MercenariesAndBeasts.Infrastructure;

public class AppUser : IdentityUser
{
    public bool IsAdmin { get; set; }
}