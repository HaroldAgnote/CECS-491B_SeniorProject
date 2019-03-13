using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.ViewModel {
    public class UnitViewModel : ObjectViewModel {
        public Unit Unit { get; }

        public UnitViewModel(GameObject gameObject, Unit unit) {
            GameObject = gameObject;
            Unit = unit;
        }        
    }
}
