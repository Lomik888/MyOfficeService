using MyOfficeService.Domain.Enums;

namespace MyOfficeService.Domain.Errors;

/// <summary>
/// Error — класс, представляющий ошибку.
/// Инварианты:
/// - Должен содержать Message (текст ошибки)
/// - Code должен содержать уникальный код ошибки
/// - ErrorType указывает на тип ошибки
/// Правила:
/// - При Serialize свойства объединяются через SEPARATOR
/// - При Deserialize проверяется, что входящая строка — это сериализованный Error
/// Методы:
/// - Create — фабрика с валидацией входных данных, позволяет создать свою ошибку
/// - Serialize — кладёт все данные об ошибке в строку, разделяя SEPARATOR
/// - Deserialize — из сериализованного Error создаёт объект
/// </summary>
public class Error
{
    private const string SEPARATOR = " | ";

    private const int COUNT_PARAMS = 3;

    /// <summary>
    /// Текст ошибки
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Код ошибки
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Тип ошибки
    /// </summary>
    public ErrorTypes Type { get; private set; }

    private Error(string message, string code, ErrorTypes type)
    {
        Message = message;
        Code = code;
        Type = type;
    }

    /// <summary>
    /// Можно создать свою ошибку
    /// </summary>
    /// <exception cref="ArgumentNullException">Вернёт исключение, если передать message или code пустым</exception>
    public static Error Create(string message, string code, ErrorTypes type)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Сообщение в ошибке не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentNullException(nameof(code), "Код в ошибке не может быть пустым");
        }

        return new Error(message, code, type);
    }

    public string Serialize()
    {
        var errorString = string.Join(SEPARATOR, this.Message, this.Code, this.Type);
        return errorString;
    }

    public static Error Deserialize(string errorString)
    {
        if (string.IsNullOrWhiteSpace(errorString))
            throw new ArgumentException("errorString не может быть пустым или null");

        var paramsError = errorString.Split(SEPARATOR);

        if (paramsError.Length != COUNT_PARAMS)
            throw new ArgumentException("errorString не Error");

        if (Enum.TryParse<ErrorTypes>(paramsError[COUNT_PARAMS - 1], out var errorType) == false)
            throw new ArgumentException("ErrorType invalid");

        var error = new Error(paramsError[0], paramsError[1], errorType);

        return error;
    }
}