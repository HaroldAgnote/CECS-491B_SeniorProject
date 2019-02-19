using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    const string RESOURCE_PATH = "Textures/";

    public GameObject normalTilePrefab;
    public GameObject obstacleTilePrefab;
    public GameObject wallTilePrefab;

    public Texture2D swampTexture;
    private Sprite[] swampSprites;
    private string swampResourcePath;

    private Sprite[] sprites;

    public void Start() {
        swampResourcePath = $"{RESOURCE_PATH}{swampTexture.name}";
        swampSprites = Resources.LoadAll<Sprite>(swampResourcePath);
    }

    public Tile CreateTile(TileData tileData, Transform parent) {
        var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

        Tile newTile = new Tile(tileData.Column, tileData.Row);

        if (tileData.FloorData.Length > 0) {
            GameObject instance = Instantiate(normalTilePrefab, tilePos, Quaternion.identity, parent) as GameObject;

            sprites = GetSprites(tileData.FloorData);
            var index = GetTileSpriteIndex(tileData.FloorData);

            instance.GetComponent<SpriteRenderer>().sprite = sprites[index];
        }

        if (tileData.ObstacleData.Length > 0) {
            newTile.TileType = Tile.BoardTileType.Obstacle;
            GameObject instance = Instantiate(obstacleTilePrefab, tilePos, Quaternion.identity, parent) as GameObject;

            sprites = GetSprites(tileData.ObstacleData);
            var index = GetTileSpriteIndex(tileData.ObstacleData);

            instance.GetComponent<SpriteRenderer>().sprite = sprites[index];
        }

        if (tileData.WallData.Length > 0) {
            newTile.TileType = Tile.BoardTileType.Boundary;
            GameObject instance = Instantiate(wallTilePrefab, tilePos, Quaternion.identity, parent) as GameObject;

            sprites = GetSprites(tileData.WallData);
            var index = GetTileSpriteIndex(tileData.WallData);

            instance.GetComponent<SpriteRenderer>().sprite = sprites[index];
        }

        return newTile;
    }

    public Sprite[] GetSprites(string tileStringData) {
        const char DELIMITER = '_';

        var split_string = tileStringData.Split(DELIMITER);
        var theme = split_string.First<string>();

        switch (theme) {
            case "Swamp":
                return swampSprites;
            default:
                return null;
        }
    }

    public int GetTileSpriteIndex(string tileStringData) {
        const char DELIMITER = '_';

        var split_string = tileStringData.Split(DELIMITER);
        var index = Int32.Parse(split_string.Last<string>());

        return index;
    }
}
