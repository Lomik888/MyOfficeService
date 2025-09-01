using MyOfficeService.Application.Dtos;

namespace MyOfficeService.Application.Repositories;

public interface ICompanyRepository
{
    /// <summary>
    /// GetEmployeesAsync - возвращает всех сотрудников компании без пагинации
    /// </summary>
    /// <param name="companyId">Id компании</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт коллекцию EmployeeDto</returns>
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
        int companyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GetEmployeesAsync - возвращает всех сотрудников департамента компании без пагинации
    /// </summary>
    /// <param name="companyId">Id компании</param>
    /// <param name="departmentId">Id департамента компании</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт коллекцию EmployeeDto</returns>
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(
        int companyId,
        int departmentId,
        CancellationToken cancellationToken = default);
}