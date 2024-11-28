using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class GeneratorMap
{
    [Tooltip("Tilemap objects to analyze for building the generator. Supports multi-layered tilemaps.")]
    public List<Tilemap> MapLayers { get; private set; } = new List<Tilemap>();

    [Tooltip("The relative importance of this map in training. Lower values prioritize maps with rare structures.")]
    [Range(0.1f, 10f)]
    public float Universality = 1.0f;

    [Tooltip("Toggle to use manual boundaries or the entire tilemap.")]
    public bool IsCustomBoundsEnabled = false;

    [Tooltip("Map boundaries for analysis when manual bounds are enabled.")]
    public BoundsInt CustomBounds = new BoundsInt(Vector3Int.zero, new Vector3Int(25, 25, 1));

    public GeneratorMap(List<Tilemap> mapLayers, BoundsInt bounds)
    {
        MapLayers = new List<Tilemap>(mapLayers);
        Universality = 1.0f;
        IsCustomBoundsEnabled = true;
        CustomBounds = bounds;
    }
}
