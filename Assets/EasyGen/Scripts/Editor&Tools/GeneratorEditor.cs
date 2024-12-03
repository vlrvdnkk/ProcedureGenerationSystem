using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Generator))]
public class InteliMapGeneratorEditor : Editor
{
    private Generator mapGenerator;

    private void OnEnable()
    {
        mapGenerator = (Generator)target;

        EditorApplication.update += UpdateGenerator;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateGenerator;
    }

    private void UpdateGenerator()
    {
        mapGenerator.fillCoroutine?.MoveNext();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10.0f);

        DrawGenerationButtons();

        GUILayout.Space(10.0f);

        ShowWarnings();
        ShowGeneratorInfo();
    }

    private void DrawGenerationButtons()
    {
        if (GUILayout.Button("Clear Bounds"))
        {
            RecordMapUndo();
            mapGenerator.ClearBounds();
        }

        if (GUILayout.Button("Generate"))
        {
            RecordMapUndo();
            TryGenerate();
        }

        if (GUILayout.Button("Clear and Generate"))
        {
            RecordMapUndo();
            mapGenerator.ClearBounds();
            TryGenerate();
        }
    }

    private void TryGenerate()
    {
        try
        {
            mapGenerator.StartGeneration();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void ShowWarnings()
    {
        if (mapGenerator.generatorData == null)
        {
            EditorGUILayout.HelpBox("WARNING: This generator has no generator data assigned to it. Use the InteliMapBuilder component to build generator data.", MessageType.Warning);
        }
        else if (mapGenerator.mapToFill == null)
        {
            EditorGUILayout.HelpBox("WARNING: Empty mapToFill. Specify the map to fill for generation.", MessageType.Warning);
        }
        else if (mapGenerator.mapToFill.Count != mapGenerator.generatorData.layerCount)
        {
            EditorGUILayout.HelpBox($"WARNING: Invalid mapToFill. This generator expects {mapGenerator.generatorData.layerCount} layers, but {mapGenerator.mapToFill.Count} layers are provided.", MessageType.Warning);
        }
    }

    private void ShowGeneratorInfo()
    {
        if (mapGenerator.generatorData != null)
        {
            GUILayout.Label("Generator Info:");
            GUILayout.Label($"      {mapGenerator.generatorData.layerCount} layers.");
            GUILayout.Label($"      {mapGenerator.NumUniqueTiles()} unique tiles.");
            GUILayout.Label($"      Neighborhood radius of {mapGenerator.GetNeighborhoodRadius()}.");
            GUILayout.Label($"      {mapGenerator.GetParameterCount()} total parameters.");
            GUILayout.Label($"      {mapGenerator.generatorData.weights.epochsTrained} total epochs trained.");
            GUILayout.Label($"      Coupling: {CouplingData.GetCouplingTypeString(mapGenerator.generatorData.connectivityType)}");
            GUILayout.Label($"      Positional Inputs: X: [{(mapGenerator.generatorData.weights.useXPositionAsInput ? "X" : "_")}] Y: [{(mapGenerator.generatorData.weights.useYPositionAsInput ? "X" : "_")}]");
            GUILayout.Label($"      Acknowledges bounds: {mapGenerator.generatorData.weights.acknowledgeBounds}.");
            GUILayout.Label($"      Enforces border connectivity: {mapGenerator.generatorData.borderConnectivity.enforceConnectivity}.");
        }
    }

    private void RecordMapUndo()
    {
        for (int layer = 0; layer < mapGenerator.generatorData.layerCount; layer++)
        {
            Undo.RecordObject(mapGenerator.mapToFill[layer], mapGenerator.mapToFill[layer].name);
        }
    }
}
