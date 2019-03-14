﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    public class SampleUnit : Unit, IMover {

        public static GameObject CreateSampleUnit(GameObject unitPrefab, Sprite unitSprite, Vector3 tilePos, Transform parent) {
            var newUnit = Instantiate(unitPrefab, tilePos, Quaternion.identity, parent) as GameObject;
            newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

            newUnit.AddComponent<SampleUnit>();

            return newUnit;
        }

        public SampleUnit() {
            Name = "Sample Unit";
            MaxHealthPoints = 100;
            MoveRange = 5;
            MainWeapon = new Weapon(1000, 1, 100, 0, DamageCalculator.DamageType.Physical);
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