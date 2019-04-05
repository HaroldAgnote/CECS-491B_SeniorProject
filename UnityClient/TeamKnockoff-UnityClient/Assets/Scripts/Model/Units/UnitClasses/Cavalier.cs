using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    [Serializable]
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

        public static Cavalier CreateCavalier() {
            return new Cavalier();
        }

        public Cavalier(): base()  {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = Cavalier.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = Cavalier.INITIAL_LEVEL;
            ExperiencePoints = Cavalier.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = Cavalier.INITIAL_STRENGTH;
            Magic.Base = Cavalier.INITIAL_MAGIC;

            Defense.Base = Cavalier.INITIAL_DEFENSE;
            Resistance.Base = Cavalier.INITIAL_RESISTANCE;

            Speed.Base = Cavalier.INITIAL_SPEED;
            Skill.Base = Cavalier.INITIAL_SKILL;

            Luck.Base = Cavalier.INITIAL_LUCK;
            Movement.Base = Cavalier.MOVEMENT_RANGE;

            Name = Cavalier.CLASS_NAME;
            Class = Cavalier.CLASS_NAME;

            var testWeapon = new Weapon(7, 1, 90, 10, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);

            EquipWeapon(testWeapon);
        }
    }
}
