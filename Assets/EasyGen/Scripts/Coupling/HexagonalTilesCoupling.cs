using System;
using UnityEngine;

public class HexagonalCoupling : CouplingData
{
    public HexagonalCoupling(int tileCount)
    {
        this.tileCount = tileCount;

        connectivity = new bool[6 * tileCount * tileCount];
    }

    public HexagonalCoupling(int tileCount, bool[] connectivity)
    {
        this.tileCount = tileCount;

        this.connectivity = connectivity;
    }

    public override CouplingType Type => CouplingType.HexagonalTiles;

    private bool[] connectivity;
    private int tileCount;

    public readonly static Vector2Int[] evenDirections = new Vector2Int[6] {
            Vector2Int.left, new Vector2Int(-1, 1), new Vector2Int(0, 1),
            Vector2Int.right, new Vector2Int(0, -1),  new Vector2Int(-1, -1)
        };

    public readonly static Vector2Int[] oddDirections = new Vector2Int[6] {
            Vector2Int.left, new Vector2Int(0, 1), new Vector2Int(1, 1), // l, tl, tr
            Vector2Int.right, new Vector2Int(1, -1), new Vector2Int(0, -1) // r, br, bl
        };

    public override int GetDirectionCount()
    {
        return 6;
    }

    public override int GetOppositeDirection(int direction)
    {
        return (direction + 3) % 6;
    }

    public override bool[] GetCouplingArray()
    {
        return connectivity;
    }

    public override Vector2Int GetCouplingOffset(int direction, Vector2Int pos, int startY)
    {
        return (Math.Abs(pos.y - startY) % 2 == 0) ? evenDirections[direction] : oddDirections[direction];
    }

    public override bool GetCoupling(int indexA, int indexB, int direction)
    {
        return connectivity[direction * tileCount * tileCount + indexA * tileCount + indexB];
    }

    public override void SetCoupling(int indexA, int indexB, int direction, bool value)
    {
        connectivity[direction * tileCount * tileCount + indexA * tileCount + indexB] = value;
    }
}
