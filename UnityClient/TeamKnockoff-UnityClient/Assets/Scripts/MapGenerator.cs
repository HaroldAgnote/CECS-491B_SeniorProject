using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public string mapName;
    public Tilemap background;
    public Tilemap floor;
    public Tilemap wall;
    public Tilemap obstacle;
    public Tilemap boundary;

    public List<Tilemap> playerTilemaps;

    // Start is called before the first frame update
    void Start()
    {
        var backgroundBounds = background.cellBounds;
        var backgroundTiles = background.GetTilesBlock(backgroundBounds);

        var floorTiles = floor.GetTilesBlock(backgroundBounds);
        var wallTiles = wall.GetTilesBlock(backgroundBounds);
        var obstacleTiles = obstacle.GetTilesBlock(backgroundBounds);
        var boundaryTiles = boundary.GetTilesBlock(backgroundBounds);

        var allPlayerTiles = playerTilemaps.Select(x => x.GetTilesBlock(backgroundBounds)).ToList();

        string path = $"Assets/Resources/Maps/{mapName}.txt";
        var writer = new StreamWriter(path, false);

        var tileData = new List<TileData>();
        int rows = 0;
        int columns = 0;
        int rowOffset = 0;
        for (int x = 0; x < backgroundBounds.size.x; x++) {
            for (int y = 0 + rowOffset; y < backgroundBounds.size.y; y++) {
                TileBase backgroundTile = backgroundTiles[x + y * backgroundBounds.size.x];
                TileBase floorTile = floorTiles[x + y * backgroundBounds.size.x];
                TileBase wallTile = wallTiles[x + y * backgroundBounds.size.x];
                TileBase obstacleTile = obstacleTiles[x + y * backgroundBounds.size.x];
                TileBase boundaryTile = boundaryTiles[x + y * backgroundBounds.size.x];
                TileBase playerTile = null;
                int player = 0;

                var foundTileBase = allPlayerTiles.SingleOrDefault(tileBase => tileBase[x + y * backgroundBounds.size.x] != null);
                if (foundTileBase != null) {
                    playerTile = foundTileBase[x + y * backgroundBounds.size.x];
                    player = allPlayerTiles.IndexOf(foundTileBase) + 1;
                }

                var newTileData = new TileData() {
                    Column = x,
                    Row = y,
                    FloorData = floorTile ? floorTile.name : "",
                    WallData = wallTile ? wallTile.name : "",
                    ObstacleData = obstacleTile ? obstacleTile.name : "",
                    BoundaryData = boundaryTile ? boundaryTile.name : "",

                    Player = player,
                    UnitData = playerTile ? playerTile.name : "",
                };

                tileData.Add(newTileData);
                rows = y + 1;
            }
            columns = x + 1;
        }

        var tileDataWrapper = new TileDataWrapper(columns, rows, tileData);

        string json = JsonUtility.ToJson(tileDataWrapper, true);
        writer.Write(json);

        writer.Close();
    }
}
