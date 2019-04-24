using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.View {
    public class UnitMover : MonoBehaviour {
        const float SPEED = 5.0f;

        private Vector3 mPosition;
        private Vector3 mDestination;
        private Queue<Vector3> mPath;
        private bool mMoving;

        public bool IsMoving => mMoving;

        void Start() {
            mPosition = transform.position;
        }

        void Update() {
            if (mMoving) {
                if (mPath.Count > 0) {
                    var nextPos = mPath.Peek();
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * SPEED);
                    mPosition = transform.position;
                    if (mPosition == nextPos || Vector3.Distance(mPosition, nextPos) < 0.00000000005f) {
                        mPath.Dequeue();
                    }
                } else {
                    mMoving = false;
                }
            }
        }

        public void SetPath(List<Vector3> path) {
            mPath = path.ToQueue();
            mDestination = mPath.Last();
            mMoving = true;
        }

        public void SetDestination(Vector3 dest) {
            mDestination = dest;
            mMoving = true;
        }
    }
}
