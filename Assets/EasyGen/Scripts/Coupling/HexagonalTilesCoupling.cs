using System;
using UnityEngine;

public class HexagonalCoupling : CouplingData
{
    private readonly bool[] connectivity;
    private readonly int tileCount;

    private static readonly Vector2Int[] EvenDirections =
    {
        Vector2Int.left, new Vector2Int(-1, 1), new Vector2Int(0, 1),
        Vector2Int.right, new Vector2Int(0, -1), new Vector2Int(-1, -1)
    };

    private static readonly Vector2Int[] OddDirections =
    {
        Vector2Int.left, new Vector2Int(0, 1), new Vector2Int(1, 1),
        Vector2Int.right, new Vector2Int(1, -1), new Vector2Int(0, -1)
    };

    public HexagonalCoupling(int tileCount)
    {
        this.tileCount = tileCount;
        connectivity = new bool[6 * tileCount * tileCount];
    }

    public HexagonalCoupling(int tileCount, bool[] connectivity)
    {
        this.tileCount = tileCount;
        this.connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity));
    }

    public override CouplingType Type => CouplingType.HexagonalTiles;

    public override int GetDirectionCount() => 6;

    public override int GetOppositeDirection(int direction) => (direction + 3) % 6;

    public override bool[] GetCouplingArray() => connectivity;

    public override Vector2Int GetCouplingOffset(int direction, Vector2Int pos, int startY)
        => (Math.Abs(pos.y - startY) % 2 == 0) ? EvenDirections[direction] : OddDirections[direction];

    public override bool GetCouple(int indexA, int indexB, int direction)
        => connectivity[direction * tileCount * tileCount + indexA * tileCount + indexB];

    public override void SetCouple(int indexA, int indexB, int direction, bool value)
        => connectivity[direction * tileCount * tileCount + indexA * tileCount + indexB] = value;
}
