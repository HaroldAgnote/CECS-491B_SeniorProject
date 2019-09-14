using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Assets.Scripts.Model.Tiles;
using Resources = UnityEngine.Resources;

namespace Assets.Scripts.Model.Units {
    public class UnitFactory : MonoBehaviour {
        const string RESOURCE_PATH = "Textures/Units/";
        const char DELIMITER = '_';
        const int UNIT_SPRITE_INDEX = 1;
        const int UNIT_CLASS_INDEX = 2;
        const int UNIT_NAME_INDEX = 3;

        public static UnitFactory instance;

        public GameObject landUnitPrefab;
        public GameObject flyingUnitPrefab;

        public Texture2D campaignUnitTextures;
        public Texture2D genericUnitTextures;
        public Texture2D temporaryUnitTextures;

        private List<Sprite> campaignUnitSprites;
        private List<Sprite> genericUnitSprites;
        private List<Sprite> temporaryUnitSprites;

        private Dictionary<string, Sprite> spriteMapper;
        private Dictionary<string, GameObject> prefabMapper;

        private Dictionary<string, Func<Unit>> unitGenerator;
        private Dictionary<string, Func<string, Unit>> namedUnitGenerator;
        private Dictionary<string, Func<UnitWrapper, Unit>> wrappedUnitGenerator;

        private List<Sprite> UnitSprites {
            get {
                var unitSprites = new List<Sprite>();
                unitSprites.AddRange(campaignUnitSprites);
                unitSprites.AddRange(genericUnitSprites);
                unitSprites.AddRange(temporaryUnitSprites);

                return unitSprites;
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
            prefabMapper = new Dictionary<string, GameObject>() {
                {"Land", landUnitPrefab },
                { "Flying", flyingUnitPrefab }
            };

            unitGenerator = new Dictionary<string, Func<Unit>>();
            namedUnitGenerator = new Dictionary<string, Func<string, Unit>>();
            wrappedUnitGenerator = new Dictionary<string, Func<UnitWrapper, Unit>>();

            foreach (Type type in
                Assembly.GetAssembly(typeof(Unit)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Unit)))) {

                var unit = Activator.CreateInstance(type) as Unit;
                var unitClass = unit.Class.Replace(" ", "");

                unitGenerator.Add(unitClass, unit.Generate);
                namedUnitGenerator.Add(unitClass, unit.Generate);
                wrappedUnitGenerator.Add(unitClass, unit.Generate);
            }

            var campaignUnitSpritesResourcePath = $"{RESOURCE_PATH}{campaignUnitTextures.name}";
            var genericUnitSpritesResourcePath = $"{RESOURCE_PATH}{genericUnitTextures.name}";
            var temporaryUnitSpritesResourcePath = $"{RESOURCE_PATH}{temporaryUnitTextures.name}";

            campaignUnitSprites = UnityEngine.Resources.LoadAll<Sprite>(campaignUnitSpritesResourcePath).ToList();
            genericUnitSprites = UnityEngine.Resources.LoadAll<Sprite>(genericUnitSpritesResourcePath).ToList();
            temporaryUnitSprites = UnityEngine.Resources.LoadAll<Sprite>(temporaryUnitSpritesResourcePath).ToList();

            spriteMapper = new Dictionary<string, Sprite>();

            foreach (var sprite in UnitSprites) {
                var split_string = sprite.name.Split(DELIMITER);
                if (split_string.Length == 4) {
                    var name = split_string[UNIT_NAME_INDEX];
                    spriteMapper.Add(name, sprite);
                }
            }
        }

        public Tuple<Unit, GameObject> CreateUnitForBoard(Vector3 unitPos, Transform parent, string unitData) {
            var split_string = unitData.Split(DELIMITER);
            var spriteSheet = split_string.First();

            switch (spriteSheet) {
                case "CampaignUnits":
                    return CreateCampaignUnitForBoard(unitPos, parent, unitData);
                case "GenericUnits":
                    return CreateGenericUnitForBoard(unitPos, parent, unitData);
                case "TemporaryUnits":
                    return CreateTemporaryUnitForBoard(unitPos, parent, unitData); 
                default:
                    throw new Exception($"No sprite sheet found for {unitData}");
            }
        }

        public Tuple<Unit, GameObject> ImportUnitForBoard(Vector3 unitPos, Transform parent, Unit unit, string unitData) {
            var split_string = unitData.Split(DELIMITER);
            var spriteSheet = split_string.First();

            switch (spriteSheet) {
                case "CampaignUnits":
                    return ImportCampaignUnitForBoard(unitPos, parent, unit, unitData);
                case "GenericUnits":
                    return ImportGenericUnitForBoard(unitPos, parent, unit, unitData);
                case "TemporaryUnits":
                    return ImportTemporaryUnitForBoard(unitPos, parent, unit, unitData);
                default:
                    throw new Exception($"No sprite sheet found for {unitData}");
            }
        }

        private Tuple<Unit, GameObject> CreateCampaignUnitForBoard(Vector3 unitPos, Transform parent, string unitData) {

            // Parse Unit Data
            var split_string = unitData.Split(DELIMITER);

            var unitClass = split_string[UNIT_CLASS_INDEX];
            var unitName = "";

            Unit unit = null;

            if (split_string.Length == 4) {
                unitName = split_string[UNIT_NAME_INDEX];
                unit = GenerateUnit(unitClass, unitName);
            } else {
                unit = GenerateUnit(unitClass);
            }

            var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);
            var sprite = campaignUnitSprites[spriteIndex];

