using System.Text.Json;

namespace src.DTO;

public class GetDeviceDto
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceType { get; set; }
    public object? CurrentEmployee { get; set; }
    public JsonElement AdditionalProperties { get; set; }
    public GetDeviceDto() { }
    
}