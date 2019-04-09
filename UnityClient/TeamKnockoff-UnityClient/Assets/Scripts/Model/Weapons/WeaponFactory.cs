using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Weapons {
    class WeaponFactory {
        public static HashSet<Weapon> WeaponBank { get; }
        public static Dictionary<string, Weapon> WeaponGenerator;
        public WeaponFactory() {
            //Read file to add weapons to WeaponGenerator
        }

        public void CreateWeapon(string name) {
            //return weapon from WeaponGenerator using name as a key
        }

    }
}
