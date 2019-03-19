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

namespace Assets.Scripts.Model.Units {
    public abstract class Unit : MonoBehaviour, IMover {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }

        private double mHealthPoints;

        // Health Points are the life points of the Unit
        // If Health Points is zero, the unit is dead
        // Health Points can never exceed max health points
        public double HealthPoints {
            get {
                return mHealthPoints;
            }
            set {
                if (value < 0) {
                    mHealthPoints = 0;
                } else {
                    mHealthPoints = value;
                }
            }
        }
        public double MaxHealthPoints { get; set; }

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

        public List<Skill> Skills { get; set; }

        // TODO: Add Item Properties
        public List<Item> Items { get; set; }

        public List<UnitEffect> Effects { get; }

        public bool IsAlive {
            get {
                return HealthPoints > 0;
            }
        } 

        // Abstract methods that must be overridden by Unit sub classes
        public abstract bool CanMove(Tile tile);
        public abstract int MoveCost(Tile tile);

        public Unit() {
            Items = new List<Item>();
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
