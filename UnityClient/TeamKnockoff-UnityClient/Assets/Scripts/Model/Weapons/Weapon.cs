using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Weapons {

    [Serializable]
    public class Weapon {

        public enum WeaponType {
            Sword,
            Spear,
            Axe,
            Bow,
            Book,
            Staff
        }

        [SerializeField]
        private string mName;

        [SerializeField]
        private int mMight;

        [SerializeField]
        private int mRange;

        [SerializeField]
        private int mWeight;

        [SerializeField]
        private int mHitRate;

        [SerializeField]
        private int mCritRate;

        [SerializeField]
        private int mRarity;

        [SerializeField]
        private int mBuyingPrice;

        [SerializeField]
        private int mSellingPrice;

        [SerializeField]
        private WeaponType mWeaponType;

        [SerializeField]
        private DamageType mDamageType;

        [SerializeField]
        private HashSet<Skill> mSkills;

        public string Name {
            get {
                return mName;
            }
        }

        public int Might {
            get {
                return mMight;
            }
        }

        public int Range {
            get {
                return mRange;
            }
        }

        public int Weight {
            get {
                return mWeight;
            }
        }

        public int HitRate {
            get {
                return mHitRate;
            }
        }

        public int CritRate {
            get {
                return mCritRate;
            }
        }

        public int Rarity {
            get {
                return mRarity;
            }
        }

        public int BuyingPrice {
            get {
                return mBuyingPrice;
            }
        }

        public int SellingPrice {
            get {
                return mSellingPrice;
            }
        }

        public WeaponType WeapType {
            get {
                return mWeaponType;
            }
        }

        public DamageType DamageType {
            get {
                return mDamageType;
            }
        }

        public HashSet<Skill> Skills {
            get {
                return mSkills;
            }
        }

        public Weapon() {
            // TODO: This is a default weapon
            mRange = 1;
        }

        public Weapon(string name,
                        int range,
                        int weight,
                        int hitRate,
                        int critRate,
                        int rarity,
                        int buyingPrice,
                        int sellingPrice,
                        WeaponType weaponType,
                        DamageType damageType) {

            mName = name;
            mRange = range;
            mWeight = weight;
            mHitRate = hitRate;
            mCritRate = critRate;
            mRarity = rarity;
            mBuyingPrice = buyingPrice;
            mSellingPrice = sellingPrice;
            mWeaponType = weaponType;
            mDamageType = damageType;
            mSkills = new HashSet<Skill>();
        }
                        

        public Weapon(int might, int range, int hitRate, int critRate, DamageType damageType) {
            // TODO: Change how this is initialized
            mMight = might;
            mRange = range;
            mHitRate = hitRate;
            mCritRate = critRate;
            mDamageType = damageType;
        }
    }
}
