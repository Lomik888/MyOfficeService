namespace MyOfficeService.Presentation.Responses;

/// <summary>
/// ErrorResponse - dto для ответа пользователю
/// </summary>
public record ErrorResponse(string ErrorMessage, string ErrorCode, string ErrorType);