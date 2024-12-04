using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorData : ScriptableObject
{
    public int layerCount = 1;

    public LayeredTile[] uniqueTiles;
    public GeneratorWeights weights;
    public CouplingType couplingType;

    public bool[] couplingData;
    public BorderManager borderCoupling;
}