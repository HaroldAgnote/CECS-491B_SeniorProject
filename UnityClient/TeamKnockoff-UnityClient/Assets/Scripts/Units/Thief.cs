using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class Thief : InfantryUnit
    {
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

        const string CLASS_NAME = "Thief";

        // TODO: Set up constants for Growth Rate

        public Thief() {
            // Set Max Health Points and initial stats here
            MaxHealthPoints = Thief.MAX_HEALTH_POINTS;
            HealthPoints = MaxHealthPoints;

            Level = Thief.INITIAL_LEVEL;
            ExperiencePoints = Thief.INITIAL_EXPERIENCE_POINTS;

            Strength = Thief.INITIAL_STRENGTH;
            Magic = Thief.INITIAL_MAGIC;

            Defense = Thief.INITIAL_DEFENSE;
            Resistance = Thief.INITIAL_RESISTANCE;

            Speed = Thief.INITIAL_SPEED;
            Skill = Thief.INITIAL_SKILL;

            Luck = Thief.INITIAL_LUCK;
            MoveRange = Thief.MOVEMENT_RANGE;

            Class = Thief.CLASS_NAME;
        }
    }
}
