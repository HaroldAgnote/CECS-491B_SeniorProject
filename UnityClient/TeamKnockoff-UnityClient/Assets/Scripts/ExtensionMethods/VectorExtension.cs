using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ExtensionMethods {
    public static class VectorExtension {
        public static Vector2 ToVector2(this Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 ToVector3(this Vector2 vector) {
            return new Vector3(vector.x, vector.y, 0f);
        }
    }
}
