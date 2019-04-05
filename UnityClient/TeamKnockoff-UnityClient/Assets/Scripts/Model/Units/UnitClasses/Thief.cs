using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Thief : InfantryUnit {
        const int MAX_HEALTH_POINTS = 100;

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

        public static Thief CreateThief() {
            return new Thief();
        }

        public static Thief CreateThief(string unitName) {
            return new Thief(unitName);
        }

        public static Thief ImportThief(UnitWrapper unitWrapper) {
            return new Thief(unitWrapper);
        }

        public Thief() 
            : base(CLASS_NAME, CLASS_NAME, 
                  MAX_HEALTH_POINTS, 
                  INITIAL_STRENGTH, 
                  INITIAL_MAGIC, 
                  INITIAL_DEFENSE, 
                  INITIAL_RESISTANCE, 
                  INITIAL_SPEED, 
                  INITIAL_SKILL, 
                  INITIAL_LUCK, 
                  MOVEMENT_RANGE) { 

            var newWeapon = new Weapon(7, 1, 95, 50, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            EquipWeapon(newWeapon);
            LearnSkill(new MediumSpeedBoost());
        }

        public Thief(string unitName) 
            : base(unitName, CLASS_NAME, 
                  MAX_HEALTH_POINTS, 
                  INITIAL_STRENGTH, 
                  INITIAL_MAGIC, 
                  INITIAL_DEFENSE, 
                  INITIAL_RESISTANCE, 
                  INITIAL_SPEED, 
                  INITIAL_SKILL, 
                  INITIAL_LUCK, 
                  MOVEMENT_RANGE) { 

            var newWeapon = new Weapon(7, 1, 95, 50, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
            EquipWeapon(newWeapon);
            LearnSkill(new MediumSpeedBoost());
        }

        public Thief(UnitWrapper unitWrapper) : base(unitWrapper) { }
    }
}
