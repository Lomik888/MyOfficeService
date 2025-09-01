using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Dtos;

namespace MyOfficeService.Application.Employees.Commands.Update;

public record UpdateEmployeeCommand(
    int Id,
    string? Name,
    string? Phone,
    int? CompanyId,
    int? DepartmentId,
    PassportDto? PassportDto) : ICommand;