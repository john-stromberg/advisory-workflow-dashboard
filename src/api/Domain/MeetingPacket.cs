namespace ClientMeetingPrep.Api.Domain;

public sealed class MeetingPacket
{
    public Guid Id { get; set; }
    public Guid MeetingId { get; set; }
    public string SummaryJson { get; set; } = "{}";
    public string ChangeLogJson { get; set; } = "[]";
    public PacketStatus Status { get; set; } = PacketStatus.Draft;
    public int Version { get; set; } = 1;

    public Meeting? Meeting { get; set; }
    public ICollection<PacketArtifact> Artifacts { get; set; } = new List<PacketArtifact>();
    public ICollection<Approval> Approvals { get; set; } = new List<Approval>();
}

