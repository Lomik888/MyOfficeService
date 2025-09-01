using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Companies.Queries.GetEmployeesByCompany;
using MyOfficeService.Application.Companies.Queries.GetEmployeesByDepartment;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Presentation.Abstractions;
using MyOfficeService.Presentation.Responses;

namespace MyOfficeService.Presentation.Controllers;

public class CompanyController : BaseApplicationController
{
    /// <summary>
    /// GetByCompanyAsync - вернтё всех сотрудников компании
    /// </summary>
    /// <param name="companyId">Id компании для поиска сотрудников</param>
    /// <param name="handler">Обработчик запроса</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Вернёт Envelope со списком сотрудников или вернёт сообщение об ошибке</returns>
    [HttpGet("/company/{id:int}/employees")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<CustomResult<IEnumerable<EmployeeDto>>> GetByCompanyAsync(
        [FromRoute] int id,
        [FromServices] IQueryHandler<IEnumerable<EmployeeDto>, ErrorList, GetEmployeesByCompanyQuery> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(new GetEmployeesByCompanyQuery(id), cancellationToken),
            StatusCodes.Status200OK);
    }

    /// <summary>
    /// GetByDepartmentAsync - вернёт список сотрудников департамента компании
    /// </summary>
    /// <param name="companyId">Id компании для поиска сотрудников</param>
    /// <param name="departmentId">Id департамента компании для поиска сотрудников</param>
    /// <param name="handler">Обработчик запроса</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Вернёт Envelope со списком сотрудников или вернёт сообщение об ошибке</returns>
    [HttpGet("/company/{companyId:int}/department/{departmentId:int}/employees")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<CustomResult<IEnumerable<EmployeeDto>>> GetByDepartmentAsync(
        [FromRoute] int companyId,
        [FromRoute] int departmentId,
        [FromServices] IQueryHandler<IEnumerable<EmployeeDto>, ErrorList, GetEmployeesByDepartmentQuery> handler,
        CancellationToken cancellationToken)
    {
        return (
            await handler.HandleAsync(new GetEmployeesByDepartmentQuery(companyId, departmentId), cancellationToken),
            StatusCodes.Status200OK);
    }
}