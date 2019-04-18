using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities.ExtensionMethods {
    public static class IEnumerableExtension {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> ts) {
            HashSet<T> hashSet = new HashSet<T>();
            foreach (var element in ts) {
                hashSet.Add(element);
            }
            return hashSet;
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> ts) {
            Queue<T> queue = new Queue<T>();
            foreach (var element in ts) {
                queue.Enqueue(element);
            }
            return queue;
        }
    }
}
