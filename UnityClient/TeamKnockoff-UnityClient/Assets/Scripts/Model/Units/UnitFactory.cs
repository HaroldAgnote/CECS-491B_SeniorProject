using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Assets.Scripts.Model.Tiles;

namespace Assets.Scripts.Model.Units {
    public class UnitFactory : MonoBehaviour {
        const string RESOURCE_PATH = "Textures/Units/";

        public static UnitFactory instance;

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
        private Dictionary<string, Sprite> spriteMapper;
        private Dictionary<string, Func<UnitWrapper, Unit>> unitGenerator;

        class UnitFactoryWrapper {
            public GameObject UnitPrefab { get; }
            public Texture2D UnitTexture { get;  }
            public Sprite[] UnitSprites { get;  }
            public string UnitsResourcePath { get;  }

            private readonly Func<Unit> mCreateUnitFunction;
            private readonly Func<string, Unit> mCreateNamedUnitFunction;
            private readonly Func<UnitWrapper, Unit> mImportUnitFunction;

            // TODO: Create delegate to load Unit from existing data

            public UnitFactoryWrapper(GameObject unitPrefab, Texture2D unitTexture, Func<Unit> createUnitFunction, Func<string, Unit> createNamedUnitFunction, Func<UnitWrapper, Unit> importUnitFunction) {
                UnitPrefab = unitPrefab;
                UnitTexture = unitTexture;
                UnitsResourcePath = $"{RESOURCE_PATH}{UnitTexture.name}";
                UnitSprites = Resources.LoadAll<Sprite> (UnitsResourcePath);
                mCreateUnitFunction = createUnitFunction;
                mCreateNamedUnitFunction = createNamedUnitFunction;
                mImportUnitFunction = importUnitFunction;
            }

            public Tuple<Unit, GameObject> CreateUnit(TileData tileData, Transform parent) {
                const char DELIMITER = '_';
                const int UNIT_NAME_INDEX = 2;

                // Parse Unit Data
                var unitData = tileData.UnitData;
                var split_string = unitData.Split(DELIMITER);

                Unit newUnitModel = null;

                if (split_string.Length == 2) {
                    // Nameless Unit
                    newUnitModel = mCreateUnitFunction();
                } else if (split_string.Length == 3) {
                    // Named Unit
                    var unitName = split_string[UNIT_NAME_INDEX];
                    newUnitModel = mCreateNamedUnitFunction(unitName);
                } else {
                    throw new NotImplementedException();
                }

                var newUnitObject = InstantiateUnit(tileData, parent);

                return new Tuple<Unit, GameObject>(newUnitModel, newUnitObject);
            }

            public Tuple<Unit, GameObject> ImportUnit(TileData tileData, Transform parent, UnitWrapper unitWrapper) {
                var importedUnitModel = mImportUnitFunction(unitWrapper);
                var newUnitObject = InstantiateUnit(tileData, parent);

                return new Tuple<Unit, GameObject>(importedUnitModel, newUnitObject);
            }

            public GameObject InstantiateUnit(TileData tileData, Transform parent) {

                var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

                const char DELIMITER = '_';
                const int UNIT_SPRITE_INDEX = 1;

                // Parse Unit Data
                var unitData = tileData.UnitData;
                var split_string = unitData.Split(DELIMITER);
                var unitType = split_string.First<string>();
                var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);

                var unitPrefab = UnitPrefab;
                var unitSprite = UnitSprites[spriteIndex];

