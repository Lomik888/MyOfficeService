using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Application.Repositories;
using MyOfficeService.Domain.Entities;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Infrastructure.Postgres.Repositories;

/// <summary>
/// EmployeeRepository - репозиторий, для работы с бд, принимает сущности, определённые в коде
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly DapperDbContext _dbContext;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(DapperDbContext dbContext, ILogger<EmployeeRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// AddEmployeeAsync - добавляет нового пользователя в уже существующую компанию и департамент
    /// </summary>
    /// <param name="employee">Сущность, для добавления в БД</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт Result<int, Error> где int это Id нового сотрудника, а Error ошибка при добавлении</returns>
    public async Task<Result<int, Error>> AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        await _dbContext.OpenConnectionAsync(cancellationToken);

        var sql = """
                  insert into employees (name, phone, company_id, department_id, passport)
                  values (@name, @phone, @company_id, @department_id, @passport::jsonb)
                  returning id;
                  """;

        var passport = JsonSerializer.Serialize(employee.Passport);

        var parameters = new DynamicParameters();
        parameters.Add("@name", employee.Name);
        parameters.Add("@phone", employee.Phone.Number);
        parameters.Add("@company_id", employee.CompanyId);
        parameters.Add("@department_id", employee.DepartmentId);
        parameters.Add("@passport", passport);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        _logger.LogInformation("{0}", sql); // Решить проблему с сенситив данными
        var employeeId = await _dbContext.Database.ExecuteScalarAsync<int?>(command);
        if (employeeId is null)
        {
            return GeneralErrors.Database.CantCreate($"Невозможно создать пользователя {employee.Name}");
        }

        return (int)employeeId;
    }

    /// <summary>
    /// DeleteAsync - удалит сотрудника по его Id
    /// </summary>
    /// <param name="id">Id сотрудника</param>
    /// <returns>Вернёт успешный результат или детальный объект ошибки</returns>
    public async Task<UnitResult<Error>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _dbContext.OpenConnectionAsync(cancellationToken);

        var sql = """
                  delete from employees
                  where id = @id;
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@id", id);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        _logger.LogInformation("{0}", sql);
        var row = await _dbContext.Database.ExecuteAsync(command);
        if (row == 0)
        {
            return GeneralErrors.Database.NotFound($"Пользователя с Id {id} нет в базе данных");
        }

        return UnitResult.Success<Error>();
    }

    /// <summary>
    /// UpdateAsync - метод, который обновляет данные о пользователе
    /// </summary>
    /// <param name="dto">Dto в которой лежат данные для обновления и Id сотрудника</param>
    /// <returns>Вернёт успешный результат или детальный объект ошибки</returns>
    public async Task<UnitResult<Error>> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken)
    {
        await _dbContext.OpenConnectionAsync(cancellationToken);

        var sql = """
                  update employees 
                  """;

        var sqlBuilder = new StringBuilder(sql);

        sqlBuilder.Append(" set ");

        var parameters = new DynamicParameters();

        if (dto.Name != null)
        {
            sqlBuilder.Append(" name = @name ");
            parameters.Add("@name", dto.Name);
        }

        if (dto.Phone != null)
        {
            sqlBuilder.Append(", phone = @phone");
            parameters.Add("@phone", dto.Phone);
        }

        if (dto.CompanyId != null)
        {
            sqlBuilder.Append(", company_id = @company_id");
            parameters.Add("@company_id", dto.CompanyId);
        }

        if (dto.DepartmentId != null)
        {
            sqlBuilder.Append(", department_id = @department_id");
            parameters.Add("@department_id", dto.DepartmentId);
        }

        if (dto.Passport != null)
        {
            var passportString = JsonSerializer.Serialize(dto.Passport);
            sqlBuilder.Append(", passport = @passport::jsonb");
            parameters.Add("@passport", passportString);
        }

        sqlBuilder.Append(" where id = @id;");
        parameters.Add("@id", dto.Id);

        sql = sqlBuilder.ToString();

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        _logger.LogInformation("{0}", sql);
        var row = await _dbContext.Database.ExecuteAsync(command);
        if (row == 0)
        {
            return GeneralErrors.Database.NotFound($"Пользователя с Id {dto.Id} нет в базе данных");
        }

        return UnitResult.Success<Error>();
    }
}