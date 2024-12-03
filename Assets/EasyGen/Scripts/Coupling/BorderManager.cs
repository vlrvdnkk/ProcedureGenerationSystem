using System;
using UnityEngine;

[Serializable]
public class BorderManager
{
    [SerializeField] private bool[] topBorder;
    [SerializeField] private bool[] bottomBorder;
    [SerializeField] private bool[] leftBorder;
    [SerializeField] private bool[] rightBorder;
    public WaysBools enforceBorder;

    public BorderManager(int tileCount, WaysBools enforceBorder)
    {
        topBorder = new bool[tileCount];
        bottomBorder = new bool[tileCount];
        leftBorder = new bool[tileCount];
        rightBorder = new bool[tileCount];

        this.enforceBorder = enforceBorder;
    }

    public bool GetConnectivity(int index, Direction direction)
    {
        return direction switch
        {
            Direction.Top => topBorder[index],
            Direction.Bottom => bottomBorder[index],
            Direction.Left => leftBorder[index],
            Direction.Right => rightBorder[index],
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };
    }

    public void SetConnectivity(int index, Direction direction, bool value)
    {
        switch (direction)
        {
            case Direction.Top:
                topBorder[index] = value;
                break;
            case Direction.Bottom:
                bottomBorder[index] = value;
                break;
            case Direction.Left:
                leftBorder[index] = value;
                break;
            case Direction.Right:
                rightBorder[index] = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction");
        }
    }
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}
