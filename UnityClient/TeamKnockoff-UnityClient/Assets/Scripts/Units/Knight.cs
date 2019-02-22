using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class Knight : ArmoredUnit {
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

        const string CLASS_NAME = "Knight";

        // TODO: Create constants for growth rate

        public Knight() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = Knight.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = Knight.INITIAL_LEVEL;
            ExperiencePoints = Knight.INITIAL_EXPERIENCE_POINTS;

            Strength = Knight.INITIAL_STRENGTH;
            Magic = Knight.INITIAL_MAGIC;

            Defense = Knight.INITIAL_DEFENSE;
            Resistance = Knight.INITIAL_RESISTANCE;

            Speed = Knight.INITIAL_SPEED;
            Skill = Knight.INITIAL_SKILL;

            Luck = Knight.INITIAL_LUCK;
            MoveRange = Knight.MOVEMENT_RANGE;
        }
    }
}
