using ClientMeetingPrep.Api.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClientMeetingPrep.Api.Controllers;

[ApiController]
[Route("api/meeting-packets/{id:guid}/export")]
public sealed class ExportsController(IExportService exportService) : ControllerBase
{
    [HttpPost("excel")]
    public async Task<IActionResult> ExportExcel(Guid id, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        try
        {
            var artifact = await exportService.ExportExcelAsync(id, actor, cancellationToken);
            return Ok(new
            {
                artifact.Id,
                artifact.PacketId,
                artifact.Type,
                artifact.StorageUri,
                artifact.Hash,
                artifact.CreatedAt
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("powerpoint")]
    public async Task<IActionResult> ExportPowerPoint(Guid id, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        try
        {
            var artifact = await exportService.ExportPowerPointAsync(id, actor, cancellationToken);
            return Ok(new
            {
                artifact.Id,
                artifact.PacketId,
                artifact.Type,
                artifact.StorageUri,
                artifact.Hash,
                artifact.CreatedAt
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("outlook-draft")]
    public async Task<IActionResult> CreateOutlookDraft(Guid id, CancellationToken cancellationToken)
    {
        var actor = User?.Identity?.Name ?? "system";
        try
        {
            var draft = await exportService.CreateOutlookDraftAsync(id, actor, cancellationToken);
            return draft is null ? NotFound() : Ok(draft);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
