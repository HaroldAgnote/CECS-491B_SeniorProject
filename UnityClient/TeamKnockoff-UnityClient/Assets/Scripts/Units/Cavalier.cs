using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class Cavalier : CavalryUnit {
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

        const string CLASS_NAME = "Cavalier";

        // TODO: Add growth rate constants

        public Cavalier() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = Cavalier.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = Cavalier.INITIAL_LEVEL;
            ExperiencePoints = Cavalier.INITIAL_EXPERIENCE_POINTS;

            Strength = Cavalier.INITIAL_STRENGTH;
            Magic = Cavalier.INITIAL_MAGIC;

            Defense = Cavalier.INITIAL_DEFENSE;
            Resistance = Cavalier.INITIAL_RESISTANCE;

            Speed = Cavalier.INITIAL_SPEED;
            Skill = Cavalier.INITIAL_SKILL;

            Luck = Cavalier.INITIAL_LUCK;
            MoveRange = Cavalier.MOVEMENT_RANGE;

            Class = Cavalier.CLASS_NAME;
        }
    }
}
