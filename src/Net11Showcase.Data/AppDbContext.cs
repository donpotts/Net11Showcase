using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Net11Showcase.Shared.Models;

namespace Net11Showcase.Data;

public class AppDbContext : DbContext
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Tier).HasMaxLength(50);
            entity.Property(e => e.MetadataJson).HasDefaultValue("{}");
            
            // Serialize float[] vector embeddings to comma-separated string for universal SQLite/SQL compatibility
            entity.Property(e => e.EmbeddingVector)
                  .HasConversion(
                      v => string.Join(',', v),
                      v => Array.ConvertAll(v.Split(',', StringSplitOptions.RemoveEmptyEntries), float.Parse)
                  );
        });

        // Seed initial data
        modelBuilder.Entity<CustomerEntity>().HasData(
            new CustomerEntity
            {
                Id = 1,
                FullName = "Sarah Jenkins",
                Email = "sarah.jenkins@apexcloud.io",
                Tier = "Enterprise Tier 1",
                MetadataJson = "{\"industry\":\"FinTech\",\"seats\":450,\"region\":\"US-East\"}",
                EmbeddingVector = new float[] { 0.12f, 0.85f, 0.42f, 0.91f },
                CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new CustomerEntity
            {
                Id = 2,
                FullName = "Marcus Vance",
                Email = "m.vance@quantumsec.org",
                Tier = "Government Enclave",
                MetadataJson = "{\"industry\":\"Cybersecurity\",\"clearance\":\"Level4\",\"region\":\"US-Gov\"}",
                EmbeddingVector = new float[] { 0.45f, 0.72f, 0.89f, 0.33f },
                CreatedAt = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc)
            },
            new CustomerEntity
            {
                Id = 3,
                FullName = "Elena Rostova",
                Email = "elena@nordicscale.se",
                Tier = "Strategic Growth",
                MetadataJson = "{\"industry\":\"BioTech\",\"clusters\":12,\"region\":\"EU-North\"}",
                EmbeddingVector = new float[] { 0.78f, 0.31f, 0.65f, 0.88f },
                CreatedAt = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
