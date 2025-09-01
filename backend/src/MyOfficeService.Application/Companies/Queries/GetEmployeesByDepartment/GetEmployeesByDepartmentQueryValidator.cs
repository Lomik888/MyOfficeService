using FluentValidation;
using MyOfficeService.Application.Extations;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Companies.Queries.GetEmployeesByDepartment;

public class GetEmployeesByDepartmentQueryValidator : AbstractValidator<GetEmployeesByDepartmentQuery>
{
    public GetEmployeesByDepartmentQueryValidator()
    {
        RuleFor(x => x.CompanyId).Must(x => x > 0)
            .WithMessage(GeneralErrors.Validation.InvalidField("CompanyId должен быть указан"));

        RuleFor(x => x.DepartmentId).Must(x => x > 0)
            .WithMessage(GeneralErrors.Validation.InvalidField("DepartmentId должен быть указан"));
    }
}