namespace ClientMeetingPrep.Api.Domain;

public sealed class PacketArtifact
{
    public Guid Id { get; set; }
    public Guid PacketId { get; set; }
    public ArtifactType Type { get; set; }
    public string StorageUri { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public MeetingPacket? Packet { get; set; }
}

