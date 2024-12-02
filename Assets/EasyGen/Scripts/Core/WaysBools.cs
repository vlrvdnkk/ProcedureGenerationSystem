using System;
using UnityEngine;

[Serializable]
public struct WaysBools
{
    [Tooltip("The state for the top direction (positive Y).")]
    [SerializeField] public bool Top;

    [Tooltip("The state for the bottom direction (negative Y).")]
    [SerializeField] public bool Bottom;

    [Tooltip("The state for the left direction (negative X).")]
    [SerializeField] public bool Left;

    [Tooltip("The state for the right direction (positive X).")]
    [SerializeField] public bool Right;

    public WaysBools(bool top, bool bottom, bool left, bool right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public static bool operator == (WaysBools lhs, WaysBools rhs)
    {
        return lhs.Top == rhs.Top &&
               lhs.Bottom == rhs.Bottom &&
               lhs.Left == rhs.Left &&
               lhs.Right == rhs.Right;
    }

    public static bool operator != (WaysBools lhs, WaysBools rhs)
    {
        return !(lhs == rhs);
    }

    public override string ToString()
    {
        return $"{(Top ? "[T" : "[_")} {(Bottom ? "B" : "_")} {(Left ? "L" : "_")} {(Right ? "R]" : "_]")}";
    }
    
    public override bool Equals(object obj)
    {
        if (obj is WaysBools other)
        {
            return this == other;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Top, Bottom, Left, Right);
    }
}
