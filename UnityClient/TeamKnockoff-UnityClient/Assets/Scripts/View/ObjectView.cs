using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.ExtensionMethods;

namespace Assets.Scripts.View {
    public abstract class ObjectView {

        public ObjectView(GameObject gameObject) {
            GameObject = gameObject;
        }

        public GameObject GameObject { get; protected set; }
        
        public virtual void UpdatePosition(Vector2Int newPosition) {
            GameObject.transform.position = newPosition.ToVector3();
        }

        public virtual void UpdatePosition(List<Vector2Int> newPosition) {
            throw new NotImplementedException();
        }
    }
}
