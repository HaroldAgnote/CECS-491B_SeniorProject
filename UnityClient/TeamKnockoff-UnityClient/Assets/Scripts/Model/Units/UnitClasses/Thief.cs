using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Thief : InfantryUnit
    {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_LEVEL = 1;
        const int INITIAL_EXPERIENCE_POINTS = 0;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 5;

        const string CLASS_NAME = "Thief";

        // TODO: Set up constants for Growth Rate

        public static GameObject CreateThief(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Thief>();

            var unit = newUnit.GetComponent<Thief>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints.Base = Thief.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints.Value;

            unit.Level = Thief.INITIAL_LEVEL;
            unit.ExperiencePoints = Thief.INITIAL_EXPERIENCE_POINTS;

            unit.Strength.Base = Thief.INITIAL_STRENGTH;
            unit.Magic.Base = Thief.INITIAL_MAGIC;

            unit.Defense.Base = Thief.INITIAL_DEFENSE;
            unit.Resistance.Base = Thief.INITIAL_RESISTANCE;

            unit.Speed.Base = Thief.INITIAL_SPEED;
            unit.Skill.Base = Thief.INITIAL_SKILL;

            unit.Luck.Base = Thief.INITIAL_LUCK;
            unit.Movement.Base = Thief.MOVEMENT_RANGE;

            unit.Name = Thief.CLASS_NAME;
            unit.Class = Thief.CLASS_NAME;

            var newWeapon = new Weapon(5, 1, 100, 50, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            unit.EquipWeapon(newWeapon);
            unit.Skills = new List<Skill>() {
                new MediumSpeedBoost()
            };

            return newUnit;
        }

        public Thief() {
        }

        public Thief(string name) {
            Name = name;
        }
    }
}
