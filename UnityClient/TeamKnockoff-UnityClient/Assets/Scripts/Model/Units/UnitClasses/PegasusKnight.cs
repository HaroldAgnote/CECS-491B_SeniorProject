using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    public class PegasusKnight : FlyingUnit {
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

        const string CLASS_NAME = "Pegasus Knight";

        // TODO: Create constants for growth rate

        public static GameObject CreatePegasusKnight(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<PegasusKnight>();

            var unit = newUnit.GetComponent<PegasusKnight>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints = PegasusKnight.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints;

            unit.Level = PegasusKnight.INITIAL_LEVEL;
            unit.ExperiencePoints = PegasusKnight.INITIAL_EXPERIENCE_POINTS;

            unit.Strength = PegasusKnight.INITIAL_STRENGTH;
            unit.Magic = PegasusKnight.INITIAL_MAGIC;

            unit.Defense = PegasusKnight.INITIAL_DEFENSE;
            unit.Resistance = PegasusKnight.INITIAL_RESISTANCE;

            unit.Speed = PegasusKnight.INITIAL_SPEED;
            unit.Skill = PegasusKnight.INITIAL_SKILL;

            unit.Luck = PegasusKnight.INITIAL_LUCK;
            unit.MoveRange = PegasusKnight.MOVEMENT_RANGE;

            unit.Name = PegasusKnight.CLASS_NAME;
            unit.Class = PegasusKnight.CLASS_NAME;

            unit.MainWeapon = new Weapon(5, 1, 25, 0, DamageCalculator.DamageType.Physical);
            unit.Skills = new List<Skill>();

            return newUnit;
        }

        public PegasusKnight() {
        }
    }
}
