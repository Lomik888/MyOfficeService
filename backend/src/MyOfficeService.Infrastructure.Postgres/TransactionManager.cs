using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using MyOfficeService.Application.Abstractions.Database;
using MyOfficeService.Application.Abstractions.Enums;
using MyOfficeService.Domain.Errors;

namespace MyOfficeService.Infrastructure.Postgres;

/// <summary>
/// TransactionManager - класс, для работы с транзакциями
/// </summary>
public class TransactionManager : ITransactionManager
{
    private readonly DapperDbContext _dbContext;
    private readonly ILogger<TransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionManager(
        DapperDbContext dbContext,
        ILogger<TransactionManager> logger,
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// BeginTransactionAsync - Открывает транзакцию 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="transactionIsolationLevel"></param>
    /// <returns>Возарвщает Result<ITransaction, Error>, если удачно то вернёт ITransaction обёртка над DbTransaction
    /// Иначе детальную ошибку о том, почему не была открыта транзакция.</returns>
    public async Task<Result<ITransaction, Error>> BeginTransactionAsync(
        CancellationToken cancellationToken,
        TransactionIsolationLevel transactionIsolationLevel = TransactionIsolationLevel.READ_COMMITTED)
    {
        try
        {
            await _dbContext.OpenConnectionAsync(cancellationToken);

            var isolationLevel = GetIsolationLevel(transactionIsolationLevel);
            var dbTransaction = await _dbContext.Database.BeginTransactionAsync(
                isolationLevel,
                cancellationToken);

            var connectionId = _dbContext.Database.GetHashCode();

            _logger.LogInformation(
                "Транзакция стартанула уровень изоляции {IsolationLevel} id соединения {ConnectionId}",
                isolationLevel,
                connectionId);
            var transaction = new Transaction(dbTransaction, _loggerFactory, connectionId);

            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{0}", ex.Message);
            return GeneralErrors.Database.TransactionError(ex.Message);
        }
    }

    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

    private IsolationLevel GetIsolationLevel(TransactionIsolationLevel transactionIsolationLevel)
    {
        var isolationLevel = transactionIsolationLevel switch
        {
            TransactionIsolationLevel.READ_COMMITTED => IsolationLevel.ReadCommitted,
            TransactionIsolationLevel.READ_UNCOMMITTED => IsolationLevel.ReadUncommitted,
            TransactionIsolationLevel.REPEATABLE_READ => IsolationLevel.RepeatableRead,
            TransactionIsolationLevel.SERIALIZABLE => IsolationLevel.Serializable,
            _ => IsolationLevel.ReadCommitted
        };

        return isolationLevel;
    }
}