using Lf.Slack.TransactionBalance.Application;
using Microsoft.AspNetCore.Mvc;

namespace Lf.Slack.TransactionBalance.Api.Controllers;

public class StartTransactionBalanceDto {
    public Guid TransactionId { get; set; }
    public decimal TransactionAmount { get; set; }
}

[ApiController]
[Route("/api/transaction-balance")]
public class TransactionBalanceController : ControllerBase
{
    private readonly TransactionBalanceService _transactionBalanceService;

    public TransactionBalanceController(TransactionBalanceService transactionBalanceService)
    {
        _transactionBalanceService = transactionBalanceService;
    }

    [HttpPost]
    public async Task<IActionResult> StartTransactionBalance(StartTransactionBalanceDto dto)
    {
        await _transactionBalanceService.Start(dto.TransactionId, dto.TransactionAmount);

        return Ok();
    }

    [HttpPut("refund/{transactionId:guid}/with/{amount:decimal}")]
    public async Task<IActionResult> RequestARefund(Guid transactionId, decimal amount)
    {
        await _transactionBalanceService.RequestRefund(transactionId.ToString(), amount);

        return Ok();
    }

    [HttpPut("refund/{refundId:guid}/approve")]
    public async Task<IActionResult> ApproveRefund(Guid refundId)
    {
        await _transactionBalanceService.ApproveRefund(refundId.ToString());

        return Ok();
    }

    [HttpPut("refund/{refundId:guid}/reject")]
    public async Task<IActionResult> RejectRefund(Guid refundId)
    {
        await _transactionBalanceService.RejectRefund(refundId.ToString());

        return Ok();
    }

    [HttpGet("{transactionId:guid}/rejected-refunds")]
    public async Task<IActionResult> GetRejectedRefunds(Guid transactionId)
    {
        var refunds = await _transactionBalanceService.GetRejectedRefunds(transactionId.ToString());

        return Ok(
            refunds.Select(r => new
            {
                r.Id,
                r.RefundAmount,
                Status = r.Status.ToString()
            })
        );
    }

    [HttpGet("{transactionId:guid}")]
    public async Task<IActionResult> GetTransactionbalance(Guid transactionId)
    {
        var transactionBalance = await _transactionBalanceService.GetTransactionBalance(transactionId.ToString());

        return Ok(new {
            Id = transactionBalance.Id,
            Amount = transactionBalance.TransactionBalanceDetails.TransactionAmount,
            Refunds = transactionBalance.Refunds.Select(r => new {
                r.Id,
                r.RefundAmount,
                Status = r.Status.ToString()
            })
        });
    }
}
