using MyOfficeService.Application.Abstractions.Handlers;

namespace MyOfficeService.Application.Employees.Commands.Create;

public record CreateEmployeeCommand(
    string Name,
    string Phone,
    int CompanyId,
    int DepartmentId,
    string PassportType,
    string PassportNumber) : ICommand;