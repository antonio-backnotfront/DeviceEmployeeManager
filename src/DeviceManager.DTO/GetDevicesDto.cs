using System.ComponentModel.DataAnnotations;

namespace src.DTO;

public class GetDevicesDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public GetDevicesDto()
    { }

    public GetDevicesDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}