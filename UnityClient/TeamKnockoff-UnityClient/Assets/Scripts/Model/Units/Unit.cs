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
using Assets.Scripts.Utilities.Cloneable;
using Assets.Scripts.Utilities.ExtensionMethods;
using Assets.Scripts.Utilities.Generator;

using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public abstract class Unit : IMover, IEquatable<Unit>, IGenerator<Unit>, ICloneable<Unit> {

        #region Constants

        public const int INITIAL_LEVEL = 1;
        public const int INITIAL_EXPERIENCE_POINTS = 0;
        public const int CONSUMABLE_ITEM_LIMIT = 3;

        #endregion

        #region Fields

        [SerializeField]
        private string mName;

        [SerializeField]
        private string mType;

        [SerializeField]
        private string mClass;

        [SerializeField]
        private string mMoveType;

        [SerializeField]
        private int mPlayerNumber;

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

        private PassiveItem mPassiveItem;

        private ConsumableItem[] mConsumableItems;

        private HashSet<UnitEffect> mUnitEffects;

        protected Dictionary<int, List<Skill>> mLevelToSkills;

        [SerializeField]
        protected int mHealthGrowthRate;
        [SerializeField]
        protected int mStrenthGrowthRate;
        [SerializeField]
        protected int mMagicGrowthRate;
        [SerializeField]
        protected int mDefenseGrowthRate;
        [SerializeField]
        protected int mResistanceGrowthRate;
        [SerializeField]
        protected int mSpeedGrowthRate;
        [SerializeField]
        protected int mSkillGrowthRate;
        [SerializeField]
        protected int mLuckGrowthRate;
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

        public string MoveType {
            get { return mMoveType; }
            set {
                if (mMoveType != value) {
                    mMoveType = value;
                }
            }
        }

        public int PlayerNumber {
            get { return mPlayerNumber; }
            set {
                if (mPlayerNumber != value) {
                    mPlayerNumber = value;
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

        public bool IsAlive => HealthPoints > 0;

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

        public int Rating =>
            mMaxHealthPoints.Base +
                mStrength.Base +
                mMagic.Base +
                mDefense.Base +
                mResistance.Base +
                mSpeed.Base +
                mSkill.Base +
                mLuck.Base;
                //mMovement.Value;

        public IEnumerable<Skill> UnitSkills {
            get { return mSkills.AsReadOnly(); }
        }

        public List<Skill> Skills {
            get {
                var combinedSkills = mSkills.Union(mMainWeapon.Skills);
                // TODO: Sebastian
                // Union this with each item's Field Skill
                if (mPassiveItem != null) {
                    var passiveItemSkills = mPassiveItem.Effects;
                    combinedSkills = combinedSkills.Union(passiveItemSkills);
                }
                
                return combinedSkills.ToList();
            }
        }

        // TODO: Add Item Properties
        public List<Item> Items {
            get {
                var itemList = new List<Item>();
                if (mPassiveItem != null) {
                    itemList.Add(mPassiveItem);
                }
                itemList.AddRange(mConsumableItems.Where(item => item != null));

                return itemList;
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
        public abstract bool CanUse(Weapon weapon);
        public abstract Unit Clone();
        public abstract Unit Generate();
        public abstract Unit Generate(string unitName);
        public abstract Unit Generate(UnitWrapper unitWrapper);

        public Unit(string unitName, string unitType, string unitClass, string moveType, int maxHealth, int strength, int magic, int defense, int resistance, int speed, int skill, int luck, int movement) {
            mName = unitName;
            mType = unitType;
            mClass = unitClass;
            mMoveType = moveType;

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
            mConsumableItems = new ConsumableItem[CONSUMABLE_ITEM_LIMIT];
            mLevelToSkills = new Dictionary<int, List<Skill>>();
        }

        public Unit(UnitWrapper unitWrapper) {
            mName = unitWrapper.unitName;
            mType = unitWrapper.unitType;
            mClass = unitWrapper.unitClass;
            mMoveType = unitWrapper.unitMoveType;

            mMaxHealthPoints = new Stat(unitWrapper.unitMaxHealthPoints);
            mCurrentHealthPoints = mMaxHealthPoints.Value;
            mStrength = new Stat(unitWrapper.unitStrength);
            mMagic = new Stat(unitWrapper.unitMagic);
            mDefense = new Stat(unitWrapper.unitDefense);
            mResistance = new Stat(unitWrapper.unitResistance);
            mSpeed = new Stat(unitWrapper.unitSpeed);
            mSkill = new Stat(unitWrapper.unitSkill);
            mLuck = new Stat(unitWrapper.unitLuck);
            mMovement = new Stat(unitWrapper.unitMovement);

            mLevel = unitWrapper.unitLevel;
            mExperiencePoints = unitWrapper.unitExperiencePoints;

            // TODO: Re-add weapon using WeaponFactory
            var weaponName = unitWrapper.unitWeapon;

            // No need to call equip since modifier is already applied
            mMainWeapon = WeaponFactory.instance.GenerateWeapon(weaponName);

            mUnitEffects = new HashSet<UnitEffect>();

            mSkills = new List<Skill>();

            var skillNames = unitWrapper.unitSkills;
            foreach (var skillName in skillNames) {
                var skill = SkillFactory.instance.GenerateSkill(skillName);
                mSkills.Add(skill);
            }

            mConsumableItems = new ConsumableItem[CONSUMABLE_ITEM_LIMIT];

            var itemNames = unitWrapper.unitItems;

            foreach (var itemName in itemNames) {
                var item = ItemFactory.instance.GenerateItem(itemName);
                EquipItem(item);
            }
            mLevelToSkills = new Dictionary<int, List<Skill>>();
        }

        public void StartGame() {
            mCurrentHealthPoints = MaxHealthPoints.Value;
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

            var fieldSkills = Skills.Where(skill => skill is FieldSkill)
                                    .Select(skill => skill as FieldSkill)
                                    .Where(skill => !skill.IsApplied);

            foreach (var skill in fieldSkills) {
                skill.ApplyFieldSkill(this);
            }

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

            mMainWeapon = Weapon.FISTS;

            return weapon;
        }

        public void EquipItem(Item item) {
            if (item is PassiveItem) {
                mPassiveItem = item as PassiveItem;
            } else if (item is ConsumableItem) {
                for (int i = 0; i < mConsumableItems.Length; i++) {
                    if (mConsumableItems[i] == null) {
                        mConsumableItems[i] = item as ConsumableItem;
                        break;
                    }
                }
            } else {
                throw new Exception("Could not Equip Item");
            }
        }

        public Item UnequipItem(Item item) {
            if (mPassiveItem == item) {
                var tempItem = mPassiveItem;
                mPassiveItem = null;
                return tempItem;
            }

            for (int i = 0; i < mConsumableItems.Length; i++) {
                if (mConsumableItems[i] == item) {
                    var tempItem = mConsumableItems[i];
                    mConsumableItems[i] = null;
                    return tempItem;
                }
            }
            throw new Exception("Could not unequip item");
        }

        public bool CanEquipItem(Item item) {
            if (item is PassiveItem) {
                return mPassiveItem == null;
            } else if (item is ConsumableItem) {
                return mConsumableItems.Any(it => it == null);
            }
            throw new Exception("Could not determine if item can be equipped");
        }

        public void LearnSkill(Skill newSkill) {
            mSkills.Add(newSkill);
        }

        public void GainExperience(int flatValue) {
            mExperiencePoints += flatValue;

            if (ExperiencePoints >= 100) {
                mExperiencePoints -= 100;
                LevelUp();
            }
        }

        public void GainExperience(Unit defendingUnit) {
            const int RATINGTHRESHOLD = 10;
            //if ratingDif is pos, self is stronger than defendingUnit
            int ratingDif = this.Rating - defendingUnit.Rating;

            if (!defendingUnit.IsAlive) {
                if(ratingDif > RATINGTHRESHOLD) {
                    mExperiencePoints += 27;
                }
                else if(ratingDif < -(RATINGTHRESHOLD)) {
                    mExperiencePoints += 110;
                }
                else { //threshold -20 <= x <= 20
                    mExperiencePoints += 40;
                }
                
            } else {
                mExperiencePoints += 13;
            }

            if (ExperiencePoints >= 100) {
                mExperiencePoints -= 100;
                LevelUp();
                //mLevel += 1;
            }
        }

        public bool Equals(Unit other) {
            return this.mName == other.mName;
        }

        public void SetLevel(int level) {
            for (int currentLevel = 1; currentLevel <= level; currentLevel++) {
                LevelUp();
            }
        }

        public void SetLevelRange(int lowLevel, int highLevel) {
            var randomLevel = UnityEngine.Random.Range(lowLevel, highLevel);
            SetLevel(randomLevel);
        }

        public void LevelUp() {
            mLevel += 1;

            if (mLevelToSkills.Keys.Contains(mLevel)) {
                var newSKills = mLevelToSkills[mLevel];
                foreach (var skill in newSKills) {
                    LearnSkill(skill);
                }
            }

            // Stat Boosts
            int chance = UnityEngine.Random.Range(0, 100);
            //int chance = 29;
            // Debug.Log($"This is chance: {chance}");

            List<Tuple<Stat, int>> statGrowthRateTuples = new List<Tuple<Stat, int>>() {
                new Tuple<Stat, int>(MaxHealthPoints, mHealthGrowthRate),
                new Tuple<Stat, int>(Strength, mStrenthGrowthRate),
                new Tuple<Stat, int>(Magic, mMagicGrowthRate),
                new Tuple<Stat, int>(Defense, mDefenseGrowthRate),
                new Tuple<Stat, int>(Resistance, mResistanceGrowthRate),
                new Tuple<Stat, int>(Speed, mSpeedGrowthRate),
                new Tuple<Stat, int>(Skill, mSkillGrowthRate),
                new Tuple<Stat, int>(Luck, mLuckGrowthRate),
            };

            foreach (var statGrowthRate in statGrowthRateTuples) {
                RollForStat(statGrowthRate);
            }

        }

        public void RollForStat(Tuple<Stat, int> statGrowthRate) {
            var stat = statGrowthRate.Item1;
            var growthRate = statGrowthRate.Item2;

            int chance = UnityEngine.Random.Range(0, 100);
            if (growthRate > chance) {
                stat.Base += 1;
            }
        }

        #endregion

    }
}