            var unitObject = InstantiateUnit(unitPos, unit.MoveType, sprite, parent);

            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        private Tuple<Unit, GameObject> ImportCampaignUnitForBoard(Vector3 unitPos, Transform parent, Unit unit, string unitData) {

            // Parse Unit Data
            var split_string = unitData.Split(DELIMITER);

            var unitClass = split_string[UNIT_CLASS_INDEX];

            var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);

            var sprite = campaignUnitSprites[spriteIndex];

            var unitObject = InstantiateUnit(unitPos, unit.MoveType, sprite, parent);

            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        private Tuple<Unit, GameObject> CreateGenericUnitForBoard(Vector3 unitPos, Transform parent, string unitData) {

            // Parse Unit Data
            var split_string = unitData.Split(DELIMITER);

            var unitClass = split_string[UNIT_CLASS_INDEX];
            var unitName = "";

            Unit unit = null;

            if (split_string.Length == 4) {
                unitName = split_string[UNIT_NAME_INDEX];
                unit = GenerateUnit(unitClass, unitName);
            } else {
                unit = GenerateUnit(unitClass);
            }

            var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);

            Sprite sprite = null;

            try { 
                sprite = genericUnitSprites[spriteIndex];
            } catch {
                Debug.Log("Generic Sprite not found. Using Temporary Sprite");
                sprite = temporaryUnitSprites.Where(s =>
                    s.name.Split(DELIMITER)[UNIT_CLASS_INDEX] == unitClass
                ).FirstOrDefault();
            }

            var unitObject = InstantiateUnit(unitPos, unit.MoveType, sprite, parent);

            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        private Tuple<Unit, GameObject> ImportGenericUnitForBoard(Vector3 unitPos, Transform parent, Unit unit, string unitData) {

            // Parse Unit Data
            var split_string = unitData.Split(DELIMITER);

            var unitClass = split_string[UNIT_CLASS_INDEX];

            var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);
            Sprite sprite = null;
            try {
                sprite = genericUnitSprites[spriteIndex];
            } catch {
                Debug.Log("Generic Sprite not found. Using Temporary Sprite");
                sprite = temporaryUnitSprites.Where(s =>
                    s.name.Split(DELIMITER)[UNIT_CLASS_INDEX] == unitClass
                ).FirstOrDefault();
            }

            var unitObject = InstantiateUnit(unitPos, unit.MoveType, sprite, parent);

            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        private Tuple<Unit, GameObject> CreateTemporaryUnitForBoard(Vector3 unitPos, Transform parent, string unitData) {

            // Parse Unit Data
            var split_string = unitData.Split(DELIMITER);

            var unitClass = split_string[UNIT_CLASS_INDEX];
            var unitName = "";

            Unit unit = null;

            if (split_string.Length == 4) {
                unitName = split_string[UNIT_NAME_INDEX];
                unit = GenerateUnit(unitClass, unitName);
            } else {
                unit = GenerateUnit(unitClass);
            }

            var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);

            Sprite sprite = null;

            try {
                sprite = temporaryUnitSprites[spriteIndex];
            } catch {
                sprite = temporaryUnitSprites.Where(s =>
                    s.name.Split(DELIMITER)[UNIT_CLASS_INDEX] == unitClass
                ).FirstOrDefault();
            }

            var unitObject = InstantiateUnit(unitPos, unit.MoveType, sprite, parent);

            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        private Tuple<Unit, GameObject> ImportTemporaryUnitForBoard(Vector3 unitPos, Transform parent, Unit unit, string unitData) {

            // Parse Unit Data
            var split_string = unitData.Split(DELIMITER);

            var unitClass = split_string[UNIT_CLASS_INDEX];

            var spriteIndex = Int32.Parse(split_string[UNIT_SPRITE_INDEX]);
            Sprite sprite = null;
            try {
                sprite = temporaryUnitSprites[spriteIndex];
            } catch {
                sprite = temporaryUnitSprites.Where(s =>
                    s.name.Split(DELIMITER)[UNIT_CLASS_INDEX] == unitClass
                ).FirstOrDefault();
            }

            var unitObject = InstantiateUnit(unitPos, unit.MoveType, sprite, parent);

            return new Tuple<Unit, GameObject>(unit, unitObject);
        }

        public Sprite GetUnitSprite(string unitName) {
            try {
                return spriteMapper[unitName];
            } catch {
                Debug.Log("Could not find sprite for unit");
                return temporaryUnitSprites[0];
            }
        }

        public Unit GenerateUnit(string unitClass) {
            try {
                return unitGenerator[unitClass].Invoke();
            } catch {
                throw new Exception($"Could not find Unit Class {unitClass}");

            }
        }

        public Unit GenerateUnit(string unitClass, string unitName) {
            try {
                return namedUnitGenerator[unitClass].Invoke(unitName);
            } catch {
                throw new Exception($"Could not find Unit Class {unitClass}");
            }
        }

        public Unit GenerateUnit(UnitWrapper unitWrapper) {
            var unitClass = unitWrapper.unitClass;
            try {
                return wrappedUnitGenerator[unitClass].Invoke(unitWrapper);
            } catch {
                throw new Exception($"Could not find Unit Class {unitClass}");
            }
        }

        public GameObject InstantiateUnit(Vector3 unitPos, string unitType, Sprite sprite, Transform parent) {
            var unitPrefab = prefabMapper[unitType];

            var newUnitObject = Instantiate(unitPrefab, unitPos, Quaternion.identity, parent) as GameObject;
            newUnitObject.GetComponent<SpriteRenderer>().sprite = sprite;
            return newUnitObject;
        }
    }
}
