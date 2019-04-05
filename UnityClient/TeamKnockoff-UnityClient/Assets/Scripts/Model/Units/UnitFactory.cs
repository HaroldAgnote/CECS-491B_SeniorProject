using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Tiles;

namespace Assets.Scripts.Model.Units {
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
            public GameObject UnitPrefab { get; }
            public Texture2D UnitTexture { get;  }
            public Sprite[] UnitSprites { get;  }
            public string UnitsResourcePath { get;  }

            private Func<Unit> mGenerateUnitFunction;

            // TODO: Create delegate to load Unit from existing data

            public UnitFactoryWrapper(GameObject unitPrefab, Texture2D unitTexture, Func<Unit> generateUnitFunction) {
                UnitPrefab = unitPrefab;
                UnitTexture = unitTexture;
                UnitsResourcePath = $"{RESOURCE_PATH}{UnitTexture.name}";
                UnitSprites = Resources.LoadAll<Sprite> (UnitsResourcePath);
                mGenerateUnitFunction = generateUnitFunction;
            }

            public Tuple<Unit, GameObject> InstantiateUnit(TileData tileData, Transform parent) {
                var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

                const char DELIMITER = '_';

                // Parse Unit Data
                var unitData = tileData.UnitData;
                var split_string = unitData.Split(DELIMITER);
                var unitType = split_string.First<string>();
                var spriteIndex = Int32.Parse(split_string.Last<string>());

                var unitPrefab = UnitPrefab;
                var unitSprite = UnitSprites[spriteIndex];

                var newUnitObject = Instantiate(UnitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
                newUnitObject.GetComponent<SpriteRenderer>().sprite = UnitSprites[spriteIndex];

                var newUnit = mGenerateUnitFunction();

                return new Tuple<Unit, GameObject>(newUnit, newUnitObject);
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

        public Tuple<Unit, GameObject> CreateUnit(TileData tileData, Transform parent) {
            var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

            const char DELIMITER = '_';

            var unitData = tileData.UnitData;
            var split_string = unitData.Split(DELIMITER);

            var unitType = split_string.First<string>();
            var unitFactoryWrapper = unitMapper[unitType];

            return unitFactoryWrapper.InstantiateUnit(tileData, parent);
        }
    }
}
