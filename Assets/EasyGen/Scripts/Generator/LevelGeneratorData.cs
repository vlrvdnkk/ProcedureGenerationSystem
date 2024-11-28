using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorData : ScriptableObject
{
    public int layerCount = 1;

    public LevelGeneratorWeights weights;

    public bool[] connectivityData;
}