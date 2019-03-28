using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    public class Mage : InfantryUnit {
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

        const string CLASS_NAME = "Mage";

        // TODO: Create constants for growth rate

        public static GameObject CreateMage(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<Mage>();

            var unit = newUnit.GetComponent<Mage>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints.Base = Mage.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints.Value;

            unit.Level = Mage.INITIAL_LEVEL;
            unit.ExperiencePoints = Mage.INITIAL_EXPERIENCE_POINTS;

            unit.Strength.Base = Mage.INITIAL_STRENGTH;
            unit.Magic.Base = Mage.INITIAL_MAGIC;

            unit.Defense.Base = Mage.INITIAL_DEFENSE;
            unit.Resistance.Base = Mage.INITIAL_RESISTANCE;

            unit.Speed.Base = Mage.INITIAL_SPEED;
            unit.Skill.Base = Mage.INITIAL_SKILL;

            unit.Luck.Base = Mage.INITIAL_LUCK;
            unit.Movement.Base = Mage.MOVEMENT_RANGE;

            unit.Name = Mage.CLASS_NAME;
            unit.Class = Mage.CLASS_NAME;

            
            var newWeapon = new Weapon(25, 2, 95, 1, Assets.Scripts.Model.DamageCalculator.DamageType.Magical);

            unit.EquipWeapon(newWeapon);

            unit.Skills = new List<Skill>() {
                new Gravity()
            };

            return newUnit;
        }

        public Mage() {
        }
    }
}
