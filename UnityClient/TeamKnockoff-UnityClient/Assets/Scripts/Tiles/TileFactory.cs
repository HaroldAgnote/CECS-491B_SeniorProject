using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.ExtensionMethods;

public class TileFactory : MonoBehaviour
{
    const string RESOURCE_PATH = "Textures/Tiles/";
    const char DELIMITER = '_';

    public GameObject normalTilePrefab;
    public GameObject obstacleTilePrefab;
    public GameObject wallTilePrefab;

    public Texture2D swampTexture;

    private Dictionary<string, TileFactoryWrapper> tileMapper;

    class TileFactoryWrapper {
        public Texture2D SpriteTexture;
        public Sprite[] TileSprites;
        public string SpriteResourcePath;

        public TileFactoryWrapper(Texture2D spriteTexture) {
            SpriteTexture = spriteTexture;
            SpriteResourcePath = $"{RESOURCE_PATH}{SpriteTexture.name}";
            TileSprites = Resources.LoadAll<Sprite>(SpriteResourcePath);
        }
    }

    public void Start() {
        var swampTilesWrapper = new TileFactoryWrapper(swampTexture);

        tileMapper = new Dictionary<string, TileFactoryWrapper>() {
            {"Swamp", swampTilesWrapper },
            // Add more themes
        };
    }

    public Tile CreateTile(TileData tileData, Transform parent) {
        var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

        Tile newTile = new Tile(tileData.Column, tileData.Row);

        if (!tileData.FloorData.IsEmpty()) {
            SetUpTile(normalTilePrefab, tilePos, newTile, tileData.FloorData, parent);
        }

        if (!tileData.ObstacleData.IsEmpty()) {
            SetUpTile(obstacleTilePrefab, tilePos, newTile, tileData.ObstacleData, parent);
        }

        if (!tileData.WallData.IsEmpty()) {
            SetUpTile(wallTilePrefab, tilePos, newTile, tileData.WallData, parent);
            
        }

        return newTile;
    }

    public void SetUpTile(GameObject tilePrefab, Vector3 tilePos, Tile newTile, string tileStringData, Transform parent) {
        GameObject instance = Instantiate(tilePrefab, tilePos, Quaternion.identity, parent) as GameObject;

        Debug.Log($"Instantiating tile: {tileStringData}");
        var tileSprite = instance.GetComponent<SpriteRenderer>();

        var theme = GetTheme(tileStringData);
        var tileFactory = tileMapper[theme];

        var tileType = GetTileType(tileStringData);
        var tileEffect = GetTileEffect(tileStringData);

        newTile.TileType = Tile.TILE_TYPES[tileType];
        newTile.TileEffect = Tile.TILE_EFFECTS[tileEffect];

        var index = GetTileSpriteIndex(tileStringData);

        tileSprite.sprite = tileFactory.TileSprites.Where(s => s.name == tileStringData).SingleOrDefault();
    }

    public string GetTheme(string tileStringData) {
        var split_string = tileStringData.Split(DELIMITER);
        var theme = split_string.First<string>();

        return theme;
    }

    public string GetTileType(string tileStringData) {
        const int TILE_TYPE_INDEX = 1;
        const string NORMAL_TILE_TYPE = "Normal";
        var split_string = tileStringData.Split(DELIMITER);

        if (split_string.Length == 2) {
            // Example - Swamp_0
            return NORMAL_TILE_TYPE;
        } else if (split_string.Length > 2) {
            // Example - Swamp_Normal_0
            // Example - Swamp_Shallow_Damage_0
            string tileType = split_string[TILE_TYPE_INDEX];
            return tileType;
        } else {
            throw new Exception($"Bad tile data - {tileStringData}");
        }

    }

    public string GetTileEffect(string tileStringData) {
        const int TILE_EFFECT_INDEX = 2;
        const string NORMAL_TILE_EFFECT = "Normal";

        var split_string = tileStringData.Split(DELIMITER);

        if (split_string.Length == 2 || split_string.Length == 3) {
            // Example - Swamp_Tree_0
            return NORMAL_TILE_EFFECT;
        } else if (split_string.Length == 4) {
            // Example - Swamp_Normal_Fortify_0
            string tileEffect = split_string[TILE_EFFECT_INDEX];
            return tileEffect;
        } else {
            throw new Exception($"Bad tile data - {tileStringData}");
        }
    }

    public int GetTileSpriteIndex(string tileStringData) {
        var split_string = tileStringData.Split(DELIMITER);
        var index = Int32.Parse(split_string.Last<string>());

        return index;
    }
}
