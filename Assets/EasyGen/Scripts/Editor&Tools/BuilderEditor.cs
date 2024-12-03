using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Builder))]
public class BuilderEditor : Editor
{
    private Builder mapBuilder;

    private void OnEnable()
    {
        mapBuilder = (Builder)target;
    }

    public override void OnInspectorGUI()
    {
        if (mapBuilder.buildResult == GeneratorBuildResult.InProgress)
        {
            BuildInProgressUI();
        }
        else
        {
            if (mapBuilder.generator != null)
            {
                ShowGeneratorSettingsWarning();
            }

            base.OnInspectorGUI();

            BuildGeneratorUI();

            ShowBuildResultStatus();
        }
    }

    private void BuildInProgressUI()
    {
        if (GUILayout.Button("Cancel Build"))
            mapBuilder.CancelBuild();

        if (GUILayout.Button("Save and Quit Build"))
            mapBuilder.SaveAndQuitBuild();
    }

    private void ShowGeneratorSettingsWarning()
    {
        if (mapBuilder.neighborhoodRadius != mapBuilder.generator.weights.GetNeighborhoodRadius() ||
            mapBuilder.generatorSettings.acknowledgeBounds != mapBuilder.generator.weights.acknowledgeBounds ||
            mapBuilder.generatorSettings.useXPositionAsInput != mapBuilder.generator.weights.useXPositionAsInput ||
            mapBuilder.generatorSettings.useYPositionAsInput != mapBuilder.generator.weights.useYPositionAsInput)
        {
            EditorGUILayout.HelpBox("Some generator settings cannot be changed once the generator is created. Changes will not apply during training.", MessageType.Info);
            if (GUILayout.Button("Restore Generator Settings"))
                RestoreGeneratorSettings();
        }

        GUILayout.Space(20.0f);
    }

    private void RestoreGeneratorSettings()
    {
        mapBuilder.neighborhoodRadius = mapBuilder.generator.weights.GetNeighborhoodRadius();
        mapBuilder.generatorSettings.acknowledgeBounds = mapBuilder.generator.weights.acknowledgeBounds;
        mapBuilder.generatorSettings.useXPositionAsInput = mapBuilder.generator.weights.useXPositionAsInput;
        mapBuilder.generatorSettings.useYPositionAsInput = mapBuilder.generator.weights.useYPositionAsInput;
    }

    private void BuildGeneratorUI()
    {
        GUILayout.Space(20.0f);

        if (GUILayout.Button("Build Generator"))
            mapBuilder.Build();

        if (mapBuilder.generator != null && GUILayout.Button("Create Generator Component"))
        {
            var gen = mapBuilder.gameObject.AddComponent<Generator>();
            gen.Build(mapBuilder.buildMaps[0].mapLayers, mapBuilder.generator);
        }

        Generator generator = mapBuilder.gameObject.GetComponent<Generator>();

        if (mapBuilder.generator != null && generator != null && GUILayout.Button("Replace Generator Component"))
        {
            generator.Build(mapBuilder.buildMaps[0].mapLayers, mapBuilder.generator);
        }
    }

    private void ShowBuildResultStatus()
    {
        switch (mapBuilder.buildResult)
        {
            case GeneratorBuildResult.None:
                GUILayout.Space(20.0f);
                EditorGUILayout.HelpBox("Generator not built. Input maps and click 'Build Generator'.", MessageType.None);
                break;

            case GeneratorBuildResult.InProgress:
                ShowBuildInProgressStatus();
                break;

            case GeneratorBuildResult.Cancelled:
                ShowBuildCancelledStatus();
                break;

            case GeneratorBuildResult.Success:
                ShowBuildSuccessStatus();
                break;

            case GeneratorBuildResult.NanError:
                ShowError("Build terminated due to NaN value, likely due to a high learning rate.");
                break;

            case GeneratorBuildResult.MismatchedLayers:
                ShowError("Error: Layer mismatch in build maps.");
                break;

            case GeneratorBuildResult.NullMaps:
                ShowError("Error: Some build maps are null.");
                break;

            case GeneratorBuildResult.ZeroMaps:
                ShowError("Error: No maps inputted.");
                break;

            case GeneratorBuildResult.InvalidCommonality:
                ShowError("Error: Invalid commonality in build maps.");
                break;
        }
    }

    private void ShowBuildInProgressStatus()
    {
        GUILayout.Space(20.0f);
        EditorGUILayout.HelpBox($"Build in progress: Epoch {mapBuilder.epoch}/{mapBuilder.epochs}\n" +
            $"Avg Loss (Last 20 Epochs): {mapBuilder.avgLossLast20Epochs}\n" +
            $"Time Elapsed: {DateTime.Now.Subtract(mapBuilder.startTime):hh\\:mm\\:ss}\n" +
            $"Current Learning Rate: {mapBuilder.currentLearningRate}", MessageType.None);
    }

    private void ShowBuildCancelledStatus()
    {
        GUILayout.Space(20.0f);
        EditorGUILayout.HelpBox("Build was cancelled.", MessageType.Warning);
    }

    private void ShowBuildSuccessStatus()
    {
        if (mapBuilder.endTime.Subtract(mapBuilder.startTime).Milliseconds > 0)
        {
            GUILayout.Space(20.0f);
            EditorGUILayout.HelpBox($"Build successful!\nTime taken: {mapBuilder.endTime.Subtract(mapBuilder.startTime):hh\\:mm\\:ss}\nEpochs trained: {mapBuilder.epoch}", MessageType.Info);
        }
    }

    private void ShowError(string errorMessage)
    {
        GUILayout.Space(20.0f);
        EditorGUILayout.HelpBox($"ERROR: {errorMessage}", MessageType.Error);
    }
}
