using MyOfficeService.Domain.Enums;

namespace MyOfficeService.Domain.Errors;

/// <summary>
/// GeneralErrors — общий каталог предопределённых ошибок (фабрика ошибок)
/// Содержит методы для создания стандартных ошибок валидации и ошибок базы данных.
/// </summary>
public static class GeneralErrors
{
    /// <summary>
    /// Заготовки для ошибок валидации
    /// </summary>
    public static class Validation
    {
        public static Error InvalidField(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            return Error.Create(message, ErrorCodes.Validation.INVALID_FIELD, ErrorTypes.VALIDATION);
        }
    }

    /// <summary>
    /// Заготовки для ошибок Сервера
    /// </summary>
    public static class Server
    {
        public static Error ServerError(string message)
        {
            return Error.Create(
                message,
                ErrorCodes.Server.SERVER_ERROR,
                ErrorTypes.SERVER);
        }
    }

    /// <summary>
    /// Заготовки для ошибок БД
    /// </summary>
    public static class Database
    {
        public static Error NotFound(string message)
        {
            return Error.Create(
                message,
                ErrorCodes.Database.NOT_FOUND,
                ErrorTypes.DB);
        }

        public static Error TransactionError(string message)
        {
            return Error.Create(
                message,
                ErrorCodes.Database.TRANSACTION_ERROR,
                ErrorTypes.DB);
        }

        public static Error CantCreate(string message)
        {
            return Error.Create(
                message,
                ErrorCodes.Database.CAN_NOT_CREATE,
                ErrorTypes.DB);
        }
    }
}