using FeedbackApi.Models;
using Microsoft.EntityFrameworkCore;
using FeedbackApi.Data.Seed;

namespace FeedbackApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<MessageTopic> MessageTopics => Set<MessageTopic>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("contacts");
            entity.HasKey(e => e.id);
            entity.Property(e => e.name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.phone).IsRequired().HasMaxLength(32);
            entity.HasIndex(e => new { e.email, e.phone }).IsUnique();
        });

        modelBuilder.Entity<MessageTopic>(entity =>
        {
            entity.ToTable("message_topics");
            entity.HasKey(e => e.id);
            entity.Property(e => e.name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("messages");
            entity.HasKey(e => e.id);
            entity.Property(e => e.text).IsRequired();
            entity.Property(e => e.created_at).IsRequired();

            entity.HasOne(e => e.contact)
                .WithMany()
                .HasForeignKey(e => e.contact_id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.topic)
                .WithMany()
                .HasForeignKey(e => e.topic_id)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.SeedInitial();
    }
}


