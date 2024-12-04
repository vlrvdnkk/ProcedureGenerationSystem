using System;
using UnityEngine;

public class EightTilesCoupling : CouplingData
{
    private readonly bool[] connectivity;
    private readonly int tileCount;

    public EightTilesCoupling(int tileCount)
    {
        this.tileCount = tileCount;
        connectivity = new bool[8 * tileCount * tileCount];
    }

    public EightTilesCoupling(int tileCount, bool[] connectivity)
    {
        this.tileCount = tileCount;
        this.connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity));
    }

    public override CouplingType Type => CouplingType.EightTiles;

    private static readonly Vector2Int[] Directions =
    {
        new Vector2Int(-1, -1), Vector2Int.left, new Vector2Int(-1, 1), Vector2Int.up,
        new Vector2Int(1, 1), Vector2Int.right, new Vector2Int(1, -1), Vector2Int.down
    };

    public override int GetDirectionCount() => 8;

    public override int GetOppositeDirection(int direction) => (direction + 4) % 8;

    public override bool[] GetCouplingArray() => connectivity;

    public override Vector2Int GetCouplingOffset(int direction, Vector2Int pos, int startY)
        => Directions[direction];

    public override bool GetCoupling(int indexA, int indexB, int direction)
        => connectivity[direction * tileCount * tileCount + indexA * tileCount + indexB];

    public override void SetCoupling(int indexA, int indexB, int direction, bool value)
        => connectivity[direction * tileCount * tileCount + indexA * tileCount + indexB] = value;
}
