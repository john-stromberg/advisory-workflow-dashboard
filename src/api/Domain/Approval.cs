namespace ClientMeetingPrep.Api.Domain;

public sealed class Approval
{
    public Guid Id { get; set; }
    public Guid PacketId { get; set; }
    public string SubmittedBy { get; set; } = string.Empty;
    public string? ReviewedBy { get; set; }
    public ApprovalDecision Decision { get; set; } = ApprovalDecision.Pending;
    public string? Notes { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    public MeetingPacket? Packet { get; set; }
}

