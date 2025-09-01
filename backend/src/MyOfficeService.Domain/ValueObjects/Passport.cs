using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;

namespace MyOfficeService.Domain.ValueObjects;

/// <summary>
/// Passport — это ValueObject
/// Инварианты:
/// - Type имеет ограничения на 50 символов
/// - Number имеет ограничения на 10 символов и только цирфы
/// Методы:
/// - Create — фабрика с валидацией входных данных
/// - Validate — проверка ограничений на поля
/// </summary>
public class Passport : ValueObject
{
    private const int MAX_TYPE_LENGTH = 50;
    private const string REGEX_PASSPORT_PATTERN = "^\\d{10}$";
    private static readonly Regex _regex = new Regex(REGEX_PASSPORT_PATTERN, RegexOptions.Compiled);

    public string Type { get; private set; }
    
    public string Number { get; private set; }

    private Passport(string type, string number)
    {
        Type = type;
        Number = number;
    }

    public static Result<Passport, ErrorList> Create(string type, string number)
    {
        var validationResult = Validate(type, number);
        if (validationResult.IsSuccess == false)
        {
            return validationResult.Error;
        }

        return new Passport(type, number);
    }

    /// <summary>
    /// Метод валидации данных, для создания ValueObject
    /// </summary>
    public static UnitResult<ErrorList> Validate(string type, string number)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(type) == true)
        {
            var error = GeneralErrors.Validation.InvalidField("Тип Паспорта не может быть пустым");
            errors.Add(error);
        }

        if (type.Length > MAX_TYPE_LENGTH)
        {
            var error = GeneralErrors.Validation
                .InvalidField($"Тип Паспорта должен не превышать {MAX_TYPE_LENGTH} символов");
            errors.Add(error);
        }

        if (_regex.IsMatch(number) == false)
        {
            var error = GeneralErrors.Validation.InvalidField("Проверьте номер паспорта");
            errors.Add(error);
        }

        return errors.Count == 0 ? UnitResult.Success<ErrorList>() : errors.ToErrorList();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Number;
    }
}