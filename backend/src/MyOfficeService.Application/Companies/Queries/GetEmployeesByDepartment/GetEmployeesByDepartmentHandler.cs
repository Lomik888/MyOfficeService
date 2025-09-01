using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Application.Extations;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Companies.Queries.GetEmployeesByDepartment;

public class GetEmployeesByDepartmentHandler
    : IQueryHandler<IEnumerable<EmployeeDto>, ErrorList, GetEmployeesByDepartmentQuery>
{
    private readonly IValidator<GetEmployeesByDepartmentQuery> _validator;
    private readonly ILogger<GetEmployeesByDepartmentHandler> _logger;
    private readonly ICompanyRepository _companyRepository;

    public GetEmployeesByDepartmentHandler(
        IValidator<GetEmployeesByDepartmentQuery> validator,
        ICompanyRepository companyRepository,
        ILogger<GetEmployeesByDepartmentHandler> logger)
    {
        _validator = validator;
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<EmployeeDto>, ErrorList>> HandleAsync(
        GetEmployeesByDepartmentQuery request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("{0}", validationResult.Errors);
            return validationResult.GetErrorList();
        }

        var employees =
            await _companyRepository.GetEmployeesAsync(request.CompanyId, request.DepartmentId, cancellationToken);

        return Result.Success<IEnumerable<EmployeeDto>, ErrorList>(employees);
    }
}