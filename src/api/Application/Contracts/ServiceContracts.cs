using ClientMeetingPrep.Api.Domain;

namespace ClientMeetingPrep.Api.Application.Contracts;

public interface IMeetingPacketService
{
    Task<MeetingPacketSummaryDto> CreateAsync(CreateMeetingPacketRequest request, string actor, CancellationToken cancellationToken);
    Task<MeetingPacketSummaryDto?> GetAsync(Guid packetId, CancellationToken cancellationToken);
    Task<MeetingPacketSummaryDto?> RefreshAsync(Guid packetId, string actor, CancellationToken cancellationToken);
    Task<bool> SubmitForReviewAsync(Guid packetId, string actor, CancellationToken cancellationToken);
    Task<bool> ApproveAsync(Guid packetId, string actor, ApprovalRequest request, CancellationToken cancellationToken);
}

public interface IExportService
{
    Task<PacketArtifact> ExportExcelAsync(Guid packetId, string actor, CancellationToken cancellationToken);
    Task<PacketArtifact> ExportPowerPointAsync(Guid packetId, string actor, CancellationToken cancellationToken);
    Task<OutlookDraftResponse?> CreateOutlookDraftAsync(Guid packetId, string actor, CancellationToken cancellationToken);
}

public interface IAuditService
{
    Task<IReadOnlyList<AuditEventDto>> GetByPacketIdAsync(Guid packetId, CancellationToken cancellationToken);
    Task WriteAsync(string entityType, Guid entityId, string action, string actor, string metadataJson, CancellationToken cancellationToken);
}

public interface IClientDataAdapter
{
    Task<Client?> GetClientAsync(Guid clientId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Client>> GetSampleClientsAsync(CancellationToken cancellationToken);
}

public interface IPortfolioDataAdapter
{
    Task<string> BuildSummaryJsonAsync(Guid clientId, DateOnly meetingDate, CancellationToken cancellationToken);
    Task<string> BuildChangeLogJsonAsync(Guid clientId, DateOnly meetingDate, DateOnly? lastMeetingDate, CancellationToken cancellationToken);
}

