using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    public class Knight : ArmoredUnit {
        const int MAX_HEALTH_POINTS = 100;

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
            unit.MaxHealthPoints.Base = Knight.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints.Value;

            unit.Level = Knight.INITIAL_LEVEL;
            unit.ExperiencePoints = Knight.INITIAL_EXPERIENCE_POINTS;

            unit.Strength.Base = Knight.INITIAL_STRENGTH;
            unit.Magic.Base = Knight.INITIAL_MAGIC;

            unit.Defense.Base = Knight.INITIAL_DEFENSE;
            unit.Resistance.Base = Knight.INITIAL_RESISTANCE;

            unit.Speed.Base = Knight.INITIAL_SPEED;
            unit.Skill.Base = Knight.INITIAL_SKILL;

            unit.Luck.Base = Knight.INITIAL_LUCK;
            unit.Movement.Base = Knight.MOVEMENT_RANGE;

            unit.Name = Knight.CLASS_NAME;
            unit.Class = Knight.CLASS_NAME;

            var newWeapon = new Weapon(12, 1, 70, 1, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            unit.EquipWeapon(newWeapon);

            unit.Skills = new List<Skill>() {
                new Bash(),
            };

            return newUnit;
        }

        public Knight() {
        }
    }
}
