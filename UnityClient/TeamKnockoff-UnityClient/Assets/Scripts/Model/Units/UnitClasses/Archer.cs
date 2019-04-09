using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

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
        const string DEFAULT_WEAPON = "Iron Bow";

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

        public override Unit Generate(UnitWrapper unitWrapper) {
            return Archer.ImportArcher(unitWrapper);
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

            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);

            LearnSkill(new PiercingShot());
            LearnSkill(new MediumSpeedBoost());
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

            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);

            EquipWeapon(defaultWeapon);
            LearnSkill(new PiercingShot());
            LearnSkill(new MediumSpeedBoost());
        }

        public Archer(UnitWrapper wrapper) : base(wrapper) { }

        public override bool CanUse(Weapon weapon) {
            var weaponType = weapon.WeapType;
            switch (weaponType) {
                case WeaponType.Sword:
                    return false;
                case WeaponType.Spear:
                    return false;
                case WeaponType.Axe:
                    return false;
                case WeaponType.Bow:
                    return true;
                case WeaponType.Book:
                    return false;
                case WeaponType.Staff:
                    return false;
                default:
                    return false;
            }
        }

    }
}
