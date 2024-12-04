using System;
using UnityEngine;

public class FourTilesCoupling : CouplingData
{
    private bool[] coupling;
    private int tileCount;

    public FourTilesCoupling(int tileCount)
    {
        this.tileCount = tileCount;
        coupling = new bool[4 * tileCount * tileCount];
    }

    public FourTilesCoupling(int tileCount, bool[] coupling)
    {
        this.tileCount = tileCount;
        this.coupling = coupling;
    }

    public override CouplingType Type => CouplingType.FourTiles;

    private static readonly Vector2Int[] Directions =
    {
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right
    };

    public override int GetDirectionCount() => 4;

    public override int GetOppositeDirection(int direction) => (direction + 2) % 4;

    public override bool[] GetCouplingArray() => coupling;

    public override Vector2Int GetCouplingOffset(int direction, Vector2Int pos, int startY)
        => Directions[direction];

    public override bool GetCoupling(int indexA, int indexB, int direction)
        => coupling[direction * tileCount * tileCount + indexA * tileCount + indexB];

    public override void SetCoupling(int indexA, int indexB, int direction, bool value)
        => coupling[direction * tileCount * tileCount + indexA * tileCount + indexB] = value;
}
