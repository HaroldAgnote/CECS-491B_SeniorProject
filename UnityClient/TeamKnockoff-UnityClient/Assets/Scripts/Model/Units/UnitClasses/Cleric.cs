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

        public static Cleric CreateCleric() {
            return new Cleric();
        }

        public Cleric(): base()  {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = Cleric.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = Cleric.INITIAL_LEVEL;
            ExperiencePoints = Cleric.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = Cleric.INITIAL_STRENGTH;
            Magic.Base = Cleric.INITIAL_MAGIC;

            Defense.Base = Cleric.INITIAL_DEFENSE;
            Resistance.Base = Cleric.INITIAL_RESISTANCE;

            Speed.Base = Cleric.INITIAL_SPEED;
            Skill.Base = Cleric.INITIAL_SKILL;

            Luck.Base = Cleric.INITIAL_LUCK;
            Movement.Base = Cleric.MOVEMENT_RANGE;

            Name = Cleric.CLASS_NAME;
            Class = Cleric.CLASS_NAME;

            var testWeapon = new Weapon(10, 2, 75, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Magical);

            EquipWeapon(testWeapon);

            LearnSkill(new Heal());
            LearnSkill(new BuffStrength());
            LearnSkill(new Renewal());
        }
    }
}
