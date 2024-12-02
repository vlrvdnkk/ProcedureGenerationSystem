using System;
using UnityEngine;

public enum CouplingType
{
    FourTiles,
    EightTiles,
    HexagonalTiles
}

public abstract class CouplingData
{
    public abstract CouplingType Type { get; }

    public abstract int GetDirectionCount();

    public abstract int GetOppositeDirection(int direction);

    public abstract bool[] GetCouplingArray();

    public abstract Vector2Int GetCouplingOffset(int direction, Vector2Int pos, int startY);

    public abstract bool GetCouple(int indexA, int indexB, int direction);

    public abstract void SetCouple(int indexA, int indexB, int direction, bool value);

    public int GetLCVHeuristic(Vector2Int pos, int startY, IndexedSet[,] domains, BoundsInt bounds, int index)
    {
        int size = 0;

        for (int d = 0; d < GetDirectionCount(); d++)
        {
            Vector2Int adjacentPosition = pos + GetCouplingOffset(d, pos, startY);

            if (IsWithinBounds(adjacentPosition, bounds))
            {
                IndexedSet domain = domains[adjacentPosition.x, adjacentPosition.y];
                foreach (int tileIndex in domain)
                {
                    if (!GetCouple(index, tileIndex, d))
                    {
                        size++;
                    }
                }
            }
        }

        return size;
    }

    private bool IsWithinBounds(Vector2Int pos, BoundsInt bounds)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < bounds.size.x && pos.y < bounds.size.y;
    }

    public static string GetConnectivityTypeString(CouplingType type) =>
        type switch
        {
            CouplingType.FourTiles => "Four way",
            CouplingType.EightTiles => "Eight way",
            CouplingType.HexagonalTiles => "Hexagonal",
            _ => "Invalid connectivity type"
        };
}
