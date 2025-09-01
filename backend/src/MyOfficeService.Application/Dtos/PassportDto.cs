namespace MyOfficeService.Application.Dtos;

public class PassportDto
{
    public string Type { get; init; }

    public string Number { get; init; }

    public PassportDto(string type, string number)
    {
        Type = type;
        Number = number;
    }
}