﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Model.Units {
        [Serializable]
        public class Stat {
            [SerializeField]
            private int mBase;

            [SerializeField]
            private int mModifier;

            public int Base {
                get {
                    return mBase;
                }

                protected internal set {
                    if (mBase != value) {
                        mBase = value;
                    }
                }
            }

            public int Modifier {
                get {
                    return mModifier;
                }

                protected internal set {
                    if (mModifier != value) {
                        mModifier = value;
                    }
                }
            }

            public Stat() {
                Base = 0;
                Modifier = 0;
            }

            public Stat(int baseStat) {
                Base = baseStat;
                Modifier = 0;
            }

            public Stat(int baseStat, int initialModifier) {
                Base = baseStat;
                Modifier = initialModifier;
            }

            public int Value => Base + Modifier;

            public override string ToString() {
                if (Modifier == 0) {
                    return $"{Base}";
                } else if (Modifier > 0) {
                    return $"{Base} +{Modifier}";
                } else {
                    return $"{Base} {Modifier}";
                }
            }

        }

}
