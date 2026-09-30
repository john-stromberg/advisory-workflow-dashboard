using ClientMeetingPrep.Api.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClientMeetingPrep.Api.Controllers;

[ApiController]
[Route("api/demo")]
public sealed class DemoController(IClientDataAdapter clientDataAdapter, IMeetingPacketService meetingPacketService) : ControllerBase
{
    [HttpGet("clients")]
    public async Task<IActionResult> GetSampleClients(CancellationToken cancellationToken)
    {
        var clients = await clientDataAdapter.GetSampleClientsAsync(cancellationToken);
        var response = clients.Select(c => new
        {
            c.Id,
            c.ExternalCrmId,
            c.Name,
            c.Segment,
            c.AdvisorId
        });

        return Ok(response);
    }

    [HttpPost("quickstart")]
    public async Task<IActionResult> Quickstart(CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        var clients = await clientDataAdapter.GetSampleClientsAsync(cancellationToken);
        var client = clients.First();

        var meetingDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(7));
        var lastMeetingDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(-3));
        var packet = await meetingPacketService.CreateAsync(
            new CreateMeetingPacketRequest(client.Id, meetingDate, lastMeetingDate),
            actor,
            cancellationToken);

        await meetingPacketService.SubmitForReviewAsync(packet.PacketId, actor, cancellationToken);
        await meetingPacketService.ApproveAsync(packet.PacketId, actor, new ApprovalRequest("Approved for sample run"), cancellationToken);

        return Ok(new
        {
            packet.PacketId,
            packet.MeetingId,
            client.Name,
            packet.Status,
            message = "Sample packet created and approved. You can now call export endpoints."
        });
    }
}
