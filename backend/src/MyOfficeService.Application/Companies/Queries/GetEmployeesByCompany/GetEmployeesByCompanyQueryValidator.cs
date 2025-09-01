using FluentValidation;
using MyOfficeService.Application.Extations;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Companies.Queries.GetEmployeesByCompany;

public class GetEmployeesByCompanyQueryValidator : AbstractValidator<GetEmployeesByCompanyQuery>
{
    public GetEmployeesByCompanyQueryValidator()
    {
        RuleFor(x => x.CompanyId).Must(x => x > 0)
            .WithMessage(GeneralErrors.Validation.InvalidField("CompanyId должен быть указан"));
    }
}