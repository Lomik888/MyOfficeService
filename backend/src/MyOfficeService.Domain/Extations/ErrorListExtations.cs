using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Domain.Extations;

/// <summary>
/// ErrorListExtations - класс расширения, чтобы удобнее парсить ответ в формат ErrorList
/// </summary>
public static class ErrorListExtations
{
    public static ErrorList ToErrorList(this IEnumerable<Error> errors)
    {
        return new ErrorList(errors.ToList());
    }

    public static ErrorList ToErrorList(this Error error)
    {
        return new ErrorList([error]);
    }
}