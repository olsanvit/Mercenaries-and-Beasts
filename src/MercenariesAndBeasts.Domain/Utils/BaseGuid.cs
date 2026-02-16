
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MercenariesAndBeasts.Domain.Utils;
public class BaseGuid
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int BaseLevel {get;set;} = 1;
    public string Code { get; set; } = String.Empty;
    [Required]
    public string NameEn {get;set;} = String.Empty;
    public string DescriptionEn {get;set;} = String.Empty;

    public string? ImagePath { get; set; }           // např. "/images/monsters/XXX.webp"
    public string? ImagePromptMeta { get; set; }     // volitelné – jaký AI prompt to vygeneroval
}

