using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Thief : InfantryUnit {
        const int MAX_HEALTH_POINTS = 17;

        const int INITIAL_STRENGTH = 5;
        const int INITIAL_MAGIC = 3;

        const int INITIAL_DEFENSE = 2;
        const int INITIAL_RESISTANCE = 3;

        const int INITIAL_SPEED = 6;
        const int INITIAL_SKILL = 6;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 5;

        const string CLASS_NAME = "Thief";
        const string DEFAULT_WEAPON = "Iron Sword";

        #region Growth Rates
        const int GROWTH_HEALTH = 80;
        const int GROWTH_STRENGTH = 45;
        const int GROWTH_MAGIC = 35;
        const int GROWTH_DEFENCE = 35;
        const int GROWTH_RESISTANCE = 35;
        const int GROWTH_SPEED = 60;
        const int GROWTH_SKILL = 60;
        const int GROWTH_LUCK = 80;
        #endregion 

        public override Unit Generate() {
            return new Thief();
        }

        public override Unit Generate(string unitName) {
            return new Thief(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new Thief(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new Thief(unitWrapper);
            return unitClone;
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

            InitGrowthRates();
            InitLevelToSkills();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);

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

            InitGrowthRates();
            InitLevelToSkills();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
            LearnSkill(new MediumSpeedBoost());
        }

        public Thief(UnitWrapper unitWrapper) : base(unitWrapper) {
            InitGrowthRates();
            InitLevelToSkills();
        }

        public override bool CanUse(Weapon weapon) {
            var weaponType = weapon.WeapType;
            switch (weaponType) {
                case WeaponType.Sword:
                    return true;
                case WeaponType.Spear:
                    return false;
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

        public void InitLevelToSkills() {
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
