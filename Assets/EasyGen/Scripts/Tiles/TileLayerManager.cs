using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct TileLayerManager : IEqualityComparer<TileLayerManager>
{
    [SerializeField] public TileBase[] Tiles;

    public TileLayerManager(int layerCount)
    {
        Tiles = new TileBase[layerCount];
    }

    public TileLayerManager(TileBase[] tiles)
    {
        Tiles = tiles;
    }

    public bool IsEmpty()
    {
        foreach (var tile in Tiles)
        {
            if (tile != null) return false;
        }
        return true;
    }

    public bool Equals(TileLayerManager x, TileLayerManager y)
    {
        if (x.Tiles.Length != y.Tiles.Length) return false;

        for (int i = 0; i < x.Tiles.Length; i++)
        {
            if (!Equals(x.Tiles[i], y.Tiles[i]))
                return false;
        }

        return true;
    }

    public int GetHashCode(TileLayerManager obj)
    {
        int hash = 17;
        foreach (var tile in obj.Tiles)
        {
            hash = hash * 31 + (tile != null ? tile.GetHashCode() : 0);
        }
        return hash;
    }
}

public class LayeredTileComparer : IEqualityComparer<TileLayerManager>
{
    public bool Equals(TileLayerManager x, TileLayerManager y)
    {
        return x.Equals(x, y);
    }

    public int GetHashCode(TileLayerManager obj)
    {
        return obj.GetHashCode(obj);
    }
}
