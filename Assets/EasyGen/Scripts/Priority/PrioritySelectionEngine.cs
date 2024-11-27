using System;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySelectionEngine
{
    private IndexedSet[,] tileSets;
    private Priority[,] priorities;

    private Vector2Int[] remainingPositions;
    private float randomChance;
    private int count;

    public PrioritySelectionEngine(IndexedSet[,] tileSets, Priority[,] priorities, BoundsInt bounds, float randomChance)
    {
        this.tileSets = tileSets;
        this.priorities = priorities;
        this.randomChance = randomChance;

        count = 0;
        remainingPositions = new Vector2Int[bounds.size.x * bounds.size.y];
    }

    public bool IsDone() => count == 0;

    public void Add(Vector2Int pos)
    {
        remainingPositions[count++] = pos;
    }

    public (Vector2Int, int) Next(System.Random rand)
    {
        int smallestSize = int.MaxValue;
        int highestPriority = 0;
        List<int> smallestIndices = new List<int>();

        for (int j = 0; j < count; j++)
        {
            Vector2Int pos = remainingPositions[j];
            Priority priority = priorities[pos.x, pos.y];
            int level = priority.PriorityLevel;
            int size;

            if (level > 0)
            {
                size = tileSets[pos.x, pos.y].OverlapCount(priority.PrioritySet);
                if (level > highestPriority)
                {
                    highestPriority = level;
                    smallestSize = size;
                    smallestIndices.Clear();
                    smallestIndices.Add(j);
                }
                else if (size < smallestSize && size != 0)
                {
                    smallestSize = size;
                    smallestIndices.Clear();
                    smallestIndices.Add(j);
                }
                else if (size == smallestSize)
                {
                    smallestIndices.Add(j);
                }
            }
            else if (highestPriority == 0)
            {
                size = tileSets[pos.x, pos.y].Count;
                if (size < smallestSize)
                {
                    smallestSize = size;
                    smallestIndices.Clear();
                    smallestIndices.Add(j);
                }
                else if (size == smallestSize)
                {
                    smallestIndices.Add(j);
                }
            }
        }

        if (highestPriority == 0 && (float)rand.NextDouble() < randomChance)
        {
            int randomIndex = rand.Next(count);
            Vector2Int randomPosition = remainingPositions[randomIndex];
            RemoveAt(randomIndex);
            return (randomPosition, 0);
        }

        int selectedIdx = smallestIndices[rand.Next(smallestIndices.Count)];
        Vector2Int selectedPosition = remainingPositions[selectedIdx];
        RemoveAt(selectedIdx);
        return (selectedPosition, highestPriority);
    }

    private void RemoveAt(int index)
    {
        count--;
        if (index < count)
        {
            remainingPositions[index] = remainingPositions[count];
        }
    }
}
