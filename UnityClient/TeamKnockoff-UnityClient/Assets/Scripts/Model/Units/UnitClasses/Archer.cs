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
        const int MAX_HEALTH_POINTS = 17;

        const int INITIAL_STRENGTH = 6;
        const int INITIAL_MAGIC = 2;

        const int INITIAL_DEFENSE = 3;
        const int INITIAL_RESISTANCE = 2;

        const int INITIAL_SPEED = 4;
        const int INITIAL_SKILL = 8;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Archer";
        const string DEFAULT_WEAPON = "Iron Bow";

        #region Growth Rates
        const int GROWTH_HEALTH = 80;
        const int GROWTH_STRENGTH = 55;
        const int GROWTH_MAGIC = 30;
        const int GROWTH_DEFENCE = 35;
        const int GROWTH_RESISTANCE = 30;
        const int GROWTH_SPEED = 60;
        const int GROWTH_SKILL = 70;
        const int GROWTH_LUCK = 40;
        #endregion  
        // TODO: Set up constants for growth rate
        public override Unit Generate() {
            return new Archer();
        }

        public override Unit Generate(string unitName) {
            return new Archer(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new Archer(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new Archer(unitWrapper);
            return unitClone;
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

            InitGrowthRates();
            InitLevelToSkills();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
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

            InitGrowthRates();
            InitLevelToSkills();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);

            EquipWeapon(defaultWeapon);
        }

        public Archer(UnitWrapper wrapper) : base(wrapper) {
            InitGrowthRates();
            InitLevelToSkills();
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
                    return true;
                case WeaponType.Book:
                    return false;
                case WeaponType.Staff:
                    return false;
                default:
                    return false;
            }
        }

        public void InitLevelToSkills() {
            mLevelToSkills.Add(2, new List<Skill>() { new PiercingShot(), new MediumSpeedBoost() });
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
