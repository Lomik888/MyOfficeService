using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;

namespace MyOfficeService.Application.Extations;

public static class FluentValidationExtations
{
    /// <summary>
    /// CustomMust - кастомный метод, для работы с сущностями и их валидацией.
    /// Он добавляет в context FluentValidation сериализованную Error из Domain
    /// </summary>
    /// <param name="predicate">Метод валидации</param>
    /// <typeparam name="T">Объект</typeparam>
    /// <typeparam name="TProperty">Свойства Объекта</typeparam>
    public static IRuleBuilderOptionsConditions<T, TProperty> Must<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, UnitResult<ErrorList>> predicate)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = predicate(value);

            if (result.IsFailure == false)
            {
                return;
            }

            foreach (var error in result.Error)
            {
                var errorString = error.Serialize();
                context.AddFailure(errorString);
            }
        });
    }

    public static ErrorList GetErrorList(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => Error.Deserialize(e.ErrorMessage));
        return errors.ToErrorList();
    }

    /// <summary>
    /// WithMessage - перегрузка, чтобы возвращать свои ошибки
    /// </summary>
    /// <param name="rule"></param>
    /// <param name="error"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, TProperty> WithMessage<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        Error error)
    {
        return rule.WithMessage(error.Serialize());
    }
}