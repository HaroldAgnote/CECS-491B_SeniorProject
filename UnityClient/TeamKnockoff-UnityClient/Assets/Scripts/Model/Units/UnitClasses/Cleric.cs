using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Weapons;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Cleric : InfantryUnit {
        const int MAX_HEALTH_POINTS = 15;

        const int INITIAL_STRENGTH = 3;
        const int INITIAL_MAGIC = 4;

        const int INITIAL_DEFENSE = 2;
        const int INITIAL_RESISTANCE = 6;

        const int INITIAL_SPEED = 3;
        const int INITIAL_SKILL = 3;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 4;

        const string CLASS_NAME = "Cleric";
        private const string DEFAULT_WEAPON = "Wooden Staff";

        #region Growth Rates
        const int GROWTH_HEALTH = 70;
        const int GROWTH_STRENGTH = 30;
        const int GROWTH_MAGIC = 50;
        const int GROWTH_DEFENCE = 20;
        const int GROWTH_RESISTANCE = 50;
        const int GROWTH_SPEED = 50;
        const int GROWTH_SKILL = 45;
        const int GROWTH_LUCK = 65;
        #endregion 

        public override Unit Generate() {
            return new Cleric();
        }

        public override Unit Generate(string unitName) {
            return new Cleric(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new Cleric(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new Cleric(unitWrapper);
            return unitClone;
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

            InitGrowthRates();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);


            LearnSkill(new Heal());
            LearnSkill(new BuffStrength());
            LearnSkill(new Renewal());
            LearnSkill(new Physic());
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

            InitGrowthRates();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);

            LearnSkill(new Heal());
            LearnSkill(new BuffStrength());
            LearnSkill(new Renewal());
            LearnSkill(new Physic());
        }

        public Cleric(UnitWrapper unitWrapper) : base(unitWrapper) {
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
                    return false;
                case WeaponType.Staff:
                    return true;
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
