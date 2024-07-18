using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Models.Settings;

public class AppSettings
{
    [Required]
    public required double DefaultApiVersion { get; init; }
}