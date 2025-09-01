namespace MyOfficeService.Domain.Enums;

/// <summary>
/// ErrorCodes — общий каталог предопределённых кодов ошибок (фабрика кодов ошибок)
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Коды ошибок для валидации
    /// </summary>
    public static class Validation
    {
        public const string INVALID_FIELD = "invalid.field";
    }

    /// <summary>
    /// Коды ошибок для сервера
    /// </summary>
    public static class Server
    {
        public const string SERVER_ERROR = "server.error";
    }

    /// <summary>
    /// Коды ошибок для БД
    /// </summary>
    public static class Database
    {
        public const string NOT_FOUND = "not.found";
        public const string CAN_NOT_CREATE = "can.not.create";
        public const string TRANSACTION_ERROR = "transaction.error";
    }
}