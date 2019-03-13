using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ExtensionMethods {
    public static class TypeExtension {
        public static bool IsSameOrSubClass(this Type potentialBase, Type potentialDescendant ) {
            return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
        }
    }
}
