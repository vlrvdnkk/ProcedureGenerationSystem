using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewRandomTile", menuName = "CustomTiles/RandomTile", order = 1)]
public class RandomPossibleTile : Tile
{
    [Tooltip("Possible sprites that this tile can randomly select.")]
    public Sprite[] PossibleSprites;

    private static readonly System.Random RandomGenerator = new System.Random();

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int seed = Mathf.Abs(location.x) << 16 | Mathf.Abs(location.y);
        Random.InitState(seed);
        tileData.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
        tileData.colliderType = colliderType;
    }
}
