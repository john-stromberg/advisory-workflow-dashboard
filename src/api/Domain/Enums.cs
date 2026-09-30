namespace ClientMeetingPrep.Api.Domain;

public enum PacketStatus
{
    Draft = 0,
    InReview = 1,
    Approved = 2,
    Released = 3,
    Rejected = 4
}

public enum ArtifactType
{
    Excel = 0,
    PowerPoint = 1
}

public enum ApprovalDecision
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

