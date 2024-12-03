using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGeneratorData))]
public class LevelDataEditor : Editor
{
    private LevelGeneratorData lgd;

    private void OnEnable()
    {
        lgd = (LevelGeneratorData)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Generator Info:");

        DisplayGeneratorInfo();

        GUILayout.Space(20);

        base.OnInspectorGUI();
    }

    private void DisplayGeneratorInfo()
    {
        var layerCount = lgd.layerCount;
        var uniqueTileCount = lgd.UniqueTiles.Length;
        var neighborhoodRadius = lgd.Weights.GetNeighborhoodRadius();
        var parameterCount = lgd.Weights.GetParameterCount();
        var trainingEpochs = lgd.Weights.TrainingEpochs;
        var couplingType = CouplingData.GetCouplingTypeString(lgd.CouplingType);
        var xPositionInput = lgd.Weights.useXPositionAsInput ? "X" : "_";
        var yPositionInput = lgd.Weights.useYPositionAsInput ? "X" : "_";
        var acknowledgeBounds = lgd.Weights.acknowledgeBounds;
        var enforceBorder = lgd.BorderCoupling.enforceBorder;

        GUILayout.Label($"      {layerCount} layers.");
        GUILayout.Label($"      {uniqueTileCount} unique tiles.");
        GUILayout.Label($"      Neighborhood radius of {neighborhoodRadius}.");
        GUILayout.Label($"      {parameterCount} total parameters.");
        GUILayout.Label($"      {trainingEpochs} total epochs trained.");
        GUILayout.Label($"      Connectivity: {couplingType}");
        GUILayout.Label($"      Positional Inputs: X: [{xPositionInput}] Y: [{yPositionInput}]");
        GUILayout.Label($"      Acknowledges bounds: {acknowledgeBounds}.");
        GUILayout.Label($"      Enforces border connectivity: {enforceBorder}.");
    }
}
