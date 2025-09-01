using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Domain.Entities;

/// <summary>
/// Employee — сотрудники
/// Инварианты:
/// - Name не может быть больше 50 символов
/// - Phone — ValueObject с собственной валидацией
/// - Employee не может существовать без Company
/// - Employee не может существовать без Department
/// - Passport это ValueObject с собственной валидацией
/// Связи:
/// - Employee принадлежит одной Company
/// - Employee принадлежит одному Department
/// Правила:
/// - При создании через Create, если Id не задан, он равен -1 (ещё не сохранён в БД)
/// Методы:
/// - Create — фабрика с валидацией входных данных
/// - Validate — проверка ограничений на поля
/// </summary>
public class Employee
{
    public const int MAX_NAME_LENGTH = 50;

    public int Id { get; }

    public string Name { get; private set; }

    public Phone Phone { get; private set; }

    public int CompanyId { get; private set; }

    public int DepartmentId { get; private set; }

    public Passport Passport { get; private set; }

    private Employee(
        int id,
        string name,
        Phone phone,
        int companyId,
        int departmentId,
        Passport passport)
    {
        Id = id;
        Name = name;
        Phone = phone;
        CompanyId = companyId;
        DepartmentId = departmentId;
        Passport = passport;
    }

    public static Result<Employee, ErrorList> Create(
        string name,
        Phone phone,
        int companyId,
        int departmentId,
        Passport passport,
        int id = -1)
    {
        var validationResult = Validate(name, companyId, departmentId);
        if (validationResult.IsSuccess == false)
        {
            return validationResult.Error.ToErrorList();
        }

        return new Employee(id, name, phone, companyId, departmentId, passport);
    }

    public static UnitResult<ErrorList> Validate(
        string name,
        int companyId,
        int departmentId)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name) == true)
        {
            var error = GeneralErrors.Validation.InvalidField("Имя сотрудника не может быть пустым");
            errors.Add(error);
        }

        if (name.Length > MAX_NAME_LENGTH)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Имя сотрудника должно не превышать {MAX_NAME_LENGTH} символов");
            errors.Add(error);
        }

        if (companyId <= 0)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Не указано id компании {companyId}");
            errors.Add(error);
        }

        if (departmentId <= 0)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Не указано id отдела {departmentId}");
            errors.Add(error);
        }

        return errors.Count == 0 ? UnitResult.Success<ErrorList>() : errors.ToErrorList();
    }
}