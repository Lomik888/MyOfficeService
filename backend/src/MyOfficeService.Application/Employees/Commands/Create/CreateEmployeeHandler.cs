using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Database;
using MyOfficeService.Application.Abstractions.Handlers;
using MyOfficeService.Application.Extations;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Domain.Entities;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Application.Employees.Commands.Create;

public class CreateEmployeeHandler : ICommandHandler<int, ErrorList, CreateEmployeeCommand>
{
    private readonly IValidator<CreateEmployeeCommand> _validator;
    private readonly ILogger<CreateEmployeeHandler> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITransactionManager _transactionManager;

    public CreateEmployeeHandler(
        IValidator<CreateEmployeeCommand> validator,
        IEmployeeRepository employeeRepository,
        ITransactionManager transactionManager,
        ILogger<CreateEmployeeHandler> logger)
    {
        _validator = validator;
        _employeeRepository = employeeRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<int, ErrorList>> HandleAsync(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("{0}", validationResult.Errors);
            return validationResult.GetErrorList();
        }

        var phone = Phone.Create(command.Phone).Value;
        var passport = Passport.Create(command.PassportType, command.PassportNumber).Value;
        var employee = Employee.Create(
            command.Name,
            phone,
            command.CompanyId,
            command.DepartmentId,
            passport).Value;

        var openTransactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (openTransactionResult.IsSuccess == false)
        {
            return openTransactionResult.Error.ToErrorList();
        }

        await using var transaction = openTransactionResult.Value;

        try
        {
            var addEmployeeResult = await _employeeRepository.AddAsync(employee, cancellationToken);
            if (addEmployeeResult.IsSuccess == false)
            {
                await transaction.RollbackAsync(cancellationToken);
                return addEmployeeResult.Error.ToErrorList();
            }

            await transaction.CommitAsync(cancellationToken);

            var employeeId = addEmployeeResult.Value;

            _logger.LogInformation("Сотрудник Id: {0} был добавлен", employeeId);
            return employeeId;
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