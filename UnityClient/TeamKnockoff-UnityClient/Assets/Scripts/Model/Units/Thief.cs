using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Thief : InfantryUnit
    {
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
        const int MOVEMENT_RANGE = 5;

        const string CLASS_NAME = "Thief";

        // TODO: Set up constants for Growth Rate

        public static GameObject CreateThief(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Thief>();

            var unit = newUnit.GetComponent<Thief>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = Thief.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = Thief.INITIAL_LEVEL;
            unit.ExperiencePoints = Thief.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = Thief.INITIAL_STRENGTH;
            unit.Magic = Thief.INITIAL_MAGIC;

            unit.Defense = Thief.INITIAL_DEFENSE;
            unit.Resistance = Thief.INITIAL_RESISTANCE;

            unit.Speed = Thief.INITIAL_SPEED;
            unit.Skill = Thief.INITIAL_SKILL;

            unit.Luck = Thief.INITIAL_LUCK;
            unit.MoveRange = Thief.MOVEMENT_RANGE;

            unit.Name = Thief.CLASS_NAME;
            unit.Class = Thief.CLASS_NAME;

            unit.MainWeapon = new Weapon(5, 1, 25, 0, DamageCalculator.DamageType.Physical);
            unit.Skills = new List<Skill>();

            return newUnit;
        }

        public Thief() {
        }

        public Thief(string name) {
            Name = name;
        }
    }
}
