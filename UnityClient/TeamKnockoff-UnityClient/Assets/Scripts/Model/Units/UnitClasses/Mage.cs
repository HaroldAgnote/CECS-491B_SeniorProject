using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Mage : InfantryUnit {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_LEVEL = 1;
        const int INITIAL_EXPERIENCE_POINTS = 90;

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

        public static Mage CreateMage() {
            return new Mage();
        }

        public Mage(): base()  {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = Mage.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = Mage.INITIAL_LEVEL;
            ExperiencePoints = Mage.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = Mage.INITIAL_STRENGTH;
            Magic.Base = Mage.INITIAL_MAGIC;

            Defense.Base = Mage.INITIAL_DEFENSE;
            Resistance.Base = Mage.INITIAL_RESISTANCE;

            Speed.Base = Mage.INITIAL_SPEED;
            Skill.Base = Mage.INITIAL_SKILL;

            Luck.Base = Mage.INITIAL_LUCK;
            Movement.Base = Mage.MOVEMENT_RANGE;

            Name = Mage.CLASS_NAME;
            Class = Mage.CLASS_NAME;

            
            var newWeapon = new Weapon(25, 2, 95, 1, Assets.Scripts.Model.DamageCalculator.DamageType.Magical);

            EquipWeapon(newWeapon);

            LearnSkill(new Gravity());
        }
    }
}
