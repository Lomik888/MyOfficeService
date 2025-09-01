using MyOfficeService.Application.Employees.Commands.Create;
using MyOfficeService.Presentation.Abstractions;

namespace MyOfficeService.Presentation.Requests;

public record CreateEmployeeRequest(
    string Name,
    string Phone,
    int CompanyId,
    int DepartmentId,
    string PassportType,
    string PassportNumber) : IRequest<CreateEmployeeCommand>
{
    public CreateEmployeeCommand ToCommand()
    {
        var command = new CreateEmployeeCommand(
            Name,
            Phone,
            CompanyId,
            DepartmentId,
            PassportType,
            PassportNumber
        );

        return command;
    }
}