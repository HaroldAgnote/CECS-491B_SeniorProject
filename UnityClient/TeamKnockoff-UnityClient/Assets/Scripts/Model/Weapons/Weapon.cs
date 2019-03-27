using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Weapons {
    public class Weapon
    {

        public string Name { get; set; }
        public int Might { get; set; }
        public int Range { get; set; }
        public int Hit { get; set; }
        public int CritRate { get; set; }
        public Assets.Scripts.Model.DamageCalculator.DamageType DamageType { get; set; }

        public Weapon() {
            // TODO: Change how weapons are initialized
            Range = 1;
        }

        public Weapon(int might, int range, int hitRate, int critRate, Assets.Scripts.Model.DamageCalculator.DamageType damageType) {
            // TODO: Change how this is initialized
            Might = might;
            Range = range;
            Hit = hitRate;
            CritRate = critRate;
            DamageType = damageType;
        }
    }
}
