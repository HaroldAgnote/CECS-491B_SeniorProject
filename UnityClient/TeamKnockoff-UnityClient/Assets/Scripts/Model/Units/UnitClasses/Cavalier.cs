using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Cavalier : CavalryUnit {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_LEVEL = 5;
        const int INITIAL_EXPERIENCE_POINTS = 50;

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
            unit.MaxHealthPoints.Base = Cavalier.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints.Value;

            unit.Level = Cavalier.INITIAL_LEVEL;
            unit.ExperiencePoints = Cavalier.INITIAL_EXPERIENCE_POINTS;

            unit.Strength.Base = Cavalier.INITIAL_STRENGTH;
            unit.Magic.Base = Cavalier.INITIAL_MAGIC;

            unit.Defense.Base = Cavalier.INITIAL_DEFENSE;
            unit.Resistance.Base = Cavalier.INITIAL_RESISTANCE;

            unit.Speed.Base = Cavalier.INITIAL_SPEED;
            unit.Skill.Base = Cavalier.INITIAL_SKILL;

            unit.Luck.Base = Cavalier.INITIAL_LUCK;
            unit.Movement.Base = Cavalier.MOVEMENT_RANGE;

            unit.Name = Cavalier.CLASS_NAME;
            unit.Class = Cavalier.CLASS_NAME;

            var testWeapon = new Weapon(7, 1, 90, 10, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);

            unit.EquipWeapon(testWeapon);
            return newUnit;
        }

        public Cavalier() {
        }
    }
}
