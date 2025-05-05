using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    public Tilemap tilemap;

    private readonly Dictionary<BoxGroup, List<Vector3Int>> groupPositions = new();
    private readonly Dictionary<Vector3Int, CustomTile> customTiles = new();

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CacheTrapTiles();
        DeactivateAllGroups();
    }

    private void CacheTrapTiles()
    {
        groupPositions.Clear();
        customTiles.Clear();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile is CustomTile customTile)
            {
                customTile.SetState(true);
                customTiles[pos] = customTile;

                if (!groupPositions.ContainsKey(customTile.boxGroup))
                    groupPositions[customTile.boxGroup] = new List<Vector3Int>();

                groupPositions[customTile.boxGroup].Add(pos);
            }
        }
    }

    private void SetGroupState(BoxGroup groupName, bool active)
    {
        if (tilemap == null)
            return;

        if (!groupPositions.ContainsKey(groupName))
        {
            return;
        }

        foreach (var pos in groupPositions[groupName])
        {
            customTiles[pos].SetState(active);
            tilemap.RefreshTile(pos);
        }
    }

    private void SetTileStateAt(Vector3 position, bool active)
    {
        if (tilemap == null)
            return;

        var tileMapPosition = tilemap.WorldToCell(position);

        if (customTiles.ContainsKey(tileMapPosition))
        {
            customTiles[tileMapPosition].SetState(active);
            tilemap.RefreshTile(tileMapPosition);
        }
    }

    public void DeactivateAllGroups()
    {
        SetGroupState(BoxGroup.Trap, false);
        SetGroupState(BoxGroup.Bridge, false);
        SetGroupState(BoxGroup.Finish, false);
    }

    public void ActivateAllGroups()
    {
        SetGroupState(BoxGroup.Trap, true);
        SetGroupState(BoxGroup.Bridge, true);
        SetGroupState(BoxGroup.Finish, true);
    }

    public void DeactivateGroup(BoxGroup groupName) => SetGroupState(groupName, false);
    public void ActivateGroup(BoxGroup groupName) => SetGroupState(groupName, true);

    public void DeactivateTileAt(Vector3 position) => SetTileStateAt(position, false);
    public void ActivateTileAt(Vector3 position) => SetTileStateAt(position, true);
}
