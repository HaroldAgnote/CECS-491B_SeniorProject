using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class PegasusKnight : FlyingUnit {
        private const int MAX_HEALTH_POINTS = 100;

        private const int INITIAL_STRENGTH = 1;
        private const int INITIAL_MAGIC = 1;

        private const int INITIAL_DEFENSE = 1;
        private const int INITIAL_RESISTANCE = 1;

        private const int INITIAL_SPEED = 1;
        private const int INITIAL_SKILL = 1;

        private const int INITIAL_LUCK = 1;
        private const int MOVEMENT_RANGE = 7;

        private const string CLASS_NAME = "Pegasus Knight";
        private const string DEFAULT_WEAPON = "Iron Spear";

        // TODO: Create constants for growth rate

        public static PegasusKnight CreatePegasusKnight() {

            return new PegasusKnight();
        }

        public static PegasusKnight CreatePegasusKnight(string unitName) {

            return new PegasusKnight(unitName);
        }

        public static PegasusKnight ImportPegasusKnight(UnitWrapper unitWrapper) {
            return new PegasusKnight(unitWrapper);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return PegasusKnight.ImportPegasusKnight(unitWrapper);
        }

        public PegasusKnight() 
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
        }

        public PegasusKnight(string unitName) 
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
        }

        public PegasusKnight(UnitWrapper unitWrapper) : base(unitWrapper) { }

        public override bool CanUse(Weapon weapon) {
            var weaponType = weapon.WeapType;
            switch (weaponType) {
                case WeaponType.Sword:
                    return false;
                case WeaponType.Spear:
                    return true;
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
