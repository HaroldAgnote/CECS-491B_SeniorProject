using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Tiles;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public abstract class CavalryUnit : Unit {

        const string UNIT_TYPE = "Cavalry";
        const string MOVE_TYPE = "Land";
        
        public CavalryUnit(string unitName, string unitClass, int maxHealth, int strength, int magic, int defense, int resistance, int speed, int skill, int luck, int movement) 
            : base(unitName, UNIT_TYPE, unitClass, MOVE_TYPE, maxHealth, strength, magic, defense, resistance, speed, skill, luck, movement)
            { }

        public CavalryUnit(UnitWrapper unitWrapper) : base (unitWrapper) { }
        
        public override bool CanMove(Tile tile) {
            var tileType = tile.TileType;
            switch (tileType) {
                case Tile.BoardTileType.Normal:
                    return true;
                case Tile.BoardTileType.Rough:
                    return false;
                case Tile.BoardTileType.Wall:
                    return false;
                case Tile.BoardTileType.Slope:
                    return false;
                case Tile.BoardTileType.Obstacle:
                    return false;
                case Tile.BoardTileType.Boundary:
                    return false;
                default:
                    return false;
            }
        }

        public override int MoveCost(Tile tile) {
            var tileType = tile.TileType;
            switch (tileType) {
                case Tile.BoardTileType.Normal:
                    return 1;
                default:
                    return Int32.MaxValue;
            }
        }
    }
}
