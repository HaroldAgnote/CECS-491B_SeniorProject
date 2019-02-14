using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class SampleUnit : Unit {
        public SampleUnit() {
            Name = "Sample Unit";
            HealthPoints = 100;
            MoveRange = 5;
            MainWeapon = new Weapon();
        }

    }
}
