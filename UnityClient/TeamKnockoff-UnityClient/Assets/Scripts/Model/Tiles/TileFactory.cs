using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.Model.Tiles {
    public class TileFactory : MonoBehaviour
    {
        const string RESOURCE_PATH = "Textures/Tiles/";
        const char DELIMITER = '_';

        public static TileFactory instance;

        public GameObject floorTilePrefab;
        public GameObject wallTilePrefab;
        public GameObject obstacleTilePrefab;
        public GameObject boundaryTilePrefab;

        public Texture2D swampTexture;
        public Texture2D villageTexture;

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

        void Awake() {
            //Check if instance already exists
            if (instance == null) {
                //if not, set instance to this
                instance = this;
            }

            //If instance already exists and it's not this:
            else if (instance != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void Start() {
            var swampTilesWrapper = new TileFactoryWrapper(swampTexture);
            var villageTileWrapper = new TileFactoryWrapper(villageTexture);

            tileMapper = new Dictionary<string, TileFactoryWrapper>() {
                {"Swamp", swampTilesWrapper },
                {"Village", villageTileWrapper},
                // Add more themes
            };
        }

        public Tile CreateTile(TileData tileData, Transform parent) {
            var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

            Tile newTile = new Tile(tileData.Column, tileData.Row);

            if (!tileData.FloorData.IsEmpty()) {
                SetUpTile(floorTilePrefab, tilePos, newTile, tileData.FloorData, parent);
            }

            if (!tileData.WallData.IsEmpty()) {
                SetUpTile(wallTilePrefab, tilePos, newTile, tileData.WallData, parent);
                
            }

            if (!tileData.ObstacleData.IsEmpty()) {
                SetUpTile(obstacleTilePrefab, tilePos, newTile, tileData.ObstacleData, parent);
            }

            if (!tileData.BoundaryData.IsEmpty()) {
                SetUpTile(boundaryTilePrefab, tilePos, newTile, tileData.BoundaryData, parent);
            }
                

            return newTile;
        }

        public void SetUpTile(GameObject tilePrefab, Vector3 tilePos, Tile newTile, string tileStringData, Transform parent) {
            GameObject instance = Instantiate(tilePrefab, tilePos, Quaternion.identity, parent) as GameObject;

            // Debug.Log($"Instantiating tile: {tileStringData}");
            var tileSprite = instance.GetComponent<SpriteRenderer>();

            var theme = GetTheme(tileStringData);
            // Debug.Log($"Theme: {theme}");
            var tileFactory = tileMapper[theme];

            var tileType = GetTileType(tileStringData);
            // Debug.Log($"Tile Type: {tileType}");
            var tileEffect = GetTileEffect(tileStringData);
            // Debug.Log($"Tile Effect: {tileEffect}");

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
            const int TILE_TYPE_INDEX = 2;
            const string NORMAL_TILE_TYPE = "Normal";
            var split_string = tileStringData.Split(DELIMITER);

            if (split_string.Length == 2) {
                // Example - Swamp_0
                return NORMAL_TILE_TYPE;
            } else if (split_string.Length > 2) {
                // Example - Swamp_0_Normal
                // Example - Swamp_0_Shallow_Damage
                string tileType = split_string[TILE_TYPE_INDEX];
                return tileType;
            } else {
                throw new Exception($"Bad tile data - {tileStringData}");
            }

        }

        public string GetTileEffect(string tileStringData) {
            const int TILE_EFFECT_INDEX = 3;
            const string NORMAL_TILE_EFFECT = "Normal";

            var split_string = tileStringData.Split(DELIMITER);

            if (split_string.Length == 2 || split_string.Length == 3) {
                // Example - Swamp_0
                // Example - Swamp_0_Tree
                return NORMAL_TILE_EFFECT;
            } else if (split_string.Length == 4) {
                // Example - Swamp_0_Normal_Fortify
                string tileEffect = split_string[TILE_EFFECT_INDEX];
                return tileEffect;
            } else {
                throw new Exception($"Bad tile data - {tileStringData}");
            }
        }

        public int GetTileSpriteIndex(string tileStringData) {
            const int TILE_SPRITE_INDEX = 1;
            var split_string = tileStringData.Split(DELIMITER);
            var index = Int32.Parse(split_string[TILE_SPRITE_INDEX]);

            return index;
        }
    }
}
