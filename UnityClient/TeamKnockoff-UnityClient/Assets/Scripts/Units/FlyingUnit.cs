using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
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
                case Tile.BoardTileType.Tree:
                    return true;
                case Tile.BoardTileType.Shallow:
                    return true;
                case Tile.BoardTileType.Deep:
                    return true;
                case Tile.BoardTileType.Mountain:
                    return true;
                case Tile.BoardTileType.Obstacle:
                    return true;
                case Tile.BoardTileType.Damage:
                    return true;
                case Tile.BoardTileType.Fortify:
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
                case Tile.BoardTileType.Tree:
                    return 1;
                case Tile.BoardTileType.Shallow:
                    return 1;
                case Tile.BoardTileType.Deep:
                    return 1;
                case Tile.BoardTileType.Mountain:
                    return 2;
                case Tile.BoardTileType.Obstacle:
                    return 2;
                case Tile.BoardTileType.Damage:
                    return 1;
                case Tile.BoardTileType.Fortify:
                    return 1;
                case Tile.BoardTileType.Boundary:
                    return Int32.MaxValue;
                default:
                    return Int32.MaxValue;
            }
        }
    }
}
