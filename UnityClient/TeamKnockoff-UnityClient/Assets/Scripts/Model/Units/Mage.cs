using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Units {
    public class Mage : InfantryUnit {
        const double MAX_HEALTH_POINTS = 100;

        const int INITIAL_LEVEL = 1;
        const int INITIAL_EXPERIENCE_POINTS = 0;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Mage";

        // TODO: Create constants for growth rate

        public static GameObject CreateMage(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Mage>();

            var unit = newUnit.GetComponent<Mage>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = Mage.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = Mage.INITIAL_LEVEL;
            unit.ExperiencePoints = Mage.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = Mage.INITIAL_STRENGTH;
            unit.Magic = Mage.INITIAL_MAGIC;

            unit.Defense = Mage.INITIAL_DEFENSE;
            unit.Resistance = Mage.INITIAL_RESISTANCE;

            unit.Speed = Mage.INITIAL_SPEED;
            unit.Skill = Mage.INITIAL_SKILL;

            unit.Luck = Mage.INITIAL_LUCK;
            unit.MoveRange = Mage.MOVEMENT_RANGE;

            unit.Name = Mage.CLASS_NAME;
            unit.Class = Mage.CLASS_NAME;

            return newUnit;
        }

        public Mage() {
        }
    }
}
