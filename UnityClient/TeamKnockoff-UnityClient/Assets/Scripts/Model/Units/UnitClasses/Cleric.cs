using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Cleric : InfantryUnit {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Cleric";

        // TODO: Create constants for growth rate

        public static Cleric CreateCleric() {
            return new Cleric();
        }

        public static Cleric CreateCleric(string unitName) {
            return new Cleric(unitName);
        }

        public static Cleric ImportCleric(UnitWrapper unitWrapper) {
            return new Cleric(unitWrapper);
        }

        public Cleric() 
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

            var testWeapon = new Weapon(10, 2, 75, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Magical);

            EquipWeapon(testWeapon);

            LearnSkill(new Heal());
            LearnSkill(new BuffStrength());
            LearnSkill(new Renewal());
        }

        public Cleric(string unitName) 
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

            var testWeapon = new Weapon(10, 2, 75, 0, Assets.Scripts.Model.DamageCalculator.DamageType.Magical);

            EquipWeapon(testWeapon);

            LearnSkill(new Heal());
            LearnSkill(new BuffStrength());
            LearnSkill(new Renewal());
        }

        public Cleric(UnitWrapper unitWrapper) : base(unitWrapper) { }
    }
}
