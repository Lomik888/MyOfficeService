using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Domain.Entities;

/// <summary>
/// Company — корневая сущность
/// Инварианты:
/// - Name не может быть больше 150 символов
/// - Phone это ValueObject с собственной валидацией
/// Связи:
/// - Company содержит много Employee
/// - Company содержит много Department
/// Правила:
/// - При создании через Create, если Id не задан, он равен -1 (ещё не сохранён в БД)
/// Методы:
/// - Create — фабрика с валидацией входных данных
/// - Validate — проверка ограничений на поля
/// </summary>
public class Company
{
    private const int MAX_NAME_LENGTH = 150;

    public int Id { get; }

    public string Name { get; private set; }

    public Phone Phone { get; private set; }

    private Company(int id, string name, Phone phone)
    {
        Id = id;
        Name = name;
        Phone = phone;
    }

    public static Result<Company, ErrorList> Create(string name, Phone phone, int id = -1)
    {
        var validationResult = Validate(name);
        if (validationResult.IsSuccess == false)
        {
            return validationResult.Error;
        }

        return new Company(id, name, phone);
    }

    public static UnitResult<ErrorList> Validate(string name)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name) == true)
        {
            var error = GeneralErrors.Validation.InvalidField("Имя компании не может быть пустым");
            errors.Add(error);
        }

        if (name.Length > MAX_NAME_LENGTH)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Имя компании должно не превышать {MAX_NAME_LENGTH} символов");
            errors.Add(error);
        }

        return errors.Count == 0 ? UnitResult.Success<ErrorList>() : errors.ToErrorList();
    }
}