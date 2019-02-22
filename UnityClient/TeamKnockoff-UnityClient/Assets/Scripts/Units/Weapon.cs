using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string Name { get; set; }
    public int Might { get; set; }
    public int Range { get; set; }
    public int Hit { get; set; }
    public int CritRate { get; set; }

    public Weapon() {
        // TODO: Change how weapons are initialized
        Range = 1;
    }
}
