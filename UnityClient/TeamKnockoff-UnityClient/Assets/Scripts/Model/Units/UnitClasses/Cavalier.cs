using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Cavalier : CavalryUnit {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 7;

        const string CLASS_NAME = "Cavalier";
        private const string DEFAULT_WEAPON = "Iron Spear";

        // TODO: Add growth rate constants

        public static Cavalier CreateCavalier() {
            return new Cavalier();
        }

        public static Cavalier CreateCavalier(string unitName) {
            return new Cavalier(unitName);
        }

        public static Cavalier ImportCavalier(UnitWrapper unitWrapper) {
            return new Cavalier(unitWrapper);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return Cavalier.ImportCavalier(unitWrapper);
        }

        public Cavalier() 
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

        public Cavalier(string unitName) 
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

        public Cavalier(UnitWrapper unitWrapper) : base(unitWrapper) { }

        public override bool CanUse(Weapon weapon) {
            var weaponType = weapon.WeapType;
            switch (weaponType) {
                case WeaponType.Sword:
                    return true;
                case WeaponType.Spear:
                    return true;
                case WeaponType.Axe:
                    return false;
                case WeaponType.Bow:
                    return false;
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
