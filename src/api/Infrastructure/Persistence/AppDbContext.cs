using ClientMeetingPrep.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClientMeetingPrep.Api.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<MeetingPacket> MeetingPackets => Set<MeetingPacket>();
    public DbSet<PacketArtifact> PacketArtifacts => Set<PacketArtifact>();
    public DbSet<Approval> Approvals => Set<Approval>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.Client)
            .WithMany(c => c.Meetings)
            .HasForeignKey(m => m.ClientId);

        modelBuilder.Entity<MeetingPacket>()
            .HasOne(p => p.Meeting)
            .WithOne(m => m.Packet)
            .HasForeignKey<MeetingPacket>(p => p.MeetingId);

        modelBuilder.Entity<PacketArtifact>()
            .HasOne(a => a.Packet)
            .WithMany(p => p.Artifacts)
            .HasForeignKey(a => a.PacketId);

        modelBuilder.Entity<Approval>()
            .HasOne(a => a.Packet)
            .WithMany(p => p.Approvals)
            .HasForeignKey(a => a.PacketId);
    }
}

