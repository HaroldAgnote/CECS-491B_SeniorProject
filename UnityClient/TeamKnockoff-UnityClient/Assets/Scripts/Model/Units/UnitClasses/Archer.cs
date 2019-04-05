using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    public class Archer : InfantryUnit {
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

        const string CLASS_NAME = "Archer";

        // TODO: Set up constants for growth rate

        public static Unit CreateArcher() {
            return new Archer();
        }

        public Archer() : base() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints.Base = Archer.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints.Value;

            Level = Archer.INITIAL_LEVEL;
            ExperiencePoints = Archer.INITIAL_EXPERIENCE_POINTS;

            Strength.Base = Archer.INITIAL_STRENGTH;
            Magic.Base = Archer.INITIAL_MAGIC;

            Defense.Base = Archer.INITIAL_DEFENSE;
            Resistance.Base = Archer.INITIAL_RESISTANCE;

            Speed.Base = Archer.INITIAL_SPEED;
            Skill.Base = Archer.INITIAL_SKILL;

            Luck.Base = Archer.INITIAL_LUCK;
            Movement.Base = Archer.MOVEMENT_RANGE;

            Name = Archer.CLASS_NAME;
            Class = Archer.CLASS_NAME;

            var testWeapon = new Weapon(8, 2, 90, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            EquipWeapon(testWeapon);

            LearnSkill(new PiercingShot());
        }
    }
}
