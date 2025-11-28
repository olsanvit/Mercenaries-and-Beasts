
using System.ComponentModel.DataAnnotations;

namespace MercenariesAndBeasts.Domain.Utils;
public class BaseGuid
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int BaseLevel {get;set;} = 1;
    [Required]
    public string NameEn {get;set;} = String.Empty;
    public string DescriptionEn {get;set;} = String.Empty;

    public string? ImageUrl { get; set; }
}