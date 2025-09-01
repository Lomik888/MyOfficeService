using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Database;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Extations;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;

namespace MyOfficeService.Application.Employees.Commands.Delete;

public class DeleteEmployeeHandler : ICommandHandler<ErrorList, DeleteEmployeeCommand>
{
    private readonly IValidator<DeleteEmployeeCommand> _validator;
    private readonly ILogger<DeleteEmployeeHandler> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITransactionManager _transactionManager;

    public DeleteEmployeeHandler(
        IValidator<DeleteEmployeeCommand> validator,
        IEmployeeRepository employeeRepository,
        ITransactionManager transactionManager,
        ILogger<DeleteEmployeeHandler> logger)
    {
        _validator = validator;
        _employeeRepository = employeeRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(
        DeleteEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("{0}", validationResult.Errors);
            return validationResult.GetErrorList();
        }

        var openTransactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (openTransactionResult.IsSuccess == false)
        {
            return openTransactionResult.Error.ToErrorList();
        }

        await using var transaction = openTransactionResult.Value;

        try
        {
            var deleteEmployeeResult = await _employeeRepository.DeleteAsync(command.Id, cancellationToken);
            if (deleteEmployeeResult.IsSuccess == false)
            {
                await transaction.RollbackAsync(cancellationToken);
                return deleteEmployeeResult.Error.ToErrorList();
            }

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Сотрудник Id: {0} был удалён", command.Id);
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