using FluentValidation;
using MyOfficeService.Application.Extations;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Employees.Commands.Delete;

public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).Must(x => x > 0)
            .WithMessage(GeneralErrors.Validation.InvalidField($"Id сотрудника  не может быть меньше 0"));
    }
}