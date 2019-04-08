using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Knight : ArmoredUnit {
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 2;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Knight";
        const string DEFAULT_WEAPON = "Iron Axe";

        // TODO: Create constants for growth rate

        public static Knight CreateKnight() {
            return new Knight();
        }

        public static Knight CreateKnight(string unitName) {
            return new Knight(unitName);
        }

        public static Knight ImportKnight(UnitWrapper unitWrapper) {
            return new Knight(unitWrapper);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return Knight.ImportKnight(unitWrapper);
        }

        public Knight() 
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

            LearnSkill(new Bash());

        }

        public Knight(string unitName) 
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

            LearnSkill(new Bash());

        }

        public Knight(UnitWrapper unitWrapper) : base(unitWrapper) { }

        public override bool CanUse(Weapon weapon) {
            var weaponType = weapon.WeapType;
            switch (weaponType) {
                case WeaponType.Sword:
                    return true;
                case WeaponType.Spear:
                    return true;
                case WeaponType.Axe:
                    return true;
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