                var newUnitObject = Instantiate(UnitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
                newUnitObject.GetComponent<SpriteRenderer>().sprite = UnitSprites[spriteIndex];

                return newUnitObject;
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
            var thiefUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, thiefUnitTexture, Thief.CreateThief, Thief.CreateThief, Thief.ImportThief);
            var archerUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, archerUnitTexture, Archer.CreateArcher, Archer.CreateArcher, Archer.ImportArcher);
            var mageUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, mageUnitTexture, Mage.CreateMage, Mage.CreateMage, Mage.ImportMage);
            var clericUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, clericUnitTexture, Cleric.CreateCleric, Cleric.CreateCleric, Cleric.ImportCleric);
            var knightUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, knightUnitTexture, Knight.CreateKnight, Knight.CreateKnight, Knight.ImportKnight);
            var cavalierUnitsWrapper = new UnitFactoryWrapper(landUnitPrefab, cavalierUnitTexture, Cavalier.CreateCavalier, Cavalier.CreateCavalier, Cavalier.ImportCavalier);
            var pegasusKnightsUnitsWrapper = new UnitFactoryWrapper(flyingUnitPrefab, pegasusKnightUnitTexture, PegasusKnight.CreatePegasusKnight, PegasusKnight.CreatePegasusKnight, PegasusKnight.ImportPegasusKnight);

            // Use constant strings rather than hardcoding
            unitMapper = new Dictionary<string, UnitFactoryWrapper>() {
                { "Thieves", thiefUnitsWrapper },
                { "Archers", archerUnitsWrapper },
                { "Mages", mageUnitsWrapper },
                { "Clerics", clericUnitsWrapper },
                { "Knights", knightUnitsWrapper },
                { "Cavaliers", cavalierUnitsWrapper },
                { "PegasusKnights", pegasusKnightsUnitsWrapper },
            };

            spriteMapper = new Dictionary<string, Sprite>();
            var sprites = new List<Sprite>();

            sprites.AddRange(thiefUnitsWrapper.UnitSprites);
            sprites.AddRange(archerUnitsWrapper.UnitSprites);
            sprites.AddRange(mageUnitsWrapper.UnitSprites);
            sprites.AddRange(clericUnitsWrapper.UnitSprites);
            sprites.AddRange(knightUnitsWrapper.UnitSprites);
            sprites.AddRange(cavalierUnitsWrapper.UnitSprites);
            sprites.AddRange(pegasusKnightsUnitsWrapper.UnitSprites);

            const char DELIMITER = '_';
            const int UNIT_NAME_INDEX = 2;

            foreach (var sprite in sprites) {
                var split_string = sprite.name.Split(DELIMITER);
                if (split_string.Length == 3) {
                    var name = split_string[UNIT_NAME_INDEX];
                    spriteMapper.Add(name, sprite);
                }
            }

            unitGenerator = new Dictionary<string, Func<UnitWrapper, Unit>>();

            List<Unit> units = new List<Unit>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Unit)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Unit)))) {
                units.Add((Unit) Activator.CreateInstance(type));
            }

            foreach (var unit in units) {
                unitGenerator.Add(unit.Class, unit.Generate);
            }
        }

        public Tuple<Unit, GameObject> CreateUnit(TileData tileData, Transform parent) {
            var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

            const char DELIMITER = '_';

            var unitData = tileData.UnitData;
            var split_string = unitData.Split(DELIMITER);

            var unitType = split_string.First<string>();
            var unitFactoryWrapper = unitMapper[unitType];

            return unitFactoryWrapper.CreateUnit(tileData, parent);
        }

        public Tuple<Unit, GameObject> ImportUnit(TileData tileData, Transform parent, UnitWrapper unitWrapper) {
            var tilePos = new Vector3(tileData.Column, tileData.Row, 0f);

            const char DELIMITER = '_';

            var unitData = tileData.UnitData;
            var split_string = unitData.Split(DELIMITER);

            var unitType = split_string.First<string>();
            var unitFactoryWrapper = unitMapper[unitType];

            return unitFactoryWrapper.ImportUnit(tileData, parent, unitWrapper);
        }

        public Tuple<Unit, GameObject> ImportUnit(TileData tileData, Transform parent, Unit unit) {
            const char DELIMITER = '_';

            var unitData = tileData.UnitData;
            var split_string = unitData.Split(DELIMITER);

            var unitType = split_string.First<string>();
            var unitFactoryWrapper = unitMapper[unitType];

            var unitObject = unitFactoryWrapper.InstantiateUnit(tileData, parent);
            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        public Sprite GetUnitSprite(string unitName) {
            return spriteMapper[unitName];
        }

        public Unit GenerateUnit(UnitWrapper unitWrapper) {
            var unitClass = unitWrapper.unitClass;
            return unitGenerator[unitClass].Invoke(unitWrapper);
        }
    }
}
