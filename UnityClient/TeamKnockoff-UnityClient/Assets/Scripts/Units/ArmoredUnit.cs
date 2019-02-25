using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public abstract class ArmoredUnit : Unit {

        const string UNIT_TYPE = "Armored";

        public ArmoredUnit() {
            Type = ArmoredUnit.UNIT_TYPE;
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
                    return false;
                case Tile.BoardTileType.Mountain:
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
                case Tile.BoardTileType.Tree:
                    return 2;
                case Tile.BoardTileType.Shallow:
                    return 2;
                default:
                    return Int32.MaxValue;
            }
        }
    }
}
