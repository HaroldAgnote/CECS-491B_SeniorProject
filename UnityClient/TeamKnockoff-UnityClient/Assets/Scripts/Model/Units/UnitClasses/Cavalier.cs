﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class Cavalier : CavalryUnit {
        const int MAX_HEALTH_POINTS = 21;

        const int INITIAL_STRENGTH = 7;
        const int INITIAL_MAGIC = 2;

        const int INITIAL_DEFENSE = 4;
        const int INITIAL_RESISTANCE = 3;

        const int INITIAL_SPEED = 4;
        const int INITIAL_SKILL = 3;

        const int INITIAL_LUCK = 1;
        const int MOVEMENT_RANGE = 7;

        const string CLASS_NAME = "Cavalier";
        private const string DEFAULT_WEAPON = "Iron Spear";

        #region Growth Rates
        const int GROWTH_HEALTH = 95;
        const int GROWTH_STRENGTH = 65;
        const int GROWTH_MAGIC = 10;
        const int GROWTH_DEFENCE = 60;
        const int GROWTH_RESISTANCE = 15;
        const int GROWTH_SPEED = 50;
        const int GROWTH_SKILL = 55;
        const int GROWTH_LUCK = 50;
        #endregion 

        public override Unit Generate() {
            return new Cavalier();
        }

        public override Unit Generate(string unitName) {
            return new Cavalier(unitName);
        }

        public override Unit Generate(UnitWrapper unitWrapper) {
            return new Cavalier(unitWrapper);
        }

        public override Unit Clone() {
            var unitWrapper = new UnitWrapper(this);
            var unitClone = new Cavalier(unitWrapper);
            return unitClone;
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

            InitGrowthRates();
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

            InitGrowthRates();
            var defaultWeapon = WeaponFactory.instance.GenerateWeapon(DEFAULT_WEAPON);
            EquipWeapon(defaultWeapon);
        }

        public Cavalier(UnitWrapper unitWrapper) : base(unitWrapper) {
            InitGrowthRates();
        }

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
