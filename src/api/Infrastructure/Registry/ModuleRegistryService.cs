using System.Text.Json;
using ClientMeetingPrep.Api.Application.Contracts;

namespace ClientMeetingPrep.Api.Infrastructure.Registry;

public sealed class ModuleRegistryService(IWebHostEnvironment environment) : IModuleRegistryService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ModuleRegistryResponse> GetRegistryAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(environment.ContentRootPath, "module-registry", "modules.json");
        if (!File.Exists(path))
        {
            return new ModuleRegistryResponse(DateTimeOffset.UtcNow, []);
        }

        await using var stream = File.OpenRead(path);
        var items = await JsonSerializer.DeserializeAsync<List<ModuleRegistryFileItem>>(stream, JsonOptions, cancellationToken)
            ?? [];

        var modules = items
            .OrderBy(x => x.SortOrder)
            .Select(x => new ModuleDescriptorDto(
                x.Id,
                x.Name,
                x.Category,
                x.Status,
                x.IntegrationType,
                x.Route,
                x.RepositoryUrl,
                x.Description,
                x.SortOrder,
                x.IsEnabled))
            .ToList();

        return new ModuleRegistryResponse(DateTimeOffset.UtcNow, modules);
    }

    private sealed class ModuleRegistryFileItem
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string IntegrationType { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;
        public string RepositoryUrl { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int SortOrder { get; init; }
        public bool IsEnabled { get; init; } = true;
    }
}
