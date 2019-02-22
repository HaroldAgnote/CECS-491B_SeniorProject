using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class SampleUnit : Unit {
        public SampleUnit() {
            Name = "Sample Unit";
            HealthPoints = 100;
            MoveRange = 5;
            Mover = new SampleUnitMover();
            MainWeapon = new Weapon(5, 1, 100, 0);
        }
    }

    public class SampleUnitMover : IMover {
        public SampleUnitMover() { }

        public bool CanMove(Tile tile) {
            if (tile.TileType != Tile.BoardTileType.Boundary) {
                return true;
            }
            return false;
        }
    }
}
