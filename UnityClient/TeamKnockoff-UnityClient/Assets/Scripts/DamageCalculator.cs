using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    
   public int GetDamage(Unit attacker, Unit defender) {
        //assume physical for now. Maybe weapon determines damage type?
        //getDistance from each unit's position
        if(attacker)
        int damageDone = attacker.Strength - defender.Defense;
        if (damageDone < 0)
        {
            return 0;
        }
        return damageDone;
        /*
        if (damageDone > 0) { //prevents from gaining health from taking negative damage
            defr.HealthPoints -= damageDone;
            if(defr.HealthPoints < 0)
            {
                defr.HealthPoints = 0;
            }
        }
        damageDone = defr.Strength - atkr.Defense;
        if (damageDone > 0) {
            atkr.HealthPoints -= damageDone;
        }   
        */
    }


    public int GetHitChance(Unit attacker, Unit defender)
    {
        double hitRate = attacker.MainWeapon.Hit;// + attacker.Skill * 0.01;
        double evasionRate = defender.Speed * 0.2 + defender.Luck * 0.1;
        return (int)(hitRate - evasionRate);
    }
    
    public int GetCritRate(Unit attacker, Unit defender)
    {
        double critRate = attacker.MainWeapon.CritRate; // + attacker.Skill * 0.01
        double evasionRate = defender.Luck * 0.3;
        return (int)(critRate - evasionRate);
    }
    
}

