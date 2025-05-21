using System.ComponentModel.DataAnnotations;

namespace src.DTO;

public class PositionDto
{
    public PositionDto(int id, string positionName)
    {
        Id = id;
        PositionName = positionName;
    }

    public int Id { get; set; }
    public string PositionName { get; set; }

    public PositionDto()
    {
    }
}