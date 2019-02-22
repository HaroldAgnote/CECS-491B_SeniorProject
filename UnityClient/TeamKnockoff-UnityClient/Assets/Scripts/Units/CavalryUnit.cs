using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Units {
    public abstract class CavalryUnit : Unit {

        const string UNIT_TYPE = "Cavalry";
        
        public CavalryUnit() {
            Type = CavalryUnit.UNIT_TYPE;
        }

        public override bool CanMove(Tile tile) {
            var tileType = tile.TileType;
            switch (tileType) {
                case Tile.BoardTileType.Normal:
                    return true;
                case Tile.BoardTileType.Tree:
                    return false;
                case Tile.BoardTileType.Shallow:
                    return false;
                case Tile.BoardTileType.Deep:
                    return false;
                case Tile.BoardTileType.Mountain:
                    return false;
                case Tile.BoardTileType.Obstacle:
                    return false;
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
                case Tile.BoardTileType.Damage:
                    return 1;
                case Tile.BoardTileType.Fortify:
                    return 1;
                default:
                    return Int32.MaxValue;
            }
        }
    }
}
