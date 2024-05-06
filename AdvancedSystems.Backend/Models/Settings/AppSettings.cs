using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Models.Settings;

public record AppSettings
{
    [Required]
    public double DefaultApiVersion { get; set; }
}