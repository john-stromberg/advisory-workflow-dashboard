using ClientMeetingPrep.Api.Application.Contracts;
using ClientMeetingPrep.Api.Domain;
using ClientMeetingPrep.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientMeetingPrep.Api.Infrastructure.Services;

public sealed class MeetingPacketService(
    AppDbContext dbContext,
    IClientDataAdapter clientAdapter,
    IPortfolioDataAdapter portfolioAdapter,
    IAuditService auditService) : IMeetingPacketService
{
    public async Task<MeetingPacketSummaryDto> CreateAsync(CreateMeetingPacketRequest request, string actor, CancellationToken cancellationToken)
    {
        var client = await clientAdapter.GetClientAsync(request.ClientId, cancellationToken)
            ?? throw new InvalidOperationException("Client not found in adapter.");

        var existing = await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == client.Id, cancellationToken);
        if (existing is null)
        {
            dbContext.Clients.Add(client);
        }

        var meeting = new Meeting
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            MeetingDate = request.MeetingDate,
            LastMeetingDate = request.LastMeetingDate,
            Status = PacketStatus.Draft
        };

        var packet = new MeetingPacket
        {
            Id = Guid.NewGuid(),
            MeetingId = meeting.Id,
            SummaryJson = await portfolioAdapter.BuildSummaryJsonAsync(client.Id, request.MeetingDate, cancellationToken),
            ChangeLogJson = await portfolioAdapter.BuildChangeLogJsonAsync(client.Id, request.MeetingDate, request.LastMeetingDate, cancellationToken),
            Status = PacketStatus.Draft,
            Version = 1
        };

        dbContext.Meetings.Add(meeting);
        dbContext.MeetingPackets.Add(packet);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditService.WriteAsync("MeetingPacket", packet.Id, "Created", actor, "{\"status\":\"Draft\"}", cancellationToken);

        return new MeetingPacketSummaryDto(packet.Id, meeting.Id, client.Name, meeting.MeetingDate, packet.Status, packet.Version, packet.SummaryJson, packet.ChangeLogJson);
    }

    public async Task<MeetingPacketSummaryDto?> GetAsync(Guid packetId, CancellationToken cancellationToken)
    {
        var packet = await dbContext.MeetingPackets
            .Include(p => p.Meeting)
            .ThenInclude(m => m!.Client)
            .FirstOrDefaultAsync(p => p.Id == packetId, cancellationToken);

        if (packet?.Meeting?.Client is null)
        {
            return null;
        }

        return new MeetingPacketSummaryDto(packet.Id, packet.MeetingId, packet.Meeting.Client.Name, packet.Meeting.MeetingDate, packet.Status, packet.Version, packet.SummaryJson, packet.ChangeLogJson);
    }

    public async Task<MeetingPacketSummaryDto?> RefreshAsync(Guid packetId, string actor, CancellationToken cancellationToken)
    {
        var packet = await dbContext.MeetingPackets
            .Include(p => p.Meeting)
            .ThenInclude(m => m!.Client)
            .FirstOrDefaultAsync(p => p.Id == packetId, cancellationToken);

        if (packet?.Meeting?.Client is null)
        {
            return null;
        }

        packet.SummaryJson = await portfolioAdapter.BuildSummaryJsonAsync(packet.Meeting.Client.Id, packet.Meeting.MeetingDate, cancellationToken);
        packet.ChangeLogJson = await portfolioAdapter.BuildChangeLogJsonAsync(packet.Meeting.Client.Id, packet.Meeting.MeetingDate, packet.Meeting.LastMeetingDate, cancellationToken);
        packet.Version += 1;

        await dbContext.SaveChangesAsync(cancellationToken);
        await auditService.WriteAsync("MeetingPacket", packet.Id, "Refreshed", actor, "{\"version\":" + packet.Version + "}", cancellationToken);

        return new MeetingPacketSummaryDto(packet.Id, packet.MeetingId, packet.Meeting.Client.Name, packet.Meeting.MeetingDate, packet.Status, packet.Version, packet.SummaryJson, packet.ChangeLogJson);
    }

    public async Task<bool> SubmitForReviewAsync(Guid packetId, string actor, CancellationToken cancellationToken)
    {
        var packet = await dbContext.MeetingPackets.FirstOrDefaultAsync(p => p.Id == packetId, cancellationToken);
        if (packet is null)
        {
            return false;
        }

        packet.Status = PacketStatus.InReview;
        dbContext.Approvals.Add(new Approval
        {
            Id = Guid.NewGuid(),
            PacketId = packet.Id,
            SubmittedBy = actor,
            Decision = ApprovalDecision.Pending
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        await auditService.WriteAsync("MeetingPacket", packet.Id, "SubmittedForReview", actor, "{\"status\":\"InReview\"}", cancellationToken);
        return true;
    }

    public async Task<bool> ApproveAsync(Guid packetId, string actor, ApprovalRequest request, CancellationToken cancellationToken)
    {
        var packet = await dbContext.MeetingPackets.FirstOrDefaultAsync(p => p.Id == packetId, cancellationToken);
        if (packet is null)
        {
            return false;
        }

        var approval = await dbContext.Approvals
            .Where(a => a.PacketId == packetId)
            .OrderByDescending(a => a.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);

        if (approval is null)
        {
            return false;
        }

        approval.ReviewedBy = actor;
        approval.Decision = ApprovalDecision.Approved;
        approval.Notes = request.Notes;
        approval.Timestamp = DateTimeOffset.UtcNow;
        packet.Status = PacketStatus.Approved;

        await dbContext.SaveChangesAsync(cancellationToken);
        await auditService.WriteAsync("MeetingPacket", packet.Id, "Approved", actor, "{\"status\":\"Approved\"}", cancellationToken);
        return true;
    }
}
