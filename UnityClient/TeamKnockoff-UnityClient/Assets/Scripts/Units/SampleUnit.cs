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
            AttackRange = 1;
        }

        public override List<Vector2Int> GetMoveLocations(Vector2Int gridPoint) {
            var moveLocations = new List<Vector2Int>();

            var moveQueue = new Queue<Vector2Int>();
            moveQueue.Enqueue(gridPoint);

            throw new NotImplementedException();
        }

        private Vector2Int FindNextMoveLocation(Vector2Int currentGridPoint, int maxMoveRange) {

            throw new NotImplementedException();
        } 

        public override List<Vector2Int> GetAttackLocations(Vector2Int gridPoint) {
            throw new NotImplementedException();
        }

    }
}
