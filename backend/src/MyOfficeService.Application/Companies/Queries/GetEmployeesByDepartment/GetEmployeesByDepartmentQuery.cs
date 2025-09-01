using MyOfficeService.Application.Abstractions.Handlers;

namespace MyOfficeService.Application.Companies.Queries.GetEmployeesByDepartment;

public record GetEmployeesByDepartmentQuery(int CompanyId, int DepartmentId) : IQuery;