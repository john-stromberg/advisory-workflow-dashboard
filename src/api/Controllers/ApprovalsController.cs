using ClientMeetingPrep.Api.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClientMeetingPrep.Api.Controllers;

[ApiController]
[Route("api/meeting-packets/{id:guid}/approvals")]
public sealed class ApprovalsController(IMeetingPacketService meetingPacketService) : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> Submit(Guid id, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var success = await meetingPacketService.SubmitForReviewAsync(id, actor, cancellationToken);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ApprovalRequest request, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var success = await meetingPacketService.ApproveAsync(id, actor, request, cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
