using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileBoundsDrawer
{
    public static void DrawBounds(Tilemap map, Vector3Int a, Vector3Int b)
    {
        DrawBounds(map, new BoundsInt(a, b - a));
    }

    public static void DrawBounds(Tilemap map, BoundsInt bounds)
    {
        TileBoundsLogic.DrawBounds(map, bounds, Gizmos.DrawLine);
    }
}
