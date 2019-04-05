using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    [Serializable]
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

        public static PegasusKnight CreatePegasusKnight() {

            return new PegasusKnight();
        }

        public PegasusKnight(): base()  {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = PegasusKnight.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = PegasusKnight.INITIAL_LEVEL;
            ExperiencePoints = PegasusKnight.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = PegasusKnight.INITIAL_STRENGTH;
            Magic.Base = PegasusKnight.INITIAL_MAGIC;

            Defense.Base = PegasusKnight.INITIAL_DEFENSE;
            Resistance.Base = PegasusKnight.INITIAL_RESISTANCE;

            Speed.Base = PegasusKnight.INITIAL_SPEED;
            Skill.Base = PegasusKnight.INITIAL_SKILL;

            Luck.Base = PegasusKnight.INITIAL_LUCK;
            Movement.Base = PegasusKnight.MOVEMENT_RANGE;

            Name = PegasusKnight.CLASS_NAME;
            Class = PegasusKnight.CLASS_NAME;

            var newWeapon = new Weapon(8, 1, 80, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);

            EquipWeapon(newWeapon);
        }
    }
}
