using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Tiles;

namespace Assets.Scripts.Model.Units {
    public abstract class FlyingUnit : Unit {

        const string UNIT_TYPE = "Flying";

        public FlyingUnit() {
            Type = FlyingUnit.UNIT_TYPE;
        }

        public override bool CanMove(Tile tile) {
            var tileType = tile.TileType;
            switch (tileType) {
                case Tile.BoardTileType.Normal:
                    return true;
                case Tile.BoardTileType.Rough:
                    return true;
                case Tile.BoardTileType.Wall:
                    return true;
                case Tile.BoardTileType.Slope:
                    return true;
                case Tile.BoardTileType.Obstacle:
                    return true;
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
                case Tile.BoardTileType.Wall:
                    return 1;
                case Tile.BoardTileType.Rough:
                    return 1;
                case Tile.BoardTileType.Slope:
                    return 2;
                case Tile.BoardTileType.Obstacle:
                    return 2;
                case Tile.BoardTileType.Boundary:
                    return Int32.MaxValue;
                default:
                    return Int32.MaxValue;
            }
        }
    }
}
