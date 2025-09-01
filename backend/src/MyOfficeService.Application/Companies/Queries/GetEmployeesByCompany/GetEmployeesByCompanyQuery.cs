using MyOfficeService.Application.Abstractions.Handlers;

namespace MyOfficeService.Application.Companies.Queries.GetEmployeesByCompany;

public record GetEmployeesByCompanyQuery(int CompanyId) : IQuery;