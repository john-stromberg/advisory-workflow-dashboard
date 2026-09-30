namespace ClientMeetingPrep.Api.Domain;

public sealed class Meeting
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateOnly MeetingDate { get; set; }
    public DateOnly? LastMeetingDate { get; set; }
    public PacketStatus Status { get; set; } = PacketStatus.Draft;

    public Client? Client { get; set; }
    public MeetingPacket? Packet { get; set; }
}

