using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Archer : InfantryUnit {
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

        const string CLASS_NAME = "Archer";

        // TODO: Set up constants for growth rate

        public static GameObject CreateArcher(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Archer>();

            var unit = newUnit.GetComponent<Archer>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = Archer.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = Archer.INITIAL_LEVEL;
            unit.ExperiencePoints = Archer.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = Archer.INITIAL_STRENGTH;
            unit.Magic = Archer.INITIAL_MAGIC;

            unit.Defense = Archer.INITIAL_DEFENSE;
            unit.Resistance = Archer.INITIAL_RESISTANCE;

            unit.Speed = Archer.INITIAL_SPEED;
            unit.Skill = Archer.INITIAL_SKILL;

            unit.Luck = Archer.INITIAL_LUCK;
            unit.MoveRange = Archer.MOVEMENT_RANGE;

            unit.Name = Archer.CLASS_NAME;
            unit.Class = Archer.CLASS_NAME;

            unit.MainWeapon = new Weapon(15, 2, 90, 0, DamageCalculator.DamageType.Physical);
            unit.Skills.Add(new PiercingShot());

            return newUnit;
        }

        public Archer() {
        }
    }
}
