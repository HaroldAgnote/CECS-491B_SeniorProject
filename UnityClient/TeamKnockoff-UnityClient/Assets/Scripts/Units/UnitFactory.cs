using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class UnitFactory : MonoBehaviour {
        const string RESOURCE_PATH = "Textures/Units/";

        public GameObject landUnitPrefab;
        public GameObject flyingUnitPrefab;

        public Texture2D landUnitTexture;

        public Texture2D thiefUnitTexture;
        public Texture2D archerUnitTexture;
        public Texture2D mageUnitTexture;
        public Texture2D clericUnitTexture;
        public Texture2D knightUnitTexture;
        public Texture2D cavalierUnitTexture;
        public Texture2D pegasusKnightUnitTexture;

        private Dictionary<string, UnitFactoryWrapper> unitMapper; 

        class UnitFactoryWrapper {
            public GameObject UnitPrefab;
            public Texture2D UnitTexture;
            public Sprite[] UnitSprites;
            public string UnitsResourcePath;

            public Func<GameObject, Sprite, Vector3, GameObject> CreateUnitHandler;

            // TODO: Create delegate to load Unit from existing data

            public UnitFactoryWrapper(GameObject unitPrefab, Texture2D unitTexture, Func<GameObject, Sprite, Vector3, GameObject> createGameObjectMethod) {
                UnitPrefab = unitPrefab;
                UnitTexture = unitTexture;
                UnitsResourcePath = $"{RESOURCE_PATH}{UnitTexture.name}";
                UnitSprites = Resources.LoadAll<Sprite> (UnitsResourcePath);
                CreateUnitHandler = createGameObjectMethod;
            }
        }

        public void Start() {

            var landUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, landUnitTexture, SampleUnit.CreateSampleUnit);

            var thiefUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, thiefUnitTexture, Thief.CreateThief);
            var archerUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, archerUnitTexture, Archer.CreateArcher);
            var mageUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, mageUnitTexture, Mage.CreateMage);
            var clericUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, clericUnitTexture, Cleric.CreateCleric);
            var knightUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, knightUnitTexture, Knight.CreateKnight);
            var cavalierUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, cavalierUnitTexture, Cavalier.CreateCavalier);
            var pegasusKnightsUnitsWrapper = new UnitFactoryWrapper(flyingUnitPrefab, pegasusKnightUnitTexture, PegasusKnight.CreatePegasusKnight);

            // Use constant strings rather than hardcoding
            unitMapper = new Dictionary<string, UnitFactoryWrapper>() {
                { "LandUnits", landUnitsWrapper },
                { "Thieves", thiefUnitsWrapper },
                { "Archers", archerUnitsWrapper },
                { "Mages", mageUnitsWrapper },
                { "Clerics", clericUnitsWrapper },
                { "Knights", knightUnitsWrapper },
                { "Cavaliers", cavalierUnitsWrapper },
                { "PegasusKnights", pegasusKnightsUnitsWrapper },
            };

        }

        public GameObject CreateUnit(TileData tileData) {
            var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

            const char DELIMITER = '_';

            var unitData = tileData.UnitData;
            var split_string = unitData.Split(DELIMITER);
            var unitType = split_string.First<string>();
            var spriteIndex = Int32.Parse(split_string.Last<string>());

            var unitFactoryWrapper = unitMapper[unitType];

            var unitPrefab = unitFactoryWrapper.UnitPrefab;
            var unitSprite = unitFactoryWrapper.UnitSprites[spriteIndex];

            var newUnit = unitFactoryWrapper.CreateUnitHandler(unitPrefab, unitSprite, tilePos);

            return newUnit;
        }
    }
}
