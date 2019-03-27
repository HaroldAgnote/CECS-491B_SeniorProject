using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    public class Cleric : InfantryUnit {
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
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Cleric";

        // TODO: Create constants for growth rate

        public static GameObject CreateCleric(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Cleric>();

            var unit = newUnit.GetComponent<Cleric>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints.Base = Cleric.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints.Value;

            unit.Level = Cleric.INITIAL_LEVEL;
            unit.ExperiencePoints = Cleric.INITIAL_EXPERIENCE_POINTS;

            unit.Strength.Base = Cleric.INITIAL_STRENGTH;
            unit.Magic.Base = Cleric.INITIAL_MAGIC;

            unit.Defense.Base = Cleric.INITIAL_DEFENSE;
            unit.Resistance.Base = Cleric.INITIAL_RESISTANCE;

            unit.Speed.Base = Cleric.INITIAL_SPEED;
            unit.Skill.Base = Cleric.INITIAL_SKILL;

            unit.Luck.Base = Cleric.INITIAL_LUCK;
            unit.Movement.Base = Cleric.MOVEMENT_RANGE;

            unit.Name = Cleric.CLASS_NAME;
            unit.Class = Cleric.CLASS_NAME;

            var testWeapon = new Weapon(10, 2, 75, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Magical);

            unit.EquipWeapon(testWeapon);

            unit.Skills = new List<Skill>() {
                new Heal(),
                new BuffStrength(),
                new Renewal()
            };

            return newUnit;
        }

        public Cleric() {
        }
    }
}
