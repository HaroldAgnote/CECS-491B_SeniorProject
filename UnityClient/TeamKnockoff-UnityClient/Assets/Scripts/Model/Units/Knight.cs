using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Knight : ArmoredUnit {
        const double MAX_HEALTH_POINTS = 100;

        const int INITIAL_LEVEL = 1;
        const int INITIAL_EXPERIENCE_POINTS = 0;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 75;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Knight";

        // TODO: Create constants for growth rate

        public static GameObject CreateKnight(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Knight>();

            var unit = newUnit.GetComponent<Knight>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = Knight.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = Knight.INITIAL_LEVEL;
            unit.ExperiencePoints = Knight.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = Knight.INITIAL_STRENGTH;
            unit.Magic = Knight.INITIAL_MAGIC;

            unit.Defense = Knight.INITIAL_DEFENSE;
            unit.Resistance = Knight.INITIAL_RESISTANCE;

            unit.Speed = Knight.INITIAL_SPEED;
            unit.Skill = Knight.INITIAL_SKILL;

            unit.Luck = Knight.INITIAL_LUCK;
            unit.MoveRange = Knight.MOVEMENT_RANGE;

            unit.Name = Knight.CLASS_NAME;
            unit.Class = Knight.CLASS_NAME;

            unit.Skills = new List<Skill>();
            unit.Skills.Add(new Bash());

            unit.ExperiencePoints = 50;
            unit.Level = 5;
            return newUnit;
        }

        public Knight() {
        }
    }
}
