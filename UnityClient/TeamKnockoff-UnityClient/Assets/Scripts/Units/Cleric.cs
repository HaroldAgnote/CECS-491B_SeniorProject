using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class Cleric : InfantryUnit {
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

        const string CLASS_NAME = "Cleric";

        // TODO: Create constants for growth rate

        public Cleric() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = Cleric.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = Cleric.INITIAL_LEVEL;
            ExperiencePoints = Cleric.INITIAL_EXPERIENCE_POINTS;

            Strength = Cleric.INITIAL_STRENGTH;
            Magic = Cleric.INITIAL_MAGIC;

            Defense = Cleric.INITIAL_DEFENSE;
            Resistance = Cleric.INITIAL_RESISTANCE;

            Speed = Cleric.INITIAL_SPEED;
            Skill = Cleric.INITIAL_SKILL;

            Luck = Cleric.INITIAL_LUCK;
            MoveRange = Cleric.MOVEMENT_RANGE;

            Class = Cleric.CLASS_NAME;
        }
    }
}
