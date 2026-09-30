using ClientMeetingPrep.Api.Application.Contracts;
using ClientMeetingPrep.Api.Domain;
using ClientMeetingPrep.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientMeetingPrep.Api.Infrastructure.Services;

public sealed class AuditService(AppDbContext dbContext) : IAuditService
{
    public async Task<IReadOnlyList<AuditEventDto>> GetByPacketIdAsync(Guid packetId, CancellationToken cancellationToken)
    {
        return await dbContext.AuditEvents
            .Where(x => x.EntityId == packetId || x.MetadataJson.Contains(packetId.ToString()))
            .OrderByDescending(x => x.Timestamp)
            .Select(x => new AuditEventDto(x.Id, x.EntityType, x.EntityId, x.Action, x.Actor, x.Timestamp, x.MetadataJson))
            .ToListAsync(cancellationToken);
    }

    public async Task WriteAsync(string entityType, Guid entityId, string action, string actor, string metadataJson, CancellationToken cancellationToken)
    {
        dbContext.AuditEvents.Add(new AuditEvent
        {
            Id = Guid.NewGuid(),
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            Actor = actor,
            MetadataJson = metadataJson,
            Timestamp = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
