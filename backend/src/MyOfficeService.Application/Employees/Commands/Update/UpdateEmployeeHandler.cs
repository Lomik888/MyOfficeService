using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Database;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Application.Extations;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Application.Employees.Commands.Update;

public class UpdateEmployeeHandler : ICommandHandler<ErrorList, UpdateEmployeeCommand>
{
    private readonly IValidator<UpdateEmployeeCommand> _validator;
    private readonly ILogger<UpdateEmployeeHandler> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITransactionManager _transactionManager;

    public UpdateEmployeeHandler(
        IValidator<UpdateEmployeeCommand> validator,
        IEmployeeRepository employeeRepository,
        ITransactionManager transactionManager,
        ILogger<UpdateEmployeeHandler> logger)
    {
        _validator = validator;
        _employeeRepository = employeeRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(
        UpdateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("{0}", validationResult.Errors);
            return validationResult.GetErrorList();
        }

        var updateEmployeeDtoResult = UpdateEmployeeDto.Create(
            command.Id,
            command.Name,
            command.Phone,
            command.CompanyId,
            command.DepartmentId,
            command.PassportDto == null
                ? null
                : Passport.Create(command.PassportDto.Type, command.PassportDto.Number).Value);
        if (updateEmployeeDtoResult.IsSuccess == false)
        {
            return updateEmployeeDtoResult.Error;
        }

        var updateEmployeeDto = updateEmployeeDtoResult.Value;

        var openTransactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (openTransactionResult.IsSuccess == false)
        {
            return openTransactionResult.Error.ToErrorList();
        }

        await using var transaction = openTransactionResult.Value;

        try
        {
            var addEmployeeResult = await _employeeRepository.UpdateAsync(updateEmployeeDto, cancellationToken);
            if (addEmployeeResult.IsSuccess == false)
            {
                await transaction.RollbackAsync(cancellationToken);
                return addEmployeeResult.Error.ToErrorList();
            }

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Сотрудник Id: {0} был обновлён", command.Id);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            var error = GeneralErrors.Database.TransactionError(ex.Message);

            _logger.LogError(ex, "{0}", error.Message);
            return error.ToErrorList();
        }
    }
}