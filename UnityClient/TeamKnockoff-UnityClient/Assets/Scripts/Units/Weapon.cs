using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string Name { get; set; }
    public int Might;
    public int Range;
    public int Hit;
    public int CritRate;

    public Weapon() {
        // TODO: Change how this is initialized
        Range = 1;
    }
}
