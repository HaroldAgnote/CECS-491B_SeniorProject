using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities.ExtensionMethods {
    public static class ListExtension {
        public static bool IsEmpty<T>(this List<T> ts) {
            return ts.Count == 0;
        }
    }
}
