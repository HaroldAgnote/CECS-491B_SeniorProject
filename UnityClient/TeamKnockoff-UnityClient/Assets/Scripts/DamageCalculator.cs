using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;

public class DamageCalculator
{
    
   public static int GetDamage(Unit attacker, Unit defender) {
        //assume physical for now. Maybe weapon determines damage type?
        //getDistance from each unit's position
        int damageDone = attacker.MainWeapon.Might + attacker.Strength - defender.Defense;
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


    public static int GetHitChance(Unit attacker, Unit defender)
    {
        double hitRate = attacker.MainWeapon.Hit;// + attacker.Skill * 0.01;
        double evasionRate = defender.Speed + defender.Luck;
        return (int)(hitRate - evasionRate);
    }
    
    public static int GetCritRate(Unit attacker, Unit defender)
    {
        double critRate = attacker.MainWeapon.CritRate; // + attacker.Skill * 0.01
        double evasionRate = defender.Luck;
        return (int)(critRate - evasionRate);
    }
    
}

