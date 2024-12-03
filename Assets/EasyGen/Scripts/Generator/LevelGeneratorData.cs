using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorData : ScriptableObject
{
    public int layerCount = 1;

    public TileLayerManager[] UniqueTiles;
    public LevelGeneratorWeights Weights;
    public CouplingType CouplingType;

    public bool[] CouplingData;
    public BorderManager BorderCoupling;
}