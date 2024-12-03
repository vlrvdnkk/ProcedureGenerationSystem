using System;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerationEngine
{
    private readonly Dictionary<int, HashSet<int>> domains;
    private readonly Dictionary<int, HashSet<int>> neighbors;
    private readonly Dictionary<(int, int), HashSet<int>> borderConstraints;
    private readonly Queue<int> processingQueue;
    private readonly HashSet<int> constrainedTiles;
    private readonly Random random;

    public LevelGenerationEngine(
        Dictionary<int, HashSet<int>> initialDomains,
        Dictionary<int, HashSet<int>> tileNeighbors,
        Dictionary<(int, int), HashSet<int>> constraints,
        int? seed = null)
    {
        domains = new Dictionary<int, HashSet<int>>(initialDomains);
        neighbors = new Dictionary<int, HashSet<int>>(tileNeighbors);
        borderConstraints = new Dictionary<(int, int), HashSet<int>>(constraints);
        processingQueue = new Queue<int>();
        constrainedTiles = new HashSet<int>();
        random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public bool Generate()
    {
        InitializeQueue();

        while (processingQueue.Count > 0)
        {
            int tile = processingQueue.Dequeue();

            if (!EnforceConstraints(tile))
            {
                return false;
            }
        }

        return true;
    }

    private void InitializeQueue()
    {
        foreach (var tile in domains.Keys)
        {
            processingQueue.Enqueue(tile);
        }
    }

    private bool EnforceConstraints(int tile)
    {
        foreach (int neighbor in neighbors[tile])
        {
            if (!Revise(tile, neighbor))
            {
                return false;
            }

            if (!processingQueue.Contains(neighbor))
            {
                processingQueue.Enqueue(neighbor);
            }
        }

        return true;
    }

    private bool Revise(int tile, int neighbor)
    {
        bool revised = false;
        var domainToRemove = new HashSet<int>();

        foreach (int value in domains[tile])
        {
            if (!HasValidNeighbor(value, tile, neighbor))
            {
                domainToRemove.Add(value);
                revised = true;
            }
        }

        foreach (int value in domainToRemove)
        {
            domains[tile].Remove(value);
        }

        if (domains[tile].Count == 0)
        {
            return false;
        }

        return revised;
    }

    private bool HasValidNeighbor(int tileValue, int tile, int neighbor)
    {
        foreach (int neighborValue in domains[neighbor])
        {
            if (borderConstraints.TryGetValue((tileValue, neighborValue), out var constraints) &&
                constraints.Contains(tileValue))
            {
                return true;
            }
        }

        return false;
    }

    public void AssignValue(int tile, int value)
    {
        domains[tile] = new HashSet<int> { value };
        processingQueue.Enqueue(tile);
    }

    public void PrintDomains()
    {
        foreach (var kvp in domains)
        {
            Console.WriteLine($"Tile {kvp.Key}: [{string.Join(", ", kvp.Value)}]");
        }
    }
}
