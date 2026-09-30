using ClientMeetingPrep.Api.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClientMeetingPrep.Api.Controllers;

[ApiController]
[Route("api/module-registry")]
public sealed class ModuleRegistryController(IModuleRegistryService moduleRegistryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ModuleRegistryResponse>> Get(CancellationToken cancellationToken)
    {
        var registry = await moduleRegistryService.GetRegistryAsync(cancellationToken);
        return Ok(registry);
    }
}
