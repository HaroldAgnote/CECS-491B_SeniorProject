using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class PegasusKnight : FlyingUnit {
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

        const string CLASS_NAME = "Pegasus Knight";

        // TODO: Create constants for growth rate

        public PegasusKnight() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = PegasusKnight.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = PegasusKnight.INITIAL_LEVEL;
            ExperiencePoints = PegasusKnight.INITIAL_EXPERIENCE_POINTS;

            Strength = PegasusKnight.INITIAL_STRENGTH;
            Magic = PegasusKnight.INITIAL_MAGIC;

            Defense = PegasusKnight.INITIAL_DEFENSE;
            Resistance = PegasusKnight.INITIAL_RESISTANCE;

            Speed = PegasusKnight.INITIAL_SPEED;
            Skill = PegasusKnight.INITIAL_SKILL;

            Luck = PegasusKnight.INITIAL_LUCK;
            MoveRange = PegasusKnight.MOVEMENT_RANGE;

            Class = PegasusKnight.CLASS_NAME;
        }
    }
}
