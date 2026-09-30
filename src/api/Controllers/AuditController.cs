using ClientMeetingPrep.Api.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClientMeetingPrep.Api.Controllers;

[ApiController]
[Route("api/audit-events")]
public sealed class AuditController(IAuditService auditService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditEventDto>>> GetByPacket([FromQuery] Guid packetId, CancellationToken cancellationToken)
    {
        if (packetId == Guid.Empty)
        {
            return BadRequest(new { message = "packetId is required." });
        }

        var events = await auditService.GetByPacketIdAsync(packetId, cancellationToken);
        return Ok(events);
    }
}
