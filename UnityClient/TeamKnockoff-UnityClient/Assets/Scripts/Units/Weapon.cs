using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;
using Assets.Scripts;

public class Weapon
{

    public string Name { get; set; }
    public int Might { get; set; }
    public int Range { get; set; }
    public int Hit { get; set; }
    public int CritRate { get; set; }
    public DamageCalculator.DamageType DamageType { get; set; }

    public Weapon() {
        // TODO: Change how weapons are initialized
        Range = 1;
    }

    public Weapon(int m, int r, int h, int c, DamageCalculator.DamageType d) {
        // TODO: Change how this is initialized
        Might = m;
        Range = r;
        Hit = h;
        CritRate = c;
        DamageType = d;
    }
}
