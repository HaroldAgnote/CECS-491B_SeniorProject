using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

public abstract class SingleDamage : Skill
{
    public abstract int GetDamage(Unit attacker, Unit defender);
    public abstract int GetHitChance(Unit attacker, Unit defender);
    public abstract int GetCritRate(Unit attacker, Unit defender);
}

