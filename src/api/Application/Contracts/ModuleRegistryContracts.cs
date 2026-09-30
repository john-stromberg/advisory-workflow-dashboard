namespace ClientMeetingPrep.Api.Application.Contracts;

public sealed record ModuleRegistryResponse(DateTimeOffset GeneratedAt, IReadOnlyList<ModuleDescriptorDto> Modules);

public sealed record ModuleDescriptorDto(
    string Id,
    string Name,
    string Category,
    string Status,
    string IntegrationType,
    string Route,
    string RepositoryUrl,
    string Description,
    int SortOrder,
    bool IsEnabled);

public interface IModuleRegistryService
{
    Task<ModuleRegistryResponse> GetRegistryAsync(CancellationToken cancellationToken);
}
