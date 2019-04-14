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

        // Todo: Override movement methods in ObjectView to animate movement
        public override void UpdatePosition(List<Vector2Int> pathToNewPosition) {
            var mover = GameObject.GetComponent<UnitMover>();
            mover.SetPath(pathToNewPosition.Select(pos => pos.ToVector3()).ToList());
        }
    }
}
