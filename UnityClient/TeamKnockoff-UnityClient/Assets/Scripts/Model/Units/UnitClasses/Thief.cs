using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Thief : InfantryUnit {
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

        public static Thief CreateThief() {
            return new Thief();
        }

        public Thief(): base()  {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = Thief.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = Thief.INITIAL_LEVEL;
            ExperiencePoints = Thief.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = Thief.INITIAL_STRENGTH;
            Magic.Base = Thief.INITIAL_MAGIC;

            Defense.Base = Thief.INITIAL_DEFENSE;
            Resistance.Base = Thief.INITIAL_RESISTANCE;

            Speed.Base = Thief.INITIAL_SPEED;
            Skill.Base = Thief.INITIAL_SKILL;

            Luck.Base = Thief.INITIAL_LUCK;
            Movement.Base = Thief.MOVEMENT_RANGE;

            Name = Thief.CLASS_NAME;
            Class = Thief.CLASS_NAME;

            var newWeapon = new Weapon(7, 1, 95, 50, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            EquipWeapon(newWeapon);
            LearnSkill(new MediumSpeedBoost());
        }
    }
}
