using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class SampleUnit : Unit, IMover {

        public static SampleUnit CreateSampleUnit() {
            return new SampleUnit();
        }

        public SampleUnit(): base()  {
            Name = "Sample Unit";
            MaxHealthPoints.Base = 100;
            Movement.Base = 5;
        }

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
            return 1;
        }
    }
}
