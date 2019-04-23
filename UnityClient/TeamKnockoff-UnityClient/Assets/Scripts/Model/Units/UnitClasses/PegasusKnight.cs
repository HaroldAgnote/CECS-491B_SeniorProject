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

        #region Growth Rates
        const int GROWTH_HEALTH = 75;
        const int GROWTH_STRENGTH = 45;
        const int GROWTH_MAGIC = 25;
        const int GROWTH_DEFENCE = 30;
        const int GROWTH_RESISTANCE = 40;
        const int GROWTH_SPEED = 70;
        const int GROWTH_SKILL = 70;
        const int GROWTH_LUCK = 60;
        #endregion 

        public override Unit Generate() {
            return new PegasusKnight();
        }

        public override Unit Generate(string unitName) {

            return new PegasusKnight(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new PegasusKnight(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new PegasusKnight(unitWrapper);
            return unitClone;
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

            InitGrowthRates();
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

            InitGrowthRates();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
        }

        public PegasusKnight(UnitWrapper unitWrapper) : base(unitWrapper) {
            InitGrowthRates();
        }

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
