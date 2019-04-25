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
        const int MAX_HEALTH_POINTS = 22;

        const int INITIAL_STRENGTH = 8;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 6;
        const int INITIAL_RESISTANCE = 3;

        const int INITIAL_SPEED = 2;
        const int INITIAL_SKILL = 3;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Knight";
        const string DEFAULT_WEAPON = "Iron Axe";

        #region Growth Rates
        const int GROWTH_HEALTH = 100;
        const int GROWTH_STRENGTH = 65;
        const int GROWTH_MAGIC = 15;
        const int GROWTH_DEFENCE = 70;
        const int GROWTH_RESISTANCE = 35;
        const int GROWTH_SPEED = 45;
        const int GROWTH_SKILL = 55;
        const int GROWTH_LUCK = 35;
        #endregion 

        public override Unit Generate() {
            return new Knight();
        }

        public override Unit Generate(string unitName) {
            return new Knight(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new Knight(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new Knight(unitWrapper);
            return unitClone;
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

            InitGrowthRates();
            InitLevelToSkills();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
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

            InitGrowthRates();
            InitLevelToSkills();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
        }

        public Knight(UnitWrapper unitWrapper) : base(unitWrapper) {
            InitGrowthRates();
            InitLevelToSkills();
        }

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

        public void InitLevelToSkills() {
            mLevelToSkills.Add(2, new List<Skill>() { new Bash() });
            mLevelToSkills.Add(3, new List<Skill>() { new Fortify() });
        }

        public void InitGrowthRates() {
            mHealthGrowthRate = GROWTH_HEALTH;
            mStrenthGrowthRate = GROWTH_STRENGTH;
            mMagicGrowthRate = GROWTH_MAGIC;
            mDefenseGrowthRate = GROWTH_DEFENCE;
            mResistanceGrowthRate = GROWTH_RESISTANCE;
            mSpeedGrowthRate = GROWTH_SPEED;
            mSkillGrowthRate = GROWTH_SKILL;
            mLuckGrowthRate = GROWTH_LUCK;
        }
    }
}
