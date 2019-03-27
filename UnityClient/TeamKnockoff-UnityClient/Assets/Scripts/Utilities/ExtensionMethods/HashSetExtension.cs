using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities.ExtensionMethods {
    public static class HashSetExtension {
        
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable <T> ts) {
            foreach (var element in ts) {
                hashSet.Add(element);
            }
        }

        public static void RemoveRange<T>(this HashSet<T> hashSet, IEnumerable <T> ts) {
            foreach (var element in ts) {
                hashSet.Remove(element);
            }
        }
    }
}
