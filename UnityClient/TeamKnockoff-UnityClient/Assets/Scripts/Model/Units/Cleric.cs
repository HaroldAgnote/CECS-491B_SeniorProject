using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Cleric : InfantryUnit {
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

        const string CLASS_NAME = "Cleric";

        // TODO: Create constants for growth rate

        public static GameObject CreateCleric(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Cleric>();

            var unit = newUnit.GetComponent<Cleric>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = Cleric.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = Cleric.INITIAL_LEVEL;
            unit.ExperiencePoints = Cleric.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = Cleric.INITIAL_STRENGTH;
            unit.Magic = Cleric.INITIAL_MAGIC;

            unit.Defense = Cleric.INITIAL_DEFENSE;
            unit.Resistance = Cleric.INITIAL_RESISTANCE;

            unit.Speed = Cleric.INITIAL_SPEED;
            unit.Skill = Cleric.INITIAL_SKILL;

            unit.Luck = Cleric.INITIAL_LUCK;
            unit.MoveRange = Cleric.MOVEMENT_RANGE;

            unit.Name = Cleric.CLASS_NAME;
            unit.Class = Cleric.CLASS_NAME;

            unit.Skills = new List<Skill>() {
                new Heal()
            };

            return newUnit;
        }

        public Cleric() {
        }
    }
}
