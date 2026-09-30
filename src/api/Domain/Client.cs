namespace ClientMeetingPrep.Api.Domain;

public sealed class Client
{
    public Guid Id { get; set; }
    public string ExternalCrmId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Segment { get; set; } = string.Empty;
    public string AdvisorId { get; set; } = string.Empty;

    public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
}

