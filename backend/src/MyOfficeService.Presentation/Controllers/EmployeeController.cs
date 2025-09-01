using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Employees.Commands.Create;
using MyOfficeService.Application.Employees.Commands.Delete;
using MyOfficeService.Application.Employees.Commands.Update;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Presentation.Abstractions;
using MyOfficeService.Presentation.Requests;
using MyOfficeService.Presentation.Responses;

namespace MyOfficeService.Presentation.Controllers;

public class EmployeeController : BaseApplicationController
{
    /// <summary>
    /// CreateAsync - создание сотрудника
    /// </summary>
    /// <param name="request">Данные из тела запроса для создания сотрудника</param>
    /// <param name="handler">Обработчик</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Вернёт Envelope с Id сотрудника или ошибку</returns>
    [HttpPost("/employee")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<CustomResult<int>> CreateAsync(
        [FromBody] CreateEmployeeRequest request,
        [FromServices] ICommandHandler<int, ErrorList, CreateEmployeeCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(request.ToCommand(), cancellationToken), StatusCodes.Status201Created);
    }

    /// <summary>
    /// DeleteAsync - удаление сотрудника
    /// </summary>
    /// <param name="id">Id сотрудника из route</param>
    /// <param name="handler">Обработчик запроса</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Вернёт Envelope без ошибки при удачном запросе и с ошибкой при неудачно</returns>
    [HttpDelete("/employee/{id::int}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<CustomResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] ICommandHandler<ErrorList, DeleteEmployeeCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(new DeleteEmployeeCommand(id), cancellationToken), StatusCodes.Status200OK);
    }

    /// <summary>
    /// UpdateAsync - удаление сотрудника
    /// </summary>
    /// <param name="id">Id сотрудника для удаления</param>
    /// <param name="request">Данные из тела запроса для обновления сотруднкиа</param>
    /// <param name="handler">Обработчик запроса</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Вернёт Envelope без ошибки при удачном запросе и с ошибкой при неудачно</returns>
    [HttpPatch("/employee-info/{id::int}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<CustomResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateEmployeeRequest request,
        [FromServices] ICommandHandler<ErrorList, UpdateEmployeeCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(request.ToCommand(id), cancellationToken), StatusCodes.Status200OK);
    }
}