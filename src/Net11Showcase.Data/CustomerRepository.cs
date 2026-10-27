using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Net11Showcase.Shared.Models;

namespace Net11Showcase.Data;

public sealed class CustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CustomerEntity>> GetAllAsync()
    {
        return await _db.Customers.AsNoTracking().ToListAsync();
    }

    public async Task<CustomerEntity?> GetHighestTierCustomerAsync()
    {
        // Modern LINQ MaxBy
        var customers = await _db.Customers.AsNoTracking().ToListAsync();
        return customers.MaxBy(c => c.CreatedAt);
    }

    public async Task<List<VectorSearchResult>> SearchSimilarAsync(float[] queryVector, int topK = 3)
    {
        var customers = await _db.Customers.AsNoTracking().ToListAsync();
        
        // Calculate Cosine Similarity over vector embeddings in memory (or via vector extension)
        var results = customers.Select(c =>
        {
            double sim = CalculateCosineSimilarity(queryVector, c.EmbeddingVector);
            return new VectorSearchResult
            {
                EntityId = c.Id,
                Title = c.FullName,
                Content = $"{c.Tier} • Email: {c.Email} • Metadata: {c.MetadataJson}",
                CosineSimilarity = Math.Round(sim, 3),
                MatchedCategory = c.Tier
            };
        })
        .OrderByDescending(r => r.CosineSimilarity)
        .Take(topK)
        .ToList();

        return results;
    }

    private static double CalculateCosineSimilarity(float[] v1, float[] v2)
    {
        if (v1.Length == 0 || v2.Length == 0) return 0.5;
        int len = Math.Min(v1.Length, v2.Length);
        double dot = 0.0, mag1 = 0.0, mag2 = 0.0;
        for (int i = 0; i < len; i++)
        {
            dot += v1[i] * v2[i];
            mag1 += v1[i] * v1[i];
            mag2 += v2[i] * v2[i];
        }
        if (mag1 == 0 || mag2 == 0) return 0.0;
        return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
    }
}
