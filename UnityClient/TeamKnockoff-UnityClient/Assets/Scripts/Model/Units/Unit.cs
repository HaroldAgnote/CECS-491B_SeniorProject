using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Items;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.UnitEffects;

namespace Assets.Scripts.Model.Units {
    public abstract class Unit : MonoBehaviour, IMover {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }

        private double mCurrentHealthPoints;

        public struct BaseStats {
            public int Strength { get; private set; }
            public int Magic { get; private set; }
            public int Defense { get; private set; }
            public int Resistance { get; private set; }
            public int Speed { get; private set; }
            public int Skill { get; private set; }
            public int Luck { get; private set; }
            public int MoveRange { get; private set; }
        }

        public BaseStats BaseStat { get; private set; }

        public struct StatModifiers {
            public int StrengthModifier;
            public int MagicModifier;
            public int DefenseModifier;
            public int ResistanceModifier;
            public int SpeedModifier;
            public int SkillModifier;
            public int LuckModifier;
            public int MoveRangeModifier;
        }

        public StatModifiers StatModifier { get; private set; }

        /// <summary>
        /// Health Points are the life points of the Unit. 
        /// If Health Points are zero, the unit is dead. 
        /// Health Points can never exceed max health points.
        /// </summary>
        public double HealthPoints {
            get {
                return mCurrentHealthPoints;
            }
            set {
                // Prevent HP from exceeding above Max Health Points
                if (value > MaxHealthPoints + MaxHealthModifier) {
                    mCurrentHealthPoints = MaxHealthPoints + MaxHealthModifier;

                // Prevent HP from exceeding below zero
                } else if (value < 0) {
                    mCurrentHealthPoints = 0;
                } else {
                    mCurrentHealthPoints = value;
                }
            }
        }

        public double MaxHealthPoints { get; protected set; }

        public double MaxHealthModifier { get; set; }

        public bool IsAlive {
            get {
                return HealthPoints > 0;
            }
        } 

        public bool HasMoved { get; set; }


        public int Level { get; protected set; }
        public int ExperiencePoints { get; protected set; }

        public int Strength { get; protected set; }
        public int Magic { get; protected set; }

        public int Defense { get; protected set; }
        public int Resistance { get; protected set; }

        public int Speed { get; protected set; }
        public int Skill { get; protected set; }

        public int Luck { get; protected set; }

        public int MoveRange { get; protected set; }

        public Weapon MainWeapon { get; protected set; }

        public List<Skill> Skills { get; protected set; }

        // TODO: Add Item Properties
        public List<Item> Items { get; protected set; }

        public List<UnitEffect> UnitEffects { get; }

        // Abstract methods that must be overridden by Unit sub classes
        public abstract bool CanMove(Tile tile);
        public abstract int MoveCost(Tile tile);

        public Unit() {
            Skills = new List<Skill>();
            Items = new List<Item>();
            UnitEffects = new List<UnitEffect>();
            MainWeapon = new Weapon(50, 1, 100, 1, DamageCalculator.DamageType.Physical);
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
                    $"Move Range: {MoveRange}\n";
                return info;
            }
        }
    }
}
