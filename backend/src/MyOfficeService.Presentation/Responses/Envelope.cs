using MyOfficeService.Domain.Enums;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Presentation.Responses;

/// <summary>
/// Envelope - класс, который всегда возвращается пользователю
/// </summary>
/// <typeparam name="T"></typeparam>
public class Envelope<T>
{
    public T? Result { get; private set; }

    public IReadOnlyList<ErrorResponse>? Errors { get; private set; }

    public DateTime TimeGenerated { get; private set; }

    private Envelope(T? result, IEnumerable<ErrorResponse>? errors, DateTime timeGenerated)
    {
        Result = result;
        Errors = errors?.ToList();
        TimeGenerated = timeGenerated;
    }

    public int GetStatusCode()
    {
        if (Errors?.Count == 0)
            throw new Exception("Envelope Errors пуст");

        var types = Errors!.Select(x => x.ErrorType).Distinct().ToList();
        var statusCode = types.Count > 1 ? StatusCodes.Status500InternalServerError : GetStatusCode(types[0]);

        return statusCode;
    }

    private static Envelope<T> Create(T? result, IEnumerable<Error>? errors)
    {
        var timeGenerated = DateTime.UtcNow;

        var errorResponses = errors?.Select(x =>
            new ErrorResponse(x.Message, x.Code, x.Type.ToString()));

        var envelope = new Envelope<T>(result, errorResponses, timeGenerated);

        return envelope;
    }

    public static Envelope<T> Ok(T? result) => Envelope<T>.Create(result, null);

    public static Envelope<T> Error(ErrorList errors) => Envelope<T>.Create(default, errors);

    private int GetStatusCode(string errorType)
    {
        if (Enum.TryParse<ErrorTypes>(errorType, out var type) == false)
            throw new Exception("Неверный тип ошибки");

        return type switch
        {
            ErrorTypes.VALIDATION => StatusCodes.Status400BadRequest,
            ErrorTypes.DB => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}