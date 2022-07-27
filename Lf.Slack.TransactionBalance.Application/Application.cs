using Lf.Slack.TransactionBalance.Domain;

namespace Lf.Slack.TransactionBalance.Application;
public class TransactionBalanceService
{
    private readonly ITransactionBalanceRepository _transactionBalanceRepository;
    public TransactionBalanceService(ITransactionBalanceRepository transactionBalanceRepository)
    {
        _transactionBalanceRepository = transactionBalanceRepository;
    }

    public async Task Start(Guid transactionId, decimal transactionAmount)
    {
        var transactionBalance = TransactionBalance.Domain.TransactionBalance.Of(new TransactionBalanceDetails(transactionId, transactionAmount));

        await _transactionBalanceRepository.Save(transactionBalance);
    }

    public async Task RequestRefund(string transactionId, decimal refundAmount)
    {
        var transactionGuidId = Guid.Parse(transactionId);
        var transactionBalance = await _transactionBalanceRepository.GetByTransactionId(transactionGuidId);
        if (transactionBalance is null) throw new CannotAddRefundToNonExistingTransactionBalance($"No transaction balance with id {transactionId}");

        transactionBalance.RequestRefundOf(refundAmount);

        await _transactionBalanceRepository.Save(transactionBalance);
    }

    public async Task ApproveRefund(string refundId)
    {
        var _refundId = Guid.Parse(refundId);
        var transactionBalance = await _transactionBalanceRepository.GetByRefundId(_refundId);
        if (transactionBalance is null) throw new CannotApproveRefundOfNonExistingTransactionBalance($"No transaction balance related to refund with id {refundId}");

        transactionBalance.AcceptRefund(_refundId);

        await _transactionBalanceRepository.Save(transactionBalance);
    }

    public async Task RejectRefund(string refundId)
    {
        var _refundId = Guid.Parse(refundId);
        var transactionBalance = await _transactionBalanceRepository.GetByRefundId(_refundId);
        if (transactionBalance is null) throw new CannotRejectRefundOfNonExistingTransactionBalance($"No transaction balance related to refund with id {refundId}");

        transactionBalance.RejectRefund(_refundId);

        await _transactionBalanceRepository.Save(transactionBalance);
}

    public Task<IEnumerable<Refund>> GetRejectedRefunds(string transactionId) =>
        _transactionBalanceRepository.GetRejectedRefunds(Guid.Parse(transactionId));

    public Task<Domain.TransactionBalance> GetTransactionBalance(string transactionId) =>
        _transactionBalanceRepository.GetByTransactionId(Guid.Parse(transactionId));
}
