namespace MyOfficeService.Domain.Enums;

/// <summary>
/// ErrorTypes - типы ошибок
/// </summary>
public enum ErrorTypes
{
    /// <summary>
    /// Ошибка валидации
    /// </summary>
    VALIDATION,

    /// <summary>
    /// Ошибка БД
    /// </summary>
    DB,

    /// <summary>
    /// Ошибка сервера
    /// </summary>
    SERVER
}