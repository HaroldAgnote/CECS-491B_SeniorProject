using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class UnitFactory : MonoBehaviour {
        const string RESOURCE_PATH = "Textures/Units/";

        public GameObject landUnitPrefab;
        public Texture2D landUnitTexture;

        private Dictionary<string, UnitFactoryWrapper> unitMapper; 

        class UnitFactoryWrapper {
            public GameObject UnitPrefab;
            public Texture2D UnitTexture;
            public Sprite[] UnitSprites;
            public string UnitsResourcePath;

            public UnitFactoryWrapper(GameObject unitPrefab, Texture2D unitTexture) {
                UnitPrefab = unitPrefab;
                UnitTexture = unitTexture;
                UnitsResourcePath = $"{RESOURCE_PATH}{UnitTexture.name}";
                UnitSprites = Resources.LoadAll<Sprite> (UnitsResourcePath);
            }
        }

        public void Start() {
            var landUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, landUnitTexture);

            unitMapper = new Dictionary<string, UnitFactoryWrapper>() {
                { "LandUnits", landUnitsWrapper },
            };

            // TODO: Add more unit wrappers for more types

        }

        public GameObject CreateUnit(TileData tileData) {
            var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

            const char DELIMITER = '_';

            var unitData = tileData.UnitData;
            var split_string = unitData.Split(DELIMITER);
            var unitType = split_string.First<string>();
            var spriteIndex = Int32.Parse(split_string.Last<string>());

            var unitFactoryWrapper = unitMapper[unitType];

            var newUnit = Instantiate(unitFactoryWrapper.UnitPrefab, tilePos, Quaternion.identity) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitFactoryWrapper.UnitSprites[spriteIndex];

            return newUnit;
        }
    }
}
