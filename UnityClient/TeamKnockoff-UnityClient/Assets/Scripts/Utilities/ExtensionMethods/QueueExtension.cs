using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utilities.ExtensionMethods {
    public static class QueueExtension {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable <T> ts) {
            foreach (var element in ts) {
                queue.Enqueue(element);
            }
        }
    }
}
