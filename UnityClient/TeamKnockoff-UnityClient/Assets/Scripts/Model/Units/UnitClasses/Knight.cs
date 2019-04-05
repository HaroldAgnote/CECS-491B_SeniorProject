using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Knight : ArmoredUnit {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_LEVEL = 17;
        const int INITIAL_EXPERIENCE_POINTS = 90;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 2;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Knight";

        // TODO: Create constants for growth rate

        public static Knight CreateKnight() {
            return new Knight();
        }

        public Knight(): base()  {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = Knight.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = Knight.INITIAL_LEVEL;
            ExperiencePoints = Knight.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = Knight.INITIAL_STRENGTH;
            Magic.Base = Knight.INITIAL_MAGIC;

            Defense.Base = Knight.INITIAL_DEFENSE;
            Resistance.Base = Knight.INITIAL_RESISTANCE;

            Speed.Base = Knight.INITIAL_SPEED;
            Skill.Base = Knight.INITIAL_SKILL;

            Luck.Base = Knight.INITIAL_LUCK;
            Movement.Base = Knight.MOVEMENT_RANGE;

            Name = Knight.CLASS_NAME;
            Class = Knight.CLASS_NAME;

            ExperiencePoints = INITIAL_EXPERIENCE_POINTS;
            Level = INITIAL_LEVEL;

            var newWeapon = new Weapon(12, 1, 70, 1, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            EquipWeapon(newWeapon);

            LearnSkill(new Bash());

        }
    }
}
