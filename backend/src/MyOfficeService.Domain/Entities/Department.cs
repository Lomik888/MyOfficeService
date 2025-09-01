using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Domain.Entities;

/// <summary>
/// Department — департаменты
/// Инварианты:
/// - Name не может быть больше 150 символов
/// - Phone это ValueObject с собственной валидацией
/// - Department не может существовать без Company
/// Связи:
/// - Есть много Department у одной Company
/// - Department содержит много Employee
/// Правила:
/// - При создании через Create, если Id не задан, он равен -1 (ещё не сохранён в БД)
/// Методы:
/// - Create — фабрика с валидацией входных данных
/// - Validate — проверка ограничений на поля
/// </summary>
public class Department
{
    private const int MAX_NAME_LENGTH = 150;

    public int Id { get; }

    public string Name { get; private set; }

    public Phone Phone { get; private set; }

    public int CompanyId { get; private set; }

    private Department(int id, string name, Phone phone, int companyId)
    {
        Id = id;
        Name = name;
        Phone = phone;
        CompanyId = companyId;
    }

    public static Result<Department, ErrorList> Create(string name, Phone phone, int companyId, int id = -1)
    {
        var validationResult = Validate(name, companyId);
        if (validationResult.IsSuccess == false)
        {
            return validationResult.Error;
        }

        return new Department(id, name, phone, companyId);
    }

    public static UnitResult<ErrorList> Validate(string name, int companyId)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name) == true)
        {
            var error = GeneralErrors.Validation.InvalidField("Имя департамента не может быть пустым");
            errors.Add(error);
        }

        if (name.Length > MAX_NAME_LENGTH)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Имя департамента должно не превышать {MAX_NAME_LENGTH} символов");
            errors.Add(error);
        }

        if (companyId <= 0)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Не указано id компании {companyId}");
            errors.Add(error);
        }

        return errors.Count == 0 ? UnitResult.Success<ErrorList>() : errors.ToErrorList();
    }
}