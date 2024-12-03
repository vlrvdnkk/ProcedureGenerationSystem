using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

[EditorTool("Tilemap Generator Tool", typeof(Generator))]
[Tooltip("Generator Tool")]
public class SchematicTool : TilemapAreaTool
{
    private bool areaSelected = false;
    private Vector3Int areaStart = Vector3Int.zero;
    private Vector3Int areaEnd = Vector3Int.zero;
    private BoundsInt areaBounds;
    private TileBase[][] original;
    private bool clear = false;
    private Generator mg;

    public override void OnEnable()
    {
        base.OnEnable();
        mg = target as Generator;
        map = mg.mapToFill[0];
        areaSelected = false;
    }

    public override Color handleColor => areaSelected ? Color.magenta : Color.cyan;

    public override void OnToolGUI(EditorWindow window)
    {
        base.OnToolGUI(window);

        Handles.BeginGUI();

        GUILayout.BeginHorizontal();
        GUILayout.Space(60);
        clear = GUILayout.Toggle(clear, "Clear on Generate");

        if (areaSelected)
        {
            DrawAreaSelectedButtons();
        }

        GUILayout.EndHorizontal();

        Handles.EndGUI();
    }

    private void DrawAreaSelectedButtons()
    {
        if (GUILayout.Button("Set as Generation Bounds"))
        {
            SetGenerationBounds();
        }
        if (GUILayout.Button("Revert to Original"))
        {
            RevertToOriginal();
        }
        if (GUILayout.Button("Clear Area"))
        {
            ClearArea();
        }
        if (GUILayout.Button("Retry Generation"))
        {
            RetryGeneration();
        }
    }

    private void SetGenerationBounds()
    {
        Undo.RecordObject(mg, mg.name);
        mg.boundsToFill = areaBounds;
    }

    private void RevertToOriginal()
    {
        RecordMapUndo();
        for (int layer = 0; layer < mg.generatorData.layerCount; layer++)
        {
            mg.mapToFill[layer].SetTilesBlock(areaBounds, original[layer]);
        }
    }

    private void ClearArea()
    {
        RecordMapUndo();
        BoundsInt previousBounds = mg.boundsToFill;
        mg.boundsToFill = areaBounds;
        mg.ClearBounds();
        mg.boundsToFill = previousBounds;
    }

    private void RetryGeneration()
    {
        RecordMapUndo();
        BoundsInt previousBounds = mg.boundsToFill;
        mg.boundsToFill = areaBounds;

        if (clear) mg.ClearBounds();
        else RestoreOriginalTiles();

        try
        {
            mg.StartGeneration();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message + " " + ex.StackTrace);
        }

        mg.boundsToFill = previousBounds;
    }

    private void RestoreOriginalTiles()
    {
        for (int layer = 0; layer < mg.generatorData.layerCount; layer++)
        {
            mg.mapToFill[layer].SetTilesBlock(areaBounds, original[layer]);
        }
    }

    public override void OnFinish()
    {
        areaStart = start;
        areaEnd = stop;
        areaBounds = new BoundsInt(Vector3Int.Min(start, stop), Vector3Int.Max(start, stop) - Vector3Int.Min(start, stop) + Vector3Int.one);
        mg.boundsToFill = areaBounds;
        areaSelected = true;
        SaveOriginalTiles();
        if (clear) mg.ClearBounds();
        TryGeneration();
        forceDraw = true;
    }

    private void SaveOriginalTiles()
    {
        original = new TileBase[mg.generatorData.layerCount][];
        for (int layer = 0; layer < mg.generatorData.layerCount; layer++)
        {
            original[layer] = mg.mapToFill[layer].GetTilesBlock(areaBounds);
        }
    }

    private void TryGeneration()
    {
        try
        {
            mg.StartGeneration();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void RecordMapUndo()
    {
        for (int layer = 0; layer < mg.generatorData.layerCount; layer++)
        {
            Undo.RecordObject(mg.mapToFill[layer], mg.mapToFill[layer].name);
        }
    }
}
