namespace Lf.Slack.TransactionBalance.Domain;

public class RefundAlreadyApprovedException : Exception
{
    public RefundAlreadyApprovedException() : base()
    {
    }

    protected RefundAlreadyApprovedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    public RefundAlreadyApprovedException(string? message) : base(message)
    {
    }

    public RefundAlreadyApprovedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
public class RefundAlreadyRejectedException : Exception
{
    public RefundAlreadyRejectedException() : base()
    {
    }

    protected RefundAlreadyRejectedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    public RefundAlreadyRejectedException(string? message) : base(message)
    {
    }

    public RefundAlreadyRejectedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class CannotApproveRejectedRefundException : Exception
{
    public CannotApproveRejectedRefundException() : base()
    {
    }

    protected CannotApproveRejectedRefundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    public CannotApproveRejectedRefundException(string? message) : base(message)
    {
    }

    public CannotApproveRejectedRefundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class TransactionAmountExceededException : Exception
{
    public TransactionAmountExceededException() : base()
    {
    }

    public TransactionAmountExceededException(string? message) : base(message)
    {
    }

    public TransactionAmountExceededException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class RejectingNotFoundRefundException : Exception
{
    public RejectingNotFoundRefundException() : base()
    {
    }

    public RejectingNotFoundRefundException(string? message) : base(message)
    {
    }

    public RejectingNotFoundRefundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class AcceptingNotFoundRefundException : Exception
{
    public AcceptingNotFoundRefundException() : base()
    {
    }

    public AcceptingNotFoundRefundException(string? message) : base(message)
    {
    }

    public AcceptingNotFoundRefundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
public interface IRefundState
{
    public string Id { get; set; }
    public string RefundStatus { get; set; }
    public string RefundAmount { get; set; }
}