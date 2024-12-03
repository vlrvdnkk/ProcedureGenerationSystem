using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewPrefabTile", menuName = "CustomTiles/PrefabTile", order = 1)]
public class CustomPrefabTile : TileBase
{
    [Tooltip("The sprite to be used for this tile. Can be null if only the prefab is needed.")]
    public Sprite TileSprite;

    [Tooltip("The prefab to be instantiated for this tile. Can be null if only the sprite is needed.")]
    public GameObject TilePrefab;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = TileSprite;
        tileData.gameObject = TilePrefab;
        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = Tile.ColliderType.Sprite;
    }
}
