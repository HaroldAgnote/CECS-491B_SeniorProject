using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class Archer : InfantryUnit {
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

        const string CLASS_NAME = "Archer";

        // TODO: Set up constants for growth rate

        public Archer() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = Archer.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = Archer.INITIAL_LEVEL;
            ExperiencePoints = Archer.INITIAL_EXPERIENCE_POINTS;

            Strength = Archer.INITIAL_STRENGTH;
            Magic = Archer.INITIAL_MAGIC;

            Defense = Archer.INITIAL_DEFENSE;
            Resistance = Archer.INITIAL_RESISTANCE;

            Speed = Archer.INITIAL_SPEED;
            Skill = Archer.INITIAL_SKILL;

            Luck = Archer.INITIAL_LUCK;
            MoveRange = Archer.MOVEMENT_RANGE;

            Class = Archer.CLASS_NAME;
        }
    }
}
