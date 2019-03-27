using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    public class PegasusKnight : FlyingUnit {
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
        const int MOVEMENT_RANGE = 7;

        const string CLASS_NAME = "Pegasus Knight";

        // TODO: Create constants for growth rate

        public static GameObject CreatePegasusKnight(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<PegasusKnight>();

            var unit = newUnit.GetComponent<PegasusKnight>();

            // Set Max Health Points and initial stats here
            unit.MaxHealthPoints.Base = PegasusKnight.MAX_HEALTH_POINTS;
            unit.HealthPoints = unit.MaxHealthPoints.Value;

            unit.Level = PegasusKnight.INITIAL_LEVEL;
            unit.ExperiencePoints = PegasusKnight.INITIAL_EXPERIENCE_POINTS;

            unit.Strength.Base = PegasusKnight.INITIAL_STRENGTH;
            unit.Magic.Base = PegasusKnight.INITIAL_MAGIC;

            unit.Defense.Base = PegasusKnight.INITIAL_DEFENSE;
            unit.Resistance.Base = PegasusKnight.INITIAL_RESISTANCE;

            unit.Speed.Base = PegasusKnight.INITIAL_SPEED;
            unit.Skill.Base = PegasusKnight.INITIAL_SKILL;

            unit.Luck.Base = PegasusKnight.INITIAL_LUCK;
            unit.Movement.Base = PegasusKnight.MOVEMENT_RANGE;

            unit.Name = PegasusKnight.CLASS_NAME;
            unit.Class = PegasusKnight.CLASS_NAME;

            var newWeapon = new Weapon(8, 1, 80, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            unit.EquipWeapon(newWeapon);
            unit.Skills = new List<Skill>();

            return newUnit;
        }

        public PegasusKnight() {
        }
    }
}
