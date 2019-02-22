using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class Mage : InfantryUnit {
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
        const int MOVEMENT_RANGE = 5;

        const string CLASS_NAME = "Mage";

        // TODO: Create constants for growth rate

        public Mage() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = Mage.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = Mage.INITIAL_LEVEL;
            ExperiencePoints = Mage.INITIAL_EXPERIENCE_POINTS;

            Strength = Mage.INITIAL_STRENGTH;
            Magic = Mage.INITIAL_MAGIC;

            Defense = Mage.INITIAL_DEFENSE;
            Resistance = Mage.INITIAL_RESISTANCE;

            Speed = Mage.INITIAL_SPEED;
            Skill = Mage.INITIAL_SKILL;

            Luck = Mage.INITIAL_LUCK;
            MoveRange = Mage.MOVEMENT_RANGE;

            Class = Mage.CLASS_NAME;
        }
    }
}
