using System;
using UnityEngine;

[Serializable]
public class LevelGeneratorWeights
{
    [SerializeField] private float[] weights;
    [SerializeField] private float[] biases;
    [HideInInspector][SerializeField] private int dim1multi;
    [HideInInspector][SerializeField] private int dim2multi;
    [HideInInspector][SerializeField] private int dim3multi;

    [SerializeField] public int TrainingEpochs;
    [SerializeField] private int neighborhoodRange;

    [SerializeField] public bool useXPositionAsInput;
    [SerializeField] public float[] xPositionWeights;
    [SerializeField] public bool useYPositionAsInput;
    [SerializeField] public float[] yPositionWeights;

    [SerializeField] public WaysBools acknowledgeBounds;

    public LevelGeneratorWeights(int uniqueTileCount, int neighborhoodRadius, bool useXPositionAsInput, bool useYPositionAsInput, WaysBools acknowledgeBounds)
    {
        this.acknowledgeBounds = acknowledgeBounds;
        this.neighborhoodRange = neighborhoodRadius;
        this.useXPositionAsInput = useXPositionAsInput;
        this.useYPositionAsInput = useYPositionAsInput;

        int neighborhoodSideLength = (neighborhoodRadius * 2 + 1);
        int neighborhoodArea = neighborhoodSideLength * neighborhoodSideLength;

        dim3multi = uniqueTileCount + 4;
        dim2multi = (neighborhoodRadius * 2 + 1) * dim3multi;
        dim1multi = (neighborhoodRadius * 2 + 1) * dim2multi;
        weights = new float[uniqueTileCount * dim1multi];

        float bound = 1.0f / Mathf.Sqrt(neighborhoodArea);
        for (int i = 0; i < uniqueTileCount; i++)
        {
            for (int j = 0; j < dim1multi; j++)
            {
                weights[i * dim1multi + j] = UnityEngine.Random.Range(-bound, bound);
            }
        }

        biases = new float[uniqueTileCount];
        for (int i = 0; i < uniqueTileCount; i++)
        {
            biases[i] = 0.0f;
        }

        xPositionWeights = new float[uniqueTileCount];
        yPositionWeights = new float[uniqueTileCount];

        for (int i = 0; i < uniqueTileCount; i++)
        {
            xPositionWeights[i] = UnityEngine.Random.Range(-bound, bound);
            yPositionWeights[i] = UnityEngine.Random.Range(-bound, bound);
        }

        TrainingEpochs = 0;
    }

    public int GetParameterCount()
    {
        return weights.Length + biases.Length + xPositionWeights.Length + yPositionWeights.Length;
    }

    public int GetNeighborhoodRadius()
    {
        return neighborhoodRange;
    }

    public float GetWeight(int tileToPlace, int nX, int nY, int tileAtLocation)
    {
        return weights[tileToPlace * dim1multi + nX * dim2multi + nY * dim3multi + tileAtLocation];
    }

    public void SetWeight(int tileToPlace, int nX, int nY, int tileAtLocation, float val)
    {
        weights[tileToPlace * dim1multi + nX * dim2multi + nY * dim3multi + tileAtLocation] = val;
    }

    public void AddToWeight(int tileToPlace, int nX, int nY, int tileAtLocation, float val)
    {
        weights[tileToPlace * dim1multi + nX * dim2multi + nY * dim3multi + tileAtLocation] += val;
    }

    public float GetBias(int tileToPlace)
    {
        return biases[tileToPlace];
    }

    public void AddToBias(int tileToPlace, float val)
    {
        biases[tileToPlace] += val;
    }
}
