using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MercenariesAndBeasts.Infrastructure;

public class AppUser : IdentityUser
{
    public bool IsAdmin { get; set; }
    [MaxLength(5)]
    public string? PreferredCulture { get; set; } // "en", "cs", "de"

    // guard aby se seed nespustil 2×
    public bool IsBanned { get; set; }
    
}