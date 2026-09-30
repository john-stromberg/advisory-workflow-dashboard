using ClientMeetingPrep.Api.Application.Contracts;
using ClientMeetingPrep.Api.Domain;

namespace ClientMeetingPrep.Api.Infrastructure.Adapters;

public sealed class MockClientDataAdapter : IClientDataAdapter
{
    private static readonly IReadOnlyList<Client> SampleClients =
    [
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ExternalCrmId = "CRM-0001",
            Name = "Anderson Family Office",
            Segment = "Ultra High Net Worth",
            AdvisorId = "advisor-001"
        },
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ExternalCrmId = "CRM-0002",
            Name = "Bennett Household",
            Segment = "Private Wealth",
            AdvisorId = "advisor-002"
        },
        new()
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            ExternalCrmId = "CRM-0003",
            Name = "Carter Retirement Trust",
            Segment = "Retirement Income",
            AdvisorId = "advisor-003"
        }
    ];

    public Task<Client?> GetClientAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var client = SampleClients.FirstOrDefault(c => c.Id == clientId);
        if (client is not null)
        {
            return Task.FromResult<Client?>(new Client
            {
                Id = client.Id,
                ExternalCrmId = client.ExternalCrmId,
                Name = client.Name,
                Segment = client.Segment,
                AdvisorId = client.AdvisorId
            });
        }

        return Task.FromResult<Client?>(new Client
        {
            Id = clientId,
            ExternalCrmId = "CRM-" + clientId.ToString("N"),
            Name = "Sample Household",
            Segment = "Private Wealth",
            AdvisorId = "advisor-001"
        });
    }

    public Task<IReadOnlyList<Client>> GetSampleClientsAsync(CancellationToken cancellationToken)
    {
        var clients = SampleClients
            .Select(client => new Client
            {
                Id = client.Id,
                ExternalCrmId = client.ExternalCrmId,
                Name = client.Name,
                Segment = client.Segment,
                AdvisorId = client.AdvisorId
            })
            .ToList();

        return Task.FromResult<IReadOnlyList<Client>>(clients);
    }
}
