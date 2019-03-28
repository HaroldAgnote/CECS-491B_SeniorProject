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

namespace Assets.Scripts.Model.Units {
    public abstract class Unit : MonoBehaviour, IMover {

        public string Name { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }

        private int mCurrentHealthPoints;

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
                if (value > MaxHealthPoints.Value ) {
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

        public void GainExperience(Unit defender)
        {
            if(defender == null) //if unit is dead
            {
                ExperiencePoints += 20;
            }

            else
            {
                ExperiencePoints += 3;
            }
            //exp logic
            //some bullshit event handler that calls levelUP
        }

        public void LevelUp()
        {

        }

        public bool HasMoved { get; set; }

        public int Level { get; protected set; }
        public int ExperiencePoints { get; protected set; }

        public class Stat {
            public int Base { get; protected internal set; }
            public int Modifier { get; protected internal set; }

            public Stat() {
                Base = 0;
                Modifier = 0;
            }

            public Stat(int baseStat) {
                Base = baseStat;
                Modifier = 0;
            }

            public Stat(int baseStat, int initialModifier) {
                Base = baseStat;
                Modifier = initialModifier;
            }

            public int Value => Base + Modifier;

            public override string ToString() {
                if (Modifier == 0) {
                    return $"{Base}";
                } else if (Modifier > 0) {
                    return $"{Base} +{Modifier}";
                } else {
                    return $"{Base} {Modifier}";
                }
            }

        }

        public Stat MaxHealthPoints { get; protected internal set; } = new Stat();
        public Stat Strength { get; protected internal set; } = new Stat();
        public Stat Magic { get; protected internal set; } = new Stat();
        public Stat Defense { get; protected internal set; } = new Stat();
        public Stat Resistance { get; protected internal set; } = new Stat();
        public Stat Speed { get; protected internal set; } = new Stat();
        public Stat Skill { get; protected internal set; } = new Stat();
        public Stat Luck { get; protected internal set; } = new Stat();
        public Stat Movement { get; protected internal set; } = new Stat();

        public Weapon MainWeapon { get; private set; }

        public List<Skill> Skills { get; protected set; }

        // TODO: Add Item Properties
        public List<Item> Items { get; protected set; }

        public HashSet<UnitEffect> UnitEffects { get; protected set; }

        // Abstract methods that must be overridden by Unit sub classes
        public abstract bool CanMove(Tile tile);
        public abstract int MoveCost(Tile tile);

        void Awake() {
            Skills = new List<Skill>();
            Items = new List<Item>();
            Level = 1;
            ExperiencePoints = 0;
            UnitEffects = new HashSet<UnitEffect>();
            MainWeapon = new Weapon(1, 1, 100, 1, Assets.Scripts.Model.DamageCalculator.DamageType.Physical);
        }

        public void StartTurn() {

            HasMoved = IsAlive ? false : true;

            var removeList = new List<UnitEffect>();

            foreach (var effect in UnitEffects) {
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

            UnitEffects.RemoveRange(removeList);
        }

        public void EquipWeapon(Weapon newWeapon) {
            MainWeapon = newWeapon;

            if (MainWeapon.DamageType == Assets.Scripts.Model.DamageCalculator.DamageType.Physical) {
                Strength.Modifier += MainWeapon.Might;
            }

            if (MainWeapon.DamageType == Assets.Scripts.Model.DamageCalculator.DamageType.Magical) {
                Magic.Modifier += MainWeapon.Might;
            }

        }

        public Weapon UnequipWeapon() {
            if (MainWeapon.DamageType == Assets.Scripts.Model.DamageCalculator.DamageType.Physical) {
                Strength.Modifier -= MainWeapon.Might;
            }

            if (MainWeapon.DamageType == Assets.Scripts.Model.DamageCalculator.DamageType.Magical) {
                Magic.Modifier -= MainWeapon.Might;
            }
            var weapon = MainWeapon;
            MainWeapon = null;
            return weapon;
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
                    $"Experience: {ExperiencePoints}\n";
                    $"Move Range: {Movement}\n";
                return info;
            }
        }
    }
}
