using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public static class TileSelectionHelper
{
    public static void DrawBounds(Tilemap map, Vector3Int a, Vector3Int b)
    {
        DrawBounds(map, new BoundsInt(a, b - a + new Vector3Int(1, 1, 1)));
    }

    public static void DrawBounds(Tilemap map, BoundsInt bounds)
    {
        Vector3Int[] corners = GetCorners(bounds);

        DrawLine(map.CellToWorld(corners[0]), map.CellToWorld(corners[1]));
        DrawLine(map.CellToWorld(corners[1]), map.CellToWorld(corners[2]));
        DrawLine(map.CellToWorld(corners[2]), map.CellToWorld(corners[3]));
        DrawLine(map.CellToWorld(corners[3]), map.CellToWorld(corners[0]));
    }

    private static Vector3Int[] GetCorners(BoundsInt bounds)
    {
        Vector3Int min = bounds.min;
        Vector3Int max = bounds.max;

        return new Vector3Int[]
        {
            min,
            new Vector3Int(min.x, max.y, min.z),
            max,
            new Vector3Int(max.x, min.y, min.z)
        };
    }

    private static void DrawLine(Vector3 a, Vector3 b)
    {
        Handles.DrawLine(a, b);
    }
}
