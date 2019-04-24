using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.View {
    public class UnitView : ObjectView {

        public UnitView(GameObject gameObject) : base(gameObject) {

        }

        public void NudgeTowardsPosition(Vector2Int startPosition, Vector2Int endPosition) {
            var direction = startPosition.GetVectorDirection(endPosition);
            var vectorDirection = VectorExtension.GetVectorTowardsDirection(direction);
            var partialVector = new Vector3() {
                x = (float)(vectorDirection.x * 0.25),
                y = (float)(vectorDirection.y * 0.25),
                z = 0.0f
            };
            var movePosition = startPosition.ToVector3() + partialVector;

            var currentPosition = new Vector3(startPosition.x, startPosition.y);
            var path = new List<Vector3>() {
                currentPosition,
            };

            const float INTERVAL = (float) 0.10;

            while (Vector3.Distance(currentPosition, movePosition) > 0.005f) {
                var nextPosition = Vector3.MoveTowards(currentPosition, movePosition, INTERVAL);
                if (partialVector.x != 0) {
                    currentPosition.x = nextPosition.x;
                }
                if (partialVector.y != 0) {
                    currentPosition.y = nextPosition.y;
                }
                path.Add(nextPosition);
            }
            path.Remove(path.Last());
            path.Add(movePosition);

            while (Vector3.Distance(currentPosition, startPosition.ToVector3()) > 0.005f) {
                var nextPosition = Vector3.MoveTowards(currentPosition, startPosition.ToVector3(), INTERVAL);
                if (partialVector.x != 0) {
                    currentPosition.x = nextPosition.x;
                }
                if (partialVector.y != 0) {
                    currentPosition.y = nextPosition.y;
                }
                path.Add(nextPosition);
            }
            path.Remove(path.Last());
            path.Add(startPosition.ToVector3());

            var mover = GameObject.GetComponent<UnitMover>();
            mover.SetPath(path);
        }

        // Todo: Override movement methods in ObjectView to animate movement
        public override void UpdatePosition(List<Vector2Int> pathToNewPosition) {
            var mover = GameObject.GetComponent<UnitMover>();
            mover.SetPath(pathToNewPosition.Select(pos => pos.ToVector3()).ToList());
        }
    }
}
