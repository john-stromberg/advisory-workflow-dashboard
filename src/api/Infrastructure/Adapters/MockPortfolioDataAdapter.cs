using ClientMeetingPrep.Api.Application.Contracts;

namespace ClientMeetingPrep.Api.Infrastructure.Adapters;

public sealed class MockPortfolioDataAdapter : IPortfolioDataAdapter
{
    public Task<string> BuildSummaryJsonAsync(Guid clientId, DateOnly meetingDate, CancellationToken cancellationToken)
    {
        var summary = "{" +
            "\"clientId\":\"" + clientId + "\"," +
            "\"meetingDate\":\"" + meetingDate + "\"," +
            "\"portfolioValue\":3250000," +
            "\"ytdPerformance\":0.082," +
            "\"cash\":210000" +
            "}";
        return Task.FromResult(summary);
    }

    public Task<string> BuildChangeLogJsonAsync(Guid clientId, DateOnly meetingDate, DateOnly? lastMeetingDate, CancellationToken cancellationToken)
    {
        var last = lastMeetingDate?.ToString() ?? "n/a";
        var changes = "[" +
            "{\"category\":\"Allocation\",\"description\":\"Equity allocation +2.1% since " + last + "\"}," +
            "{\"category\":\"Cash Flow\",\"description\":\"New quarterly distribution scheduled\"}" +
            "]";
        return Task.FromResult(changes);
    }
}
