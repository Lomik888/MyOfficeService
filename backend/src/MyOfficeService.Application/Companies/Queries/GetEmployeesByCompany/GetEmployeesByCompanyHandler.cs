using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Application.Extations;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Companies.Queries.GetEmployeesByCompany;

public class GetEmployeesByCompanyHandler
    : IQueryHandler<IEnumerable<EmployeeDto>, ErrorList, GetEmployeesByCompanyQuery>
{
    private readonly IValidator<GetEmployeesByCompanyQuery> _validator;
    private readonly ILogger<GetEmployeesByCompanyHandler> _logger;
    private readonly ICompanyRepository _companyRepository;

    public GetEmployeesByCompanyHandler(
        IValidator<GetEmployeesByCompanyQuery> validator,
        ICompanyRepository companyRepository,
        ILogger<GetEmployeesByCompanyHandler> logger)
    {
        _validator = validator;
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<EmployeeDto>, ErrorList>> HandleAsync(
        GetEmployeesByCompanyQuery request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("{0}", validationResult.Errors);
            return validationResult.GetErrorList();
        }

        var employees =
            await _companyRepository.GetEmployeesAsync(request.CompanyId, cancellationToken);

        return Result.Success<IEnumerable<EmployeeDto>, ErrorList>(employees);
    }
}