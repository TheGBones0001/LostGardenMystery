using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/CustomTile")]
public class CustomTile : TileBase
{
    public BoxGroup boxGroup;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public bool alwaysCollide;
    public bool neverCollide;

    [HideInInspector] public bool isActive = false;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = isActive ? activeSprite : inactiveSprite;
        tileData.colliderType = !neverCollide && (!isActive || alwaysCollide) ? Tile.ColliderType.Grid : Tile.ColliderType.None;
    }

    public void SetState(bool active)
    {
        isActive = active;
    }
}
