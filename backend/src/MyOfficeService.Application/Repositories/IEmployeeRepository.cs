using CSharpFunctionalExtensions;
using MyOfficeService.Application.Dtos;
using MyOfficeService.Domain.Entities;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Application.Repositories;

public interface IEmployeeRepository
{
    /// <summary>
    /// AddAsync - добавляет нового пользователя в уже существующую компанию и департамент
    /// </summary>
    /// <param name="employee">Сущность, для добавления в БД</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт Result типа int и Error, где int — это Id нового сотрудника,
    /// а Error — ошибка при добавлении</returns>
    Task<Result<int, Error>> AddAsync(Employee employee, CancellationToken cancellationToken);

    /// <summary>
    /// DeleteAsync - удаляет сотрудника из БД по id
    /// </summary>
    /// <param name="id">Id сотрудника</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт UnitResult типа Error, те вернёт успешное выполнение или детализированную ошибку</returns>
    Task<UnitResult<Error>> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// UpdateAsync - обновляет данные о сотруднике, которые не null в UpdateEmployeeDto
    /// </summary>
    /// <param name="dto">Данные для обновления</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт UnitResult типа Error, те вернёт успешное выполнение или детализированную ошибку</returns>
    Task<UnitResult<Error>> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken);
}