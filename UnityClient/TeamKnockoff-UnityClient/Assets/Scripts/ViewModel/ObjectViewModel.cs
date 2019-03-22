using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.ViewModel {
    public abstract class ObjectViewModel {
        public GameObject GameObject { get; protected set; }

        public void UpdatePosition(Vector2Int newPosition) {
            GameObject.transform.position = newPosition.ToVector3();
        }
    }
}
