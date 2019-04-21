using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model{
    public class WaitMoveResult : GameMoveResult{
        private Vector2Int mUnitPosition;

        public Vector2Int UnitPosition => mUnitPosition;

        public WaitMoveResult(Vector2Int unitPosition) {
            mUnitPosition = unitPosition;
        }
    }
}
