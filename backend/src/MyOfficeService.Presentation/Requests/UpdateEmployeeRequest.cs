using MyOfficeService.Application.Dtos;
using MyOfficeService.Application.Employees.Commands.Update;
using MyOfficeService.Presentation.Abstractions;

namespace MyOfficeService.Presentation.Requests;

public record UpdateEmployeeRequest(
    string? Name,
    string? Phone,
    int? CompanyId,
    int? DepartmentId,
    PassportDto? Passport) : IRequest<int, UpdateEmployeeCommand>
{
    public UpdateEmployeeCommand ToCommand(int id)
    {
        var command = new UpdateEmployeeCommand(
            id,
            Name,
            Phone,
            CompanyId,
            DepartmentId,
            Passport
        );

        return command;
    }
}