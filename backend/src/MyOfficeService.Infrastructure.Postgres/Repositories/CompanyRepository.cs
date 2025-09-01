using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Application.Repositories;

namespace MyOfficeService.Infrastructure.Postgres.Repositories;

/// <summary>
/// CompanyRepository - репозиторий, для работы с бд, принимает сущности, определённые в коде
/// </summary>
public class CompanyRepository : ICompanyRepository
{
    private readonly DapperDbContext _dbContext;
    private readonly ILogger<CompanyRepository> _logger;

    public CompanyRepository(DapperDbContext dbContext, ILogger<CompanyRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// GetEmployeesAsync - вернёт всех сотрудников компании по Id компании без пагинации
    /// </summary>
    /// <param name="id">Id компании</param>
    /// <returns>Вернёт список EmployeeDto</returns>
    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.OpenConnectionAsync(cancellationToken);

        var sql = """
                    select 
                         e.name,
                         e.phone,
                         e.company_id,
                         e.department_id,
                         e.passport
                  from companies as c
                           inner join employees as e on c.id = e.company_id
                  where c.id = @id;
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@id", id);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        _logger.LogInformation("{0}", sql);
        var employees = await _dbContext.Database.QueryAsync<EmployeeDto, string, EmployeeDto>(
            command,
            (emp, str) =>
            {
                var passport = JsonSerializer.Deserialize<PassportDto>(
                    str,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                emp.PassportType = passport!.Type;
                emp.PassportNumber = passport.Number;
                return emp;
            },
            "passport");

        return employees;
    }

    /// <summary>
    /// GetEmployeesAsync - вернёт всех сотрудников департамента компании по Id копании и департамента без пагинации
    /// </summary>
    /// <param name="companyId">Id компании</param>
    /// <param name="departmentId">Id департаменты</param>
    /// <returns>Вернёт коллекцию EmployeeDto</returns>
    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
        int companyId,
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.OpenConnectionAsync(cancellationToken);

        var sql = """
                  select
                      e.name,
                      e.phone,
                      e.company_id,
                      e.department_id,
                      e.passport
                  from companies as c
                           inner join employees as e on c.id = e.company_id
                  where c.id = @company_id and e.department_id = @department_id;
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@company_id", companyId);
        parameters.Add("@department_id", departmentId);

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        _logger.LogInformation("{0}", sql);
        var employees = await _dbContext.Database.QueryAsync<EmployeeDto, string, EmployeeDto>(
            command,
            (emp, str) =>
            {
                var passport = JsonSerializer.Deserialize<PassportDto>(
                    str,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                emp.PassportType = passport!.Type;
                emp.PassportNumber = passport.Number;
                return emp;
            },
            "passport");

        return employees;
    }
}