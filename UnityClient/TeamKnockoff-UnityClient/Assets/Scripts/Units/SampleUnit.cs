using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class SampleUnit : Unit, IMover {

        public static GameObject CreateSampleUnit(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<SampleUnit>();

            return newUnit;
        }

        public SampleUnit() {
            Name = "Sample Unit";
            MaxHealthPoints = 100;
            MoveRange = 5;
            MainWeapon = new Weapon();
            Mover = new SampleUnitMover();
            MainWeapon = new Weapon(5, 1, 100, 0);
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
                    return true;
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
            return 1;
        }
    }
}
