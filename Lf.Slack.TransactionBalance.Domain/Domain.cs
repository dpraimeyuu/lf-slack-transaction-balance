namespace Lf.Slack.TransactionBalance.Domain;
using System.Collections.Generic;
using System;
using System.Linq;

public interface ITransactionBalanceRepository
{
    public Task Save(TransactionBalance transactionBalance);
    public Task<TransactionBalance> GetByTransactionId(Guid transactionId);
    public Task<TransactionBalance> GetByRefundId(Guid refundId);

    public Task<IEnumerable<Refund>> GetRejectedRefunds(Guid transactionId);
}

public enum RefundStatus
{
    Created,
    Approved,
    Rejected
}

public class Refund
{
    public Guid Id { get; private set; }
    public RefundStatus Status { get; private set; }
    public decimal RefundAmount { get; private set; }
    private Refund(decimal refundAmount)
    {
        RefundAmount = refundAmount;
        Status = RefundStatus.Created;
        Id = Guid.NewGuid();
    }

    private Refund() { }
    public static Refund Of(IRefundState state)
    {
        return new Refund()
        {
            Id = Guid.Parse(state.Id),
            RefundAmount = decimal.Parse(state.RefundAmount),
            Status = (RefundStatus)Enum.Parse(typeof(RefundStatus), state.RefundStatus)
        };
    }

    public static Refund Of(decimal refundAmount) => new(refundAmount);

    public void Approve()
    {
        if (Status == RefundStatus.Approved) throw new RefundAlreadyApprovedException("Cannot approve already approved refund");
        if (Status == RefundStatus.Rejected) throw new CannotApproveRejectedRefundException("Cannot approve rejected refund");

        Status = RefundStatus.Approved;
    }

    public void Reject()
    {
        if (Status == RefundStatus.Rejected) throw new RefundAlreadyRejectedException("Cannot reject already rejected refund");
        if (Status == RefundStatus.Approved) throw new CannotApproveRejectedRefundException("Cannot reject approved refund");

        Status = RefundStatus.Rejected;
    }

    public bool IsApproved => Status == RefundStatus.Approved;
    public bool IsRejected => Status == RefundStatus.Rejected;
    public decimal ToDecimal() => RefundAmount;
}

public class TransactionBalanceDetails
{
    public decimal TransactionAmount { get; }
    public Guid TransactionId { get; }

    public TransactionBalanceDetails(Guid transactionId, decimal transactionAmount)
    {
        TransactionAmount = transactionAmount;
        TransactionId = transactionId;
    }
}

public interface IDomainEvent { }
public record AmountRefundingRequested(Guid RefundId) : IDomainEvent { };

public interface ITransactionBalanceState
{
    public string TransactionId { get; set; }
    public string TransactionAmount { get; set; }
    public IReadOnlyList<IRefundState> Refunds { get; set; }
}

public class TransactionBalance
{
    public Guid Id => TransactionBalanceDetails.TransactionId;
    public TransactionBalanceDetails TransactionBalanceDetails { get; private set; }
    private List<Refund> _refunds = new();
    public IReadOnlyList<Refund> Refunds => _refunds;
    private IEnumerable<Refund> _sumableRefunds => _refunds.Where(refund => !refund.IsRejected);

    private TransactionBalance(TransactionBalanceDetails details)
    {
        TransactionBalanceDetails = details;
    }

    public static TransactionBalance Of(TransactionBalanceDetails details) => new(details);

    private TransactionBalance() { }

    public static TransactionBalance Of(ITransactionBalanceState state) => new()
    {
        TransactionBalanceDetails = new TransactionBalanceDetails(Guid.Parse(state.TransactionId), decimal.Parse(state.TransactionAmount)),
        _refunds = state.Refunds.Select(refund => Refund.Of(refund)).ToList()
    };

    private bool BalanceIsExceededByExtra(decimal amount) => TransactionBalanceDetails.TransactionAmount < _sumableRefunds.Sum(refund => refund.ToDecimal()) + amount;
    public IReadOnlyList<IDomainEvent> RequestRefundOf(decimal amount)
    {

        if (BalanceIsExceededByExtra(amount))
        {
            throw new TransactionAmountExceededException($"Cannot add refund with amount {amount}");
        }

        var refund = Refund.Of(amount);
        _refunds.Add(refund);

        return new List<IDomainEvent>() { new AmountRefundingRequested(refund.Id) };
    }

    public void AcceptRefund(Guid refundId)
    {
        var refund = _refunds.First(refund => refund.Id == refundId);
        if (refund is null)
        {
            throw new AcceptingNotFoundRefundException($"Cannot accept refund with ID {refundId}, because it doesn't exist");
        }

        refund.Approve();
    }

    public void RejectRefund(Guid refundId)
    {
        var refund = _refunds.First(refund => refund.Id == refundId);
        if (refund is null)
        {
            throw new RejectingNotFoundRefundException($"Cannot reject refund with ID {refundId}, because it doesn't exist");
        }

        refund.Reject();
    }
}
