using FluentValidation;
using MyOfficeService.Application.Extations;
using MyOfficeService.Domain.Entities;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Application.Employees.Commands.Create;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => Employee.Validate(
                x.Name,
                x.CompanyId,
                x.DepartmentId));

        RuleFor(x => x.Phone).Must(x => Phone.Validate(x));

        RuleFor(x => x)
            .Must(x => Passport.Validate(x.PassportType, x.PassportNumber));
    }
}