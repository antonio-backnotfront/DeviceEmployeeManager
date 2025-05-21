namespace src.DTO;

public class GetDeviceDto
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceType { get; set; }
    public object? CurrentEmployee { get; set; }
    public object AdditionalProperties { get; set; }
    public GetDeviceDto() { }
    
}