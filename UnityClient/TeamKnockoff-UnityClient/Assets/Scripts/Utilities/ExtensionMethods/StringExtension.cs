using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities.ExtensionMethods {
    public static class StringExtension {
        public static bool IsEmpty(this String stringVal) {
            return stringVal.Trim().Length == 0;
        }
    }
}
