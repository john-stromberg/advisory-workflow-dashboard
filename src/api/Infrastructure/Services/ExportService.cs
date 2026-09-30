using ClientMeetingPrep.Api.Application.Contracts;
using ClientMeetingPrep.Api.Domain;
using ClientMeetingPrep.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientMeetingPrep.Api.Infrastructure.Services;

public sealed class ExportService(AppDbContext dbContext, IAuditService auditService) : IExportService
{
    public async Task<PacketArtifact> ExportExcelAsync(Guid packetId, string actor, CancellationToken cancellationToken)
    {
        var packet = await EnsurePacketApproved(packetId, cancellationToken);
        var artifact = BuildArtifact(packet.Id, ArtifactType.Excel, "/exports/meeting-packet.xlsx");
        dbContext.PacketArtifacts.Add(artifact);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditService.WriteAsync("MeetingPacket", packet.Id, "ExportedExcel", actor, "{\"artifactType\":\"Excel\"}", cancellationToken);
        return artifact;
    }

    public async Task<PacketArtifact> ExportPowerPointAsync(Guid packetId, string actor, CancellationToken cancellationToken)
    {
        var packet = await EnsurePacketApproved(packetId, cancellationToken);
        var artifact = BuildArtifact(packet.Id, ArtifactType.PowerPoint, "/exports/client-review-deck.pptx");
        dbContext.PacketArtifacts.Add(artifact);
        await dbContext.SaveChangesAsync(cancellationToken);
        await auditService.WriteAsync("MeetingPacket", packet.Id, "ExportedPowerPoint", actor, "{\"artifactType\":\"PowerPoint\"}", cancellationToken);
        return artifact;
    }

    public async Task<OutlookDraftResponse?> CreateOutlookDraftAsync(Guid packetId, string actor, CancellationToken cancellationToken)
    {
        var packet = await EnsurePacketApproved(packetId, cancellationToken);
        var draft = new OutlookDraftResponse(Guid.NewGuid().ToString("N"), "Meeting follow-up packet ready");
        await auditService.WriteAsync("MeetingPacket", packet.Id, "OutlookDraftCreated", actor, "{\"channel\":\"Outlook\"}", cancellationToken);
        return draft;
    }

    private async Task<MeetingPacket> EnsurePacketApproved(Guid packetId, CancellationToken cancellationToken)
    {
        var packet = await dbContext.MeetingPackets.FirstOrDefaultAsync(p => p.Id == packetId, cancellationToken)
            ?? throw new KeyNotFoundException("Meeting packet not found.");

        if (packet.Status != PacketStatus.Approved && packet.Status != PacketStatus.Released)
        {
            throw new InvalidOperationException("Packet must be approved before export or Outlook draft generation.");
        }

        return packet;
    }

    private static PacketArtifact BuildArtifact(Guid packetId, ArtifactType type, string storageUri)
    {
        return new PacketArtifact
        {
            Id = Guid.NewGuid(),
            PacketId = packetId,
            Type = type,
            StorageUri = storageUri,
            Hash = Guid.NewGuid().ToString("N"),
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
