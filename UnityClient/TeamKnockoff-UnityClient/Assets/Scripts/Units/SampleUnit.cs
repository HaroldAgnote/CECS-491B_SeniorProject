using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class SampleUnit : Unit {
        public SampleUnit()
        {
            Name = "Sample Unit";
            HealthPoints = 100;
            MoveRange = 3;
            AttackRange = 1;
        }
        public override List<Vector2Int> AttackLocations(Vector2Int gridPoint) {
            throw new NotImplementedException();
        }

        public override List<Vector2Int> MoveLocations(Vector2Int gridPoint) {
            throw new NotImplementedException();
        }
    }
}
