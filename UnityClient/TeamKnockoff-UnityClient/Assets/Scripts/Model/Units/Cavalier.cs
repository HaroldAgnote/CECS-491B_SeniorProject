using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Units {
    public class Cavalier : CavalryUnit {
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
        const int MOVEMENT_RANGE = 7;

        const string CLASS_NAME = "Cavalier";

        // TODO: Add growth rate constants

        public static GameObject CreateCavalier(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Cavalier>();

            var unit = newUnit.GetComponent<Cavalier>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = Cavalier.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = Cavalier.INITIAL_LEVEL;
            unit.ExperiencePoints = Cavalier.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = Cavalier.INITIAL_STRENGTH;
            unit.Magic = Cavalier.INITIAL_MAGIC;

            unit.Defense = Cavalier.INITIAL_DEFENSE;
            unit.Resistance = Cavalier.INITIAL_RESISTANCE;

            unit.Speed = Cavalier.INITIAL_SPEED;
            unit.Skill = Cavalier.INITIAL_SKILL;

            unit.Luck = Cavalier.INITIAL_LUCK;
            unit.MoveRange = Cavalier.MOVEMENT_RANGE;

            unit.Name = Cavalier.CLASS_NAME;
            unit.Class = Cavalier.CLASS_NAME;

            return newUnit;
        }

        public Cavalier() {
        }
    }
}
