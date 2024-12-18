using InteliMapPro;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

[EditorTool("Schematic Generator Tool", typeof(Builder))]
public class MapAreaTool : TilemapAreaTool
{
    private Tilemap tileMap;

    public override void OnEnable()
    {
        base.OnEnable();
        tileMap = FindObjectOfType<Tilemap>();
    }

    public override Color handleColor => Color.magenta;

    public override void OnToolGUI(EditorWindow window)
    {
        base.OnToolGUI(window);
    }

    public override void OnFinish()
    {
        if (!(target is Builder builder)) return;

        Vector3Int mins = Vector3Int.Min(start, stop);
        Vector3Int maxs = Vector3Int.Max(start, stop);

        if (mins == maxs) return;

        Undo.RecordObject(builder, builder.name);

        if (builder.buildMaps == null)
        {
            builder.buildMaps = new List<GeneratorMap>();
        }

        builder.buildMaps.Add(new GeneratorMap(
            new List<Tilemap>(FindObjectsOfType<Tilemap>()),
            new BoundsInt(mins, maxs - mins + Vector3Int.one)
        ));
    }
}
