using System.Collections.Generic;
using UnityEngine;

public class GeneratorEngine
{
    private LevelGeneratorWeights weights;
    private int neighborhoodRadius;
    private BoundsInt bounds;
    private int uniqueCount;
    private System.Random random;

    private PriorityHeap<float> taskQueue;
    private float[,,] unactivated;
    private float[,,] activated;

    public GeneratorEngine(LevelGeneratorWeights weights, System.Random random, BoundsInt bounds, int uniqueCount)
    {
        this.weights = weights;
        this.neighborhoodRadius = weights.GetNeighborhoodRadius();
        this.bounds = bounds;
        this.uniqueCount = uniqueCount;
        this.random = random;

        taskQueue = new PriorityHeap<float>(bounds);
        unactivated = new float[bounds.size.x, bounds.size.y, uniqueCount];
        activated = new float[bounds.size.x, bounds.size.y, uniqueCount];
    }

    public void Reset(System.Random threadRand, int[] mapIndices)
    {
        taskQueue.Clear();

        int width = bounds.size.x;
        int height = bounds.size.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int i = 0; i < uniqueCount; i++)
                {
                    float bias = weights.GetBias(i);
                    bias += weights.useXPositionAsInput ? ((x / (float)width) * 2.0f + 1.0f) * weights.xPositionWeights[i] : 0;
                    bias += weights.useYPositionAsInput ? ((y / (float)height) * 2.0f + 1.0f) * weights.yPositionWeights[i] : 0;
                    unactivated[x, y, i] = bias;
                }
            }
        }

        ApplyNeighborhoodWeights(mapIndices);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapIndices[x + y * width] == -1)
                {
                    taskQueue.Enqueue(new Vector2Int(x, y), Activate(x, y));
                }
            }
        }
    }

    private void ApplyNeighborhoodWeights(int[] mapIndices)
    {
        int width = bounds.size.x;
        int height = bounds.size.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int i = 0; i < uniqueCount; i++)
                {
                    ApplyWeightsForTile(x, y, i, mapIndices);
                }
            }
        }
    }

    private void ApplyWeightsForTile(int x, int y, int tileIndex, int[] mapIndices)
    {
        int width = bounds.size.x;
        int height = bounds.size.y;

        for (int offsetX = -neighborhoodRadius; offsetX <= neighborhoodRadius; offsetX++)
        {
            for (int offsetY = -neighborhoodRadius; offsetY <= neighborhoodRadius; offsetY++)
            {
                int neighborX = x + offsetX;
                int neighborY = y + offsetY;

                if (IsWithinBounds(neighborX, neighborY, width, height))
                {
                    int neighborIndex = mapIndices[neighborX + neighborY * width];
                    if (neighborIndex >= 0)
                    {
                        unactivated[x, y, tileIndex] += weights.GetWeight(tileIndex, offsetX + neighborhoodRadius, offsetY + neighborhoodRadius, neighborIndex);
                    }
                }
                else
                {
                    ApplyOutOfBoundsWeight(x, y, tileIndex, offsetX, offsetY);
                }
            }
        }
    }

    private bool IsWithinBounds(int x, int y, int width, int height)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private void ApplyOutOfBoundsWeight(int x, int y, int tileIndex, int offsetX, int offsetY)
    {
        if (offsetX < 0 /*&& weights.acknowledgeBounds.left*/)
        {
            unactivated[x, y, tileIndex] += weights.GetWeight(tileIndex, offsetX + neighborhoodRadius, offsetY + neighborhoodRadius, uniqueCount);
        }
        else if (offsetY < 0 /*&& weights.acknowledgeBounds.bottom*/)
        {
            unactivated[x, y, tileIndex] += weights.GetWeight(tileIndex, offsetX + neighborhoodRadius, offsetY + neighborhoodRadius, uniqueCount + 1);
        }
        else if (offsetX >= bounds.size.x /*&& weights.acknowledgeBounds.right*/)
        {
            unactivated[x, y, tileIndex] += weights.GetWeight(tileIndex, offsetX + neighborhoodRadius, offsetY + neighborhoodRadius, uniqueCount + 2);
        }
        else if (offsetY >= bounds.size.y /*&& weights.acknowledgeBounds.top*/)
        {
            unactivated[x, y, tileIndex] += weights.GetWeight(tileIndex, offsetX + neighborhoodRadius, offsetY + neighborhoodRadius, uniqueCount + 3);
        }
    }

    public bool IsDone() => taskQueue.IsEmpty();

    public Vector2Int NextPos() => taskQueue.Dequeue();

    public int PredictAndCollapse(Vector2Int pos, float rerolls)
    {
        int collapsedTo = Predict(pos, rerolls);
        UpdateAfterCollapse(pos, collapsedTo);
        return collapsedTo;
    }

    private int Predict(Vector2Int pos, float rerolls)
    {
        float maxConfidence = 0.0f;
        int bestIndex = uniqueCount - 1;

        for (int i = 0; i < uniqueCount; i++)
        {
            float confidence = activated[pos.x, pos.y, i];
            if (confidence > maxConfidence)
            {
                maxConfidence = confidence;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    private void UpdateAfterCollapse(Vector2Int pos, int collapsedTo)
    {
        for (int offsetX = -neighborhoodRadius; offsetX <= neighborhoodRadius; offsetX++)
        {
            for (int offsetY = -neighborhoodRadius; offsetY <= neighborhoodRadius; offsetY++)
            {
                int neighborX = pos.x + offsetX;
                int neighborY = pos.y + offsetY;

                if (IsWithinBounds(neighborX, neighborY, bounds.size.x, bounds.size.y))
                {
                    for (int i = 0; i < uniqueCount; i++)
                    {
                        unactivated[neighborX, neighborY, i] += weights.GetWeight(i, offsetX + neighborhoodRadius, offsetY + neighborhoodRadius, collapsedTo);
                    }

                    taskQueue.Update(new Vector2Int(neighborX, neighborY), Activate(neighborX, neighborY));
                }
            }
        }
    }

    private float Activate(int x, int y)
    {
        float total = 0.0f;
        float maxConfidence = 0.0f;

        for (int i = 0; i < uniqueCount; i++)
        {
            activated[x, y, i] = Mathf.Exp(unactivated[x, y, i]);
            total += activated[x, y, i];
        }

        for (int i = 0; i < uniqueCount; i++)
        {
            activated[x, y, i] /= total;
            maxConfidence = Mathf.Max(maxConfidence, activated[x, y, i]);
        }

        return maxConfidence;
    }
}
