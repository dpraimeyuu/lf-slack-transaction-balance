namespace Lf.Slack.TransactionBalance.Infrastructure;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Lf.Slack.TransactionBalance.Domain;
using SQLite;
using Microsoft.Data.Sqlite;

[Table("TransactionBalance")]
public class PersistableTransactionBalance
{
    [PrimaryKey]
    public string Id { get; set; }
    public decimal Amount { get; set; }
}
[Table("Refund")]
public record PersistableRefund
{
    [PrimaryKey]
    public string Id { get; set; }
    public string Status { get; set; }
    public string Amount { get; set; }
    public string TransactionId { get; set; }
}

public record TransactionBalanceState : ITransactionBalanceState
{
    public string TransactionId { get; set; }
    public string TransactionAmount { get; set; }
    public IReadOnlyList<IRefundState> Refunds { get; set; }
}

public record RefundState : IRefundState
{
    public string Id { get; set; }
    public string RefundStatus { get; set; }
    public string RefundAmount { get; set; }
}
public class TransactionBalanceRelationalRepository : ITransactionBalanceRepository
{
    private string _connString;

    public TransactionBalanceRelationalRepository(string connString)
    {
        _connString = connString;
    }

    private string ToRefundsInsertSql(string transactionId, IEnumerable<Refund> refunds)
    {
        if(!refunds.Any()) return "";

        var refundsValues =
            refunds
            .Select(refund => $"('{refund.Id}', '{transactionId}', {refund.RefundAmount}, '{refund.Status}')");
        var refundUpdates =
            refunds
            .Select(refund => $"UPDATE [Refund] SET Status = '{refund.Status}' WHERE Id = '{refund.Id}'");
        return $@"
            INSERT OR IGNORE INTO [Refund]([Id], [TransactionId], [Amount], [Status]) VALUES
            {string.Join(",\n", refundsValues)};
            {string.Join(";\n", refundUpdates)}
        ";
    }
    public async Task Save(TransactionBalance transactionBalance)
    {
        using IDbConnection db = new SqliteConnection(_connString);
        string insertSql = $@"
            INSERT OR IGNORE INTO [TransactionBalance]([Id], [Amount]) VALUES ('{transactionBalance.Id}', {transactionBalance.TransactionBalanceDetails.TransactionAmount});
            {ToRefundsInsertSql(transactionBalance.Id.ToString(), transactionBalance.Refunds)}        
        ";

        await db.ExecuteAsync(insertSql);
    }

    public async Task<TransactionBalance> GetByRefundId(Guid refundId)
    {
        using IDbConnection db = new SqliteConnection(_connString);


        string transactionBalanceQuerySql = $@"
            SELECT t.Id, t.Amount FROM [TransactionBalance] as t
            LEFT JOIN [Refund] as r ON r.TransactionId = t.Id
            WHERE r.Id = '{refundId}'
        ";

        var transactionBalanceState = await db.QuerySingleAsync<PersistableTransactionBalance>(transactionBalanceQuerySql);
        if (transactionBalanceState is null) return null;
        string refundsQuerySql = $@"
        SELECT Id, TransactionId, Amount, Status FROM [Refund] WHERE TransactionId = '{transactionBalanceState.Id}'
        ";

        var refundStates = await db.QueryAsync<PersistableRefund>(refundsQuerySql);

        var state = new TransactionBalanceState
        {
            TransactionId = transactionBalanceState.Id.ToString(),
            TransactionAmount = transactionBalanceState.Amount.ToString(),
            Refunds = refundStates.Select(rs => new RefundState
            {
                Id = rs.Id.ToString(),
                RefundAmount = rs.Amount,
                RefundStatus = rs.Status
            }).ToList()
        };

        return await Task.FromResult(TransactionBalance.Of(state));
    }

    public async Task<TransactionBalance> GetByTransactionId(Guid transactionId)
    {
        using IDbConnection db = new SqliteConnection(_connString);


        string transactionBalanceQuerySql = $@"
            SELECT Id, Amount FROM [TransactionBalance] as t
            WHERE t.Id = '{transactionId}'
        ";

        var transactionBalanceState = await db.QuerySingleOrDefaultAsync<PersistableTransactionBalance>(transactionBalanceQuerySql);
        if (transactionBalanceState is null) return null;
        if(transactionBalanceState is null) return null;
        string refundsQuerySql = $@"
        SELECT Id, TransactionId, Amount, Status FROM [Refund] WHERE TransactionId = '{transactionId}'
        ";

        var refundStates = await db.QueryAsync<PersistableRefund>(refundsQuerySql);

        var state = new TransactionBalanceState
        {
            TransactionId = transactionBalanceState.Id.ToString(),
            TransactionAmount = transactionBalanceState.Amount.ToString(),
            Refunds = refundStates.Select(rs => new RefundState
            {
                Id = rs.Id.ToString(),
                RefundAmount = rs.Amount,
                RefundStatus = rs.Status
            }).ToList()
        };

        return await Task.FromResult(TransactionBalance.Of(state));
    }

    public async Task<IEnumerable<Refund>> GetRejectedRefunds(Guid transactionId)
    {
        using IDbConnection db = new SqliteConnection(_connString);

        string rejectedRefundsQuerySql = $@"
            SELECT r.Id, r.Amount, r.TransactionId, r.Status FROM [Refund] as r
            WHERE r.TransactionId = '{transactionId}' AND r.Status = '{RefundStatus.Rejected}'
        ";

        var refundStates = await db.QueryAsync<PersistableRefund>(rejectedRefundsQuerySql);

        return refundStates.Select(rs => new RefundState
        {
            Id = rs.Id,
            RefundAmount = rs.Amount,
            RefundStatus = rs.Status
        })
        .Select(r => Refund.Of(r))
        .ToList();
    }
}