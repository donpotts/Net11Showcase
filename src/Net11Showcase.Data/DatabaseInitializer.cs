using System;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Net11Showcase.Shared.Models;

namespace Net11Showcase.Data;

public static class DatabaseInitializer
{
    /// <summary>
    /// Creates the schema and fills the JSON <see cref="CustomerEntity.Profile"/> column from the legacy
    /// <see cref="CustomerEntity.MetadataJson"/> string. (EF Core cannot seed complex properties through HasData,
    /// see dotnet/efcore#31254, so the profile is populated here.) Idempotent.
    /// </summary>
    public static void Initialize(AppDbContext db)
    {
        db.Database.EnsureCreated();

        var missing = db.Customers.ToList().Where(c => c.Profile is null).ToList();
        if (missing.Count == 0) return;

        foreach (var customer in missing)
            customer.Profile = ProfileFromMetadata(customer.MetadataJson);

        db.SaveChanges();
    }

    internal static CustomerProfile ProfileFromMetadata(string metadataJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(metadataJson) ? "{}" : metadataJson);
            var root = doc.RootElement;
            return new CustomerProfile
            {
                Industry = root.TryGetProperty("industry", out var i) ? i.GetString() ?? "" : "",
                Region = root.TryGetProperty("region", out var r) ? r.GetString() ?? "" : "",
                Seats = root.TryGetProperty("seats", out var st) && st.TryGetInt32(out var n) ? n : null
            };
        }
        catch (JsonException)
        {
            return new CustomerProfile();
        }
    }
}
