using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model{
    public class PositionMoveResult : GameMoveResult {
        private Vector2Int mStartPosition;
        private Vector2Int mEndPosition;
        private List<Vector2Int> mPath;

        public Vector2Int StartPosition => mStartPosition;
        public Vector2Int EndPosition => mEndPosition;
        public List<Vector2Int> Path => mPath;

        public PositionMoveResult(Vector2Int startPosition, Vector2Int endPosition, List<Vector2Int> path) {
            mStartPosition = startPosition;
            mEndPosition = endPosition;
            mPath = path;
        }
    }
}
