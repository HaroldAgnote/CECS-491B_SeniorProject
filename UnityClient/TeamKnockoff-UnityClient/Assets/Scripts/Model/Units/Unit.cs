using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Items;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.UnitEffects;
using Assets.Scripts.Utilities.ExtensionMethods;

using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public abstract class Unit : IMover {

        #region Constants

        public const int INITIAL_LEVEL = 1;
        public const int INITIAL_EXPERIENCE_POINTS = 0;

        #endregion

        #region Fields

        [SerializeField]
        private string mName;

        [SerializeField]
        private string mType;

        [SerializeField]
        private string mClass;

        private int mCurrentHealthPoints;

        [SerializeField]
        private int mLevel;

        [SerializeField]
        private int mExperiencePoints;

        [SerializeField]
        private Stat mMaxHealthPoints;

        [SerializeField]
        private Stat mStrength;

        [SerializeField]
        private Stat mMagic;

        [SerializeField]
        private Stat mDefense;

        [SerializeField]
        private Stat mResistance;

        [SerializeField]
        private Stat mSpeed;

        [SerializeField]
        private Stat mSkill;

        [SerializeField]
        private Stat mLuck;

        [SerializeField]
        private Stat mMovement;

        [SerializeField]
        private Weapon mMainWeapon;

        [SerializeField]
        private List<Skill> mSkills;

        [SerializeField]
        private List<Item> mItems;

        private HashSet<UnitEffect> mUnitEffects;

        #endregion

        #region Properties

        public string Name {
            get { return mName; }
            set {
                if (mName != value) {
                    mName = value;
                }
            }
        }

        public string Type {
            get { return mType; }
            set {
                if (mType != value) {
                    mType = value;
                }
            }
        }

        public string Class {
            get { return mClass; }
            set {
                if (mClass != value) {
                    mClass = value;
                }
            }
        }

        /// <summary>
        /// Health Points are the life points of the Unit. 
        /// If Health Points are zero, the unit is dead. 
        /// Health Points can never exceed max health points.
        /// </summary>
        public int HealthPoints {
            get {
                return mCurrentHealthPoints;
            }
            set {
                // Prevent HP from exceeding above Max Health Points
                if (value > MaxHealthPoints.Value) {
                    mCurrentHealthPoints = MaxHealthPoints.Value;

                    // Prevent HP from exceeding below zero
                } else if (value < 0) {
                    mCurrentHealthPoints = 0;
                } else {
                    mCurrentHealthPoints = value;
                }
            }
        }

        public bool IsAlive {
            get {
                return HealthPoints > 0;
            }
        }

        public bool HasMoved { get; set; }

        public int Level {
            get {
                return mLevel;
            }
            protected set {
                if (mLevel != value) {
                    mLevel = value;
                }
            }
        }

        public int ExperiencePoints {
            get {
                return mExperiencePoints;
            }
            protected set {
                if (mExperiencePoints != value) {
                    mExperiencePoints = value;
                }
            }
        }


        public Stat MaxHealthPoints {
            get {
                return mMaxHealthPoints;
            }
            protected internal set {
                if (mMaxHealthPoints != value) {
                    mMaxHealthPoints = value;
                }
            }
        }
        public Stat Strength {
            get {
                return mStrength;
            }
            protected internal set {
                if (mStrength != value) {
                    mStrength = value;
                }
            }
        }
        public Stat Magic {
            get {
                return mMagic;
            }
            protected internal set {
                if (mMagic != value) {
                    mMagic = value;
                }
            }
        }
        public Stat Defense {
            get {
                return mDefense;
            }
            protected internal set {
                if (mDefense != value) {
                    mDefense = value;
                }
            }
        }

        public Stat Resistance {
            get {
                return mResistance;
            }
            protected internal set {
                if (mResistance != value) {
                    mResistance = value;
                }
            }
        }

        public Stat Speed {
            get {
                return mSpeed;
            }
            protected internal set {
                if (mSpeed != value) {
                    mSpeed = value;
                }
            }
        }

        public Stat Skill {
            get {
                return mSkill;
            }
            protected internal set {
                if (mSkill != value) {
                    mSkill = value;
                }
            }
        }

        public Stat Luck {
            get {
                return mLuck;
            }
            protected internal set {
                if (mLuck != value) {
                    mLuck = value;
                }
            }
        }

        public Stat Movement {
            get {
                return mMovement;
            }
            protected internal set {
                if (mMovement != value) {
                    mMovement = value;
                }
            }
        }

        public Weapon MainWeapon {
            get {
                return mMainWeapon;
            }
        }

        public List<Skill> Skills {
            get {
                var combinedSkills = mSkills.Union(mMainWeapon.Skills);
                return combinedSkills.ToList();
            }
        }

        // TODO: Add Item Properties
        public List<Item> Items {
            get {
                return mItems;
            }
        }

        public HashSet<UnitEffect> UnitEffects {
            get {
                return mUnitEffects;
            }
        }

        public string UnitInformation {
            get {
                var info =
                    $"Name: {Name}\n" +
                    $"HealthPoints: {HealthPoints}\n" +
                    $"MaxHealth: {MaxHealthPoints}\n" +
                    $"Level: {Level}\n" +
                    $"Strength: {Strength}\n" +
                    $"Magic: {Magic}\n" +
                    $"Defense: {Defense}\n" +
                    $"Resistance: {Resistance}\n" +
                    $"Speed: {Speed}\n" +
                    $"Skill: {Skill}\n" +
                    $"Luck: {Luck}\n" +
                    $"Level: {Level}\n" +
                    $"Experience: {ExperiencePoints}\n" +
                    $"Move Range: {Movement}\n";
                return info;
            }
        }

        #endregion

        #region Methods

        // Abstract methods that must be overridden by Unit sub classes
        public abstract bool CanMove(Tile tile);
        public abstract int MoveCost(Tile tile);

        public Unit() {
            mMaxHealthPoints = new Stat();
            mStrength = new Stat();
            mMagic = new Stat();
            mDefense = new Stat();
            mResistance = new Stat();
            mSpeed = new Stat();
            mSkill = new Stat();
            mLuck = new Stat();
            mMovement = new Stat();

            mLevel = 1;
            mExperiencePoints = 0;

            mUnitEffects = new HashSet<UnitEffect>();
            mSkills = new List<Skill>();
            mItems = new List<Item>();
        }

        public Unit(string unitName, string unitType, string unitClass, int maxHealth, int strength, int magic, int defense, int resistance, int speed, int skill, int luck, int movement) {
            mName = unitName;
            mType = unitType;
            mClass = unitClass;

            mMaxHealthPoints = new Stat(maxHealth);
            mCurrentHealthPoints = mMaxHealthPoints.Value;
            mStrength = new Stat(strength);
            mMagic = new Stat(magic);
            mDefense = new Stat(defense);
            mResistance = new Stat(resistance);
            mSpeed = new Stat(speed);
            mSkill = new Stat(skill);
            mLuck = new Stat(luck);
            mMovement = new Stat(movement);

            mLevel = INITIAL_LEVEL;
            mExperiencePoints = INITIAL_EXPERIENCE_POINTS;

            mUnitEffects = new HashSet<UnitEffect>();
            mSkills = new List<Skill>();
            mItems = new List<Item>();
        }

        public Unit (UnitWrapper unitWrapper) {
            mName = unitWrapper.unitName;
            mType = unitWrapper.unitType;
            mClass = unitWrapper.unitClass;

            mMaxHealthPoints = unitWrapper.unitMaxHealthPoints;
            mCurrentHealthPoints = mMaxHealthPoints.Value;
            mStrength = unitWrapper.unitStrength;
            mMagic = unitWrapper.unitMagic;
            mDefense = unitWrapper.unitDefense;
            mResistance = unitWrapper.unitResistance;
            mSpeed = unitWrapper.unitSpeed;
            mSkill = unitWrapper.unitSkill;
            mLuck = unitWrapper.unitLuck;
            mMovement = unitWrapper.unitMovement;

            mLevel = unitWrapper.unitLevel;
            mExperiencePoints = unitWrapper.unitExperiencePoints;

            // TODO: Re-add weapon using WeaponFactory
            var weaponName = unitWrapper.unitWeapon;

            // No need to call equip since modifier is already applied
            mMainWeapon = WeaponFactory.instance.GenerateWeapon(weaponName);

            mUnitEffects = new HashSet<UnitEffect>();

            // TODO: Re-add skills using SkillFactory
            mSkills = new List<Skill>();

            var skillNames = unitWrapper.unitSkills;
            foreach (var skillName in skillNames) {
                var skill = SkillFactory.instance.GenerateSkill(skillName);
                mSkills.Add(skill);
            }

            // TODO: Re-add items using ItemFactory
            mItems = new List<Item>();
        }

        public void StartTurn() {

            HasMoved = IsAlive ? false : true;

            var removeList = new List<UnitEffect>();

            foreach (var effect in mUnitEffects) {
                if (effect is IApplicableEffect) {
                    var applicableEffect = effect as IApplicableEffect;
                    if (applicableEffect.IsApplicable()) {
                        applicableEffect.ApplyEffect(this);
                    }
                }

                if (effect is IRemovableEffect) {
                    var removableEffect = effect as IRemovableEffect;
                    if (removableEffect.IsRemovable()) {
                        removableEffect.RemoveEffect(this);
                        removeList.Add(effect);
                    }
                }

                if (effect is ITurnEffect) {
                    (effect as ITurnEffect).UpdateTurns();
                }
            }

            mUnitEffects.RemoveRange(removeList);
        }

        public void EquipWeapon(Weapon newWeapon) {
            mMainWeapon = newWeapon;

            if (mMainWeapon.DamageType == DamageType.Physical) {
                mStrength.Modifier += mMainWeapon.Might;
            }

            if (mMainWeapon.DamageType == DamageType.Magical) {
                mMagic.Modifier += mMainWeapon.Might;
            }

            mSpeed.Modifier -= mMainWeapon.Weight;

        }

        public Weapon UnequipWeapon() {
            if (mMainWeapon.DamageType == DamageType.Physical) {
                mStrength.Modifier -= mMainWeapon.Might;
            }

            if (mMainWeapon.DamageType == DamageType.Magical) {
                mMagic.Modifier -= mMainWeapon.Might;
            }

            mSpeed.Modifier += mMainWeapon.Weight;

            var weapon = mMainWeapon;
            mMainWeapon = null;
            return weapon;
        }

        public void LearnSkill(Skill newSkill) {
            mSkills.Add(newSkill);
        }

        public void GainExperience(Unit defendingUnit) {
            if (!defendingUnit.IsAlive) {
                mExperiencePoints += 20;
            } else {
                mExperiencePoints += 3;
            }

            if (ExperiencePoints >= 100) {
                mExperiencePoints -= 100;
                mLevel += 1;
            }
        }

        #endregion

    }
}
