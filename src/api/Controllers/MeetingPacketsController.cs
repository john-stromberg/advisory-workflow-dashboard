using ClientMeetingPrep.Api.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClientMeetingPrep.Api.Controllers;

[ApiController]
[Route("api/meeting-packets")]
public sealed class MeetingPacketsController(IMeetingPacketService meetingPacketService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MeetingPacketSummaryDto>> Create([FromBody] CreateMeetingPacketRequest request, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var packet = await meetingPacketService.CreateAsync(request, actor, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = packet.PacketId }, packet);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MeetingPacketSummaryDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var packet = await meetingPacketService.GetAsync(id, cancellationToken);
        return packet is null ? NotFound() : Ok(packet);
    }

    [HttpPost("{id:guid}/refresh")]
    public async Task<ActionResult<MeetingPacketSummaryDto>> Refresh(Guid id, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var packet = await meetingPacketService.RefreshAsync(id, actor, cancellationToken);
        return packet is null ? NotFound() : Ok(packet);
    }

    [HttpPost("{id:guid}/submit-review")]
    public async Task<IActionResult> SubmitReview(Guid id, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var success = await meetingPacketService.SubmitForReviewAsync(id, actor, cancellationToken);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ApprovalRequest request, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var success = await meetingPacketService.ApproveAsync(id, actor, request, cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
