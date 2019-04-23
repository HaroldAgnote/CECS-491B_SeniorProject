using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Mage : InfantryUnit {
        #region const Stats
        const int MAX_HEALTH_POINTS = 100;

        const int INITIAL_STRENGTH = 1;
        const int INITIAL_MAGIC = 1;

        const int INITIAL_DEFENSE = 1;
        const int INITIAL_RESISTANCE = 1;

        const int INITIAL_SPEED = 1;
        const int INITIAL_SKILL = 1;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Mage";
        private const string DEFAULT_WEAPON = "Dark Book";
        #endregion
        #region Growth Rates
        const int GROWTH_HEALTH = 85;
        const int GROWTH_STRENGTH = 20;
        const int GROWTH_MAGIC = 55;
        const int GROWTH_DEFENCE = 35;
        const int GROWTH_RESISTANCE = 35;
        const int GROWTH_SPEED = 50;
        const int GROWTH_SKILL = 50;
        const int GROWTH_LUCK = 65;
        #endregion  

        public override Unit Generate() {
            return new Mage();
        }

        public override Unit Generate(string unitName) {
            return new Mage(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new Mage(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new Mage(unitWrapper);
            return unitClone;
        }

        public Mage() 
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
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
        }

        public Mage(string unitName) 
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
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
        }

        public Mage(UnitWrapper unitWrapper) : base (unitWrapper) {
            InitGrowthRates();
        }

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
                    return false;
                case WeaponType.Book:
                    return true;
                case WeaponType.Staff:
                    return false;
                default:
                    return false;
            }
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
