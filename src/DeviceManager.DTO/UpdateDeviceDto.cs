using System.ComponentModel.DataAnnotations;

namespace src.DTO;

public class UpdateDeviceDto
{
    [Required]
    public string Name { get; set; }
    public string? DeviceType { get; set; }
    public bool IsEnabled { get; set; }
    public string AdditionalProperties { get; set; }

    public UpdateDeviceDto(string name, string? deviceType, bool isEnabled, string additionalProperties)
    {
        Name = name;
        DeviceType = deviceType;
        IsEnabled = isEnabled;
        AdditionalProperties = additionalProperties;
    }
}