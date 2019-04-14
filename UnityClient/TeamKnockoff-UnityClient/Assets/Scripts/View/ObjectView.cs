using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.View {
    public abstract class ObjectView {

        public ObjectView(GameObject gameObject) {
            GameObject = gameObject;
        }

        public GameObject GameObject { get; protected set; }
        
        public virtual void UpdatePosition(Vector2Int newPosition) {
            GameObject.transform.position = newPosition.ToVector3();
        }

        public virtual void UpdatePosition(List<Vector2Int> pathToNewPosition) {
            throw new NotImplementedException();
        }
    }
}
