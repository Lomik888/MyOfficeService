using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;

namespace MyOfficeService.Domain.ValueObjects;

/// <summary>
/// Phone — ValueObject для телефонного номера
/// Инварианты:
/// - Number должен состоять из 10 цифр(не считает префикса) и начинаться с +7 или 8 
/// Методы:
/// - Create — фабрика с валидацией входных данных
/// - Validate — проверка ограничений на поля
/// </summary>
public class Phone : ValueObject
{
    private const string REGEX_PHONE_PATTERN = "^(?:\\+7|8)\\d{10}$";
    private static readonly Regex _regex = new Regex(REGEX_PHONE_PATTERN, RegexOptions.Compiled);

    public string Number { get; }

    private Phone(string number)
    {
        Number = number;
    }

    public static Result<Phone, ErrorList> Create(string number)
    {
        var validationResult = Validate(number);
        if (validationResult.IsSuccess == false)
        {
            return validationResult.Error;
        }

        return new Phone(number);
    }

    public static UnitResult<ErrorList> Validate(string number)
    {
        return _regex.IsMatch(number) == true
            ? UnitResult.Success<ErrorList>()
            : GeneralErrors.Validation.InvalidField($"Неверный номер телефона").ToErrorList();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}