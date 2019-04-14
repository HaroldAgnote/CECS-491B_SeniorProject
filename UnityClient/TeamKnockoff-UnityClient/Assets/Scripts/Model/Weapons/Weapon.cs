using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Utilities.ExtensionMethods;
using Assets.Scripts.Utilities.Generator;
using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Weapons {

    [Serializable]
    public class Weapon : IGenerator<Weapon>, IComparable<Weapon>, IEquatable<Weapon> {

        public static Weapon FISTS = new Weapon() {
            mName = "Fists",
            mRange = 1,
            mMight = 0,
            mWeight = 0,
            mHitRate = 10,
            mCritRate = 0,
            mBuyingPrice = 0,
            mSellingPrice = 0,
            mDamageType = DamageType.Physical,
            mWeaponType = WeaponType.Fists,
            mSkills =  new HashSet<Skill>()
        };

        public enum WeaponType {
            Fists,
            Sword,
            Spear,
            Axe,
            Bow,
            Book,
            Staff
        }

        #region Fields

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

        #endregion

        #region Properties

        public string Name { get { return mName; } }

        public int Might { get { return mMight; } }

        public int Range { get { return mRange; } }

        public int Weight { get { return mWeight; } }

        public int HitRate { get { return mHitRate; } }

        public int CritRate { get { return mCritRate; } }

        public int Rarity { get { return mRarity; } }

        public int BuyingPrice { get { return mBuyingPrice; } }

        public int SellingPrice { get { return mSellingPrice; } }

        public bool IsBuyable => mBuyingPrice > 0;

        public bool IsSellable => mSellingPrice > 0;

        public WeaponType WeapType { get { return mWeaponType; } }

        public DamageType DamageType { get { return mDamageType; } }

        public HashSet<Skill> Skills { get { return mSkills; } }

        #endregion

        public Weapon() { }

        public Weapon(string name,
                        int range,
                        int weight,
                        int might,
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
            mMight = might;
            mHitRate = hitRate;
            mCritRate = critRate;
            mRarity = rarity;
            mBuyingPrice = buyingPrice;
            mSellingPrice = sellingPrice;
            mWeaponType = weaponType;
            mDamageType = damageType;
            mSkills = new HashSet<Skill>();
        }

        public Weapon(string name,
                        int range,
                        int weight,
                        int might,
                        int hitRate,
                        int critRate,
                        int rarity,
                        int buyingPrice,
                        int sellingPrice,
                        WeaponType weaponType,
                        DamageType damageType,
                        IEnumerable<Skill> skills) {

            mName = name;
            mRange = range;
            mWeight = weight;
            mMight = might;
            mHitRate = hitRate;
            mCritRate = critRate;
            mRarity = rarity;
            mBuyingPrice = buyingPrice;
            mSellingPrice = sellingPrice;
            mWeaponType = weaponType;
            mDamageType = damageType;
            mSkills = new HashSet<Skill>();
            mSkills.AddRange(skills);
        }

        public Weapon Generate() {
            return new Weapon(mName,
                            mRange,
                            mWeight,
                            mMight,
                            mHitRate,
                            mCritRate,
                            mRarity,
                            mBuyingPrice,
                            mSellingPrice,
                            mWeaponType,
                            mDamageType,
                            mSkills);
        }

        public bool Equals(Weapon other) {
            return this.mName == other.mName;
        }

        public int CompareTo(Weapon other) {
            return this.mName.CompareTo(other.Name);
        }
    }
}
