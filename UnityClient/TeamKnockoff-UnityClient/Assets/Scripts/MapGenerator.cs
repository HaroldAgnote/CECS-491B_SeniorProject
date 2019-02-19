using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public string mapName;
    public Tilemap floor;
    public Tilemap obstacle;
    public Tilemap wall;

    // Start is called before the first frame update
    void Start()
    {
        var floorBounds = floor.cellBounds;
        var floorTiles = floor.GetTilesBlock(floorBounds);

        var obstacleBounds = obstacle.cellBounds;
        var obstacleTiles = obstacle.GetTilesBlock(obstacleBounds);

        var wallBounds = wall.cellBounds;
        var wallTiles = wall.GetTilesBlock(wallBounds);

        Debug.Log($"floorBounds: {floorBounds}, obstacleBounds: {obstacleBounds}, wallBounds: {wallBounds}");
        if (floorBounds == obstacleBounds && obstacleBounds == wallBounds) {
            Debug.Log("Bounds match!");
        } else {
            Debug.Log("Bounds do not match!");
            return;
        }

        string path = $"Assets/Maps/{mapName}.txt";
        var writer = new StreamWriter(path, false);

        var tileData = new List<TileData>();
        int rows = 0;
        int columns = 0;
        int rowOffset = 0;
        for (int x = 0; x < floorBounds.size.x; x++) {
            for (int y = 0 + rowOffset; y < floorBounds.size.y; y++) {
                TileBase floorTile = floorTiles[x + y * floorBounds.size.x];
                TileBase obstacleTile = obstacleTiles[x + y * obstacleBounds.size.x];
                TileBase wallTile = wallTiles[x + y * wallBounds.size.x];

                var newTileData = new TileData() {
                    Column = x,
                    Row = y,
                    FloorData = floorTile ? floorTile.name : "",
                    ObstacleData = obstacleTile ? obstacleTile.name : "",
                    WallData = wallTile ? wallTile.name : "",
                };

                if (floorTile == null && wallTile == null) { 
                    Debug.Log("col:" + x + " row:" + y + " tile: (null)");
                    rowOffset++;
                    y--;
                    continue;
                } else {
                    if (floorTile != null) {
                        Debug.Log("col:" + x + " row:" + y + " floor tile:" + floorTile.name);
                    }
                    if (obstacleTile != null) {
                        Debug.Log("col:" + x + " row:" + y + " obstacle tile:" + obstacleTile.name);
                    }
                    if (wallTile != null) {
                        Debug.Log("col:" + x + " row:" + y + " wall tile:" + wallTile.name);
                    }
                }

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
