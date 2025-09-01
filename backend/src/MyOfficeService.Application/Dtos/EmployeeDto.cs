namespace MyOfficeService.Application.Dtos;

public class EmployeeDto
{
    public string Name { get; set; }

    public string Phone { get; set; }

    public int CompanyId { get; set; }

    public int DepartmentId { get; set; }

    public string PassportType { get; set; }

    public string PassportNumber { get; set; }
}