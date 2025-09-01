using FluentValidation;
using MyOfficeService.Application.Extations;
using MyOfficeService.Domain.Entities;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Application.Employees.Commands.Update;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                x.Name != null ||
                x.Phone != null ||
                x.CompanyId != null ||
                x.DepartmentId != null ||
                x.PassportDto != null)
            .WithMessage(
                GeneralErrors.Validation.InvalidField("Нет данных для обновления сотрудника"))
            .DependentRules(() =>
            {
                RuleFor(x => x.Id).Must(x => x > 0)
                    .WithMessage(GeneralErrors.Validation.InvalidField("Необходимо указать Id сотрудника"));

                When(
                    x => string.IsNullOrWhiteSpace(x.Phone) == false,
                    () => { RuleFor(x => x.Phone).Must(x => Phone.Validate(x!)); });

                When(x => x.PassportDto != null,
                    () =>
                    {
                        RuleFor(x => x.PassportDto)
                            .Must(x => Passport.Validate(x!.Type, x.Number));
                    });

                When(x => x.CompanyId != null, () =>
                {
                    RuleFor(x => x.CompanyId).Must(x => x > 0)
                        .WithMessage(GeneralErrors.Validation.InvalidField("Неправильный CompanyId"));
                });

                When(x => x.DepartmentId != null, () =>
                {
                    RuleFor(x => x.DepartmentId).Must(x => x > 0)
                        .WithMessage(GeneralErrors.Validation.InvalidField("Неправильный DepartmentId"));
                });

                When(x => string.IsNullOrWhiteSpace(x.Name) == false, () =>
                {
                    RuleFor(x => x.Name).Must(x => x!.Length <= Employee.MAX_NAME_LENGTH)
                        .WithMessage(GeneralErrors.Validation.InvalidField("Неправильный DepartmentId"));
                });
            });
    }
}