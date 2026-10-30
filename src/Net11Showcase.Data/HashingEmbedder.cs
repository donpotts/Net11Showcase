using System;

namespace Net11Showcase.Data;

/// <summary>
/// Deterministic, dependency-free text embedder using signed feature hashing. Same text always produces the same
/// unit vector, so vector search results are reproducible offline. Swap for a real embedding model in production.
/// </summary>
public static class HashingEmbedder
{
    public static float[] Embed(string? text, int dimensions = 4)
    {
        if (dimensions <= 0) throw new ArgumentOutOfRangeException(nameof(dimensions));
        var vector = new float[dimensions];
        if (string.IsNullOrWhiteSpace(text)) return vector;

        foreach (var token in Tokenize(text))
        {
            var hash = Fnv1a(token);
            var bucket = (int)(hash % (uint)dimensions);
            vector[bucket] += ((hash >> 16) & 1) == 0 ? 1f : -1f;
        }

        double norm = 0;
        foreach (var v in vector) norm += v * v;
        if (norm == 0) return vector;

        var scale = (float)(1.0 / Math.Sqrt(norm));
        for (var i = 0; i < vector.Length; i++) vector[i] *= scale;
        return vector;
    }

    private static string[] Tokenize(string text)
    {
        var chars = text.ToLowerInvariant().ToCharArray();
        for (var i = 0; i < chars.Length; i++)
            if (!char.IsLetterOrDigit(chars[i])) chars[i] = ' ';
        return new string(chars).Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    private static uint Fnv1a(string s)
    {
        var hash = 2166136261u;
        foreach (var c in s)
        {
            hash ^= c;
            hash *= 16777619u;
        }
        return hash;
    }
}
