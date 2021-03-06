﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Tiles;

namespace Assets.Scripts.Model.Units {

    [Serializable]
    public abstract class InfantryUnit : Unit {

        const string UNIT_TYPE = "Infantry";
        const string MOVE_TYPE = "Land";

        public InfantryUnit(string unitName, string unitClass, int maxHealth, int strength, int magic, int defense, int resistance, int speed, int skill, int luck, int movement) 
            : base(unitName, UNIT_TYPE, unitClass, MOVE_TYPE, maxHealth, strength, magic, defense, resistance, speed, skill, luck, movement)
            { }

        public InfantryUnit(UnitWrapper unitWrapper) : base (unitWrapper) { }

        public override bool CanMove(Tile tile) {
            var tileType = tile.TileType;
            switch (tileType) {
                case Tile.BoardTileType.Normal:
                    return true;
                case Tile.BoardTileType.Rough:
                    return true;
                case Tile.BoardTileType.Wall:
                    return false;
                case Tile.BoardTileType.Slope:
                    return true;
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
                case Tile.BoardTileType.Rough:
                    return 2;
                case Tile.BoardTileType.Wall:
                    return Int32.MaxValue;
                case Tile.BoardTileType.Slope:
                    return 3;
                case Tile.BoardTileType.Obstacle:
                    return Int32.MaxValue;
                case Tile.BoardTileType.Boundary:
                    return Int32.MaxValue;
                default:
                    return Int32.MaxValue;
            }
        }
    }
}
