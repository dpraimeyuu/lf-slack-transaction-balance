namespace Lf.Slack.TransactionBalance.Application;

public class CannotAddRefundToNonExistingTransactionBalance : Exception {
    public CannotAddRefundToNonExistingTransactionBalance() : base()
    {
    }

    public CannotAddRefundToNonExistingTransactionBalance(string? message) : base(message)
    {
    }

    public CannotAddRefundToNonExistingTransactionBalance(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class CannotApproveRefundOfNonExistingTransactionBalance : Exception
{
    public CannotApproveRefundOfNonExistingTransactionBalance() : base()
    {
    }

    public CannotApproveRefundOfNonExistingTransactionBalance(string? message) : base(message)
    {
    }

    public CannotApproveRefundOfNonExistingTransactionBalance(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class CannotRejectRefundOfNonExistingTransactionBalance : Exception
{
    public CannotRejectRefundOfNonExistingTransactionBalance()
    {
    }

    public CannotRejectRefundOfNonExistingTransactionBalance(string? message) : base(message)
    {
    }

    public CannotRejectRefundOfNonExistingTransactionBalance(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}