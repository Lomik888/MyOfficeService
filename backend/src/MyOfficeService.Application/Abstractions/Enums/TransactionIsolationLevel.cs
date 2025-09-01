namespace MyOfficeService.Application.Abstractions.Enums;

public enum TransactionIsolationLevel
{
    READ_COMMITTED,
    READ_UNCOMMITTED,
    REPEATABLE_READ,
    SERIALIZABLE
}