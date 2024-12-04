using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneratorData))]
public class LevelDataEditor : Editor
{
    private GeneratorData lgd;

    private void OnEnable()
    {
        lgd = (GeneratorData)target;
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
        var uniqueTileCount = lgd.uniqueTiles.Length;
        var neighborhoodRadius = lgd.weights.GetNeighborhoodRadius();
        var parameterCount = lgd.weights.GetParameterCount();
        var trainingEpochs = lgd.weights.TrainingEpochs;
        var couplingType = CouplingData.GetCouplingTypeString(lgd.couplingType);
        var xPositionInput = lgd.weights.useXPositionAsInput ? "X" : "_";
        var yPositionInput = lgd.weights.useYPositionAsInput ? "X" : "_";
        var acknowledgeBounds = lgd.weights.acknowledgeBounds;
        var enforceBorder = lgd.borderCoupling.enforceBorder;

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
