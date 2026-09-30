using ClientMeetingPrep.Api.Domain;

namespace ClientMeetingPrep.Api.Application.Contracts;

public sealed record CreateMeetingPacketRequest(Guid ClientId, DateOnly MeetingDate, DateOnly? LastMeetingDate);

public sealed record MeetingPacketSummaryDto(
    Guid PacketId,
    Guid MeetingId,
    string ClientName,
    DateOnly MeetingDate,
    PacketStatus Status,
    int Version,
    string SummaryJson,
    string ChangeLogJson);

public sealed record ApprovalRequest(string Notes);

public sealed record OutlookDraftResponse(string DraftId, string Subject);

public sealed record AuditEventDto(
    Guid Id,
    string EntityType,
    Guid EntityId,
    string Action,
    string Actor,
    DateTimeOffset Timestamp,
    string MetadataJson);

