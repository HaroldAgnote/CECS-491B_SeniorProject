using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Assets.Scripts.UtilityMethods {
    public class UtilityMethods {
        // Simple check to see if passed in type is subclass or same class
        public static bool IsSameOrSubClass(Type potentialBase, Type potentialDescendant) {
            return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
        }
    }
}
