using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Archer : InfantryUnit {
        const int MAX_HEALTH_POINTS = 100;

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

        public static Archer CreateArcher() {
            return new Archer();
        }

        public static Archer CreateArcher(string unitName) {
            return new Archer(unitName);
        }

        public static Archer ImportArcher(UnitWrapper unitWrapper) {
            return new Archer(unitWrapper);
        }

        public Archer() 
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

            var testWeapon = new Weapon(8, 2, 90, 0, DamageType.Physical);
            EquipWeapon(testWeapon);

            LearnSkill(new PiercingShot());
        }

        public Archer(string unitName) 
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

            var testWeapon = new Weapon(8, 2, 90, 0, DamageType.Physical);
            EquipWeapon(testWeapon);

            LearnSkill(new PiercingShot());
        }

        public Archer(UnitWrapper wrapper) : base(wrapper) { }

    }
}
