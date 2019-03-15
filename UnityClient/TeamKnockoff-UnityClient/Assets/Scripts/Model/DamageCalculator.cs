using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;

public class DamageCalculator 
{
    public enum DamageType { Physical, Magical };

    public static int GetDamage(Unit attacker, Unit defender)
    {
        //assume physical for now. Maybe weapon determines damage type?
        //getDistance from each unit's position

        //check magic
        if (attacker.MainWeapon.DamageType == DamageType.Physical) //Matthew added (int) cast
        {
            Debug.Log("physical");
            return GetPhysicalDamage(attacker, defender);
        }

        if (attacker.MainWeapon.DamageType == DamageType.Magical) //Matthew added (int) cast
        {
            return GetMagicalDamage(attacker, defender);
        }

        return 0;
    }

    public static int GetPhysicalDamage(Unit attacker, Unit defender)
    {
        int damageDone = attacker.MainWeapon.Might + attacker.Strength - defender.Defense;
        if (damageDone <= 0)
        {
            return 1;
        }
        return damageDone;
    }

    public static int GetMagicalDamage(Unit attacker, Unit defender)
    {
        int damageDone = attacker.MainWeapon.Might + attacker.Magic - defender.Resistance;
        if (damageDone <= 0)
        {
            return 1;
        }
        return damageDone;
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

    public static void ApplySkill(Unit attacker, Unit defender, Skill s)
    {
        attacker.Speed = 4;
        //we gotta do some polymorphism shit on these boy-toys when we get other skills maybe
    }

    public static int GetSuccessRate(Unit attacker, Unit defender, Skill s)
    {
        //we gotta do some polymorphism shit on these boy-toys when we get other skills maybe
        SingleDamage sd = s as SingleDamage;
        int hit = sd.GetHitChance(attacker, defender);
        return hit;

    }
    /*
    public static int GetPhysicalDamage(Unit attacker, Unit defender, Skill s)
    {
        int damageDone = attacker.MainWeapon.Might + attacker.Strength - defender.Defense;
        if (damageDone <= 0)
        {
            return 1;
        }
        return damageDone;
    }

    public static int GetMagicalDamage(Unit attacker, Unit defender, Skill s)
    {
        int damageDone = attacker.MainWeapon.Might + attacker.Magic - defender.Resistance;
        if (damageDone <= 0)
        {
            return 1;
        }
        return damageDone;
    }

    public static int GetHitChance(Unit attacker, Unit defender, Skill s)
    {
        double hitRate = attacker.MainWeapon.Hit;// + attacker.Skill * 0.01;
        double evasionRate = defender.Speed + defender.Luck;
        return (int)(hitRate - evasionRate);
    }

    public static int GetCritRate(Unit attacker, Unit defender, Skill s)
    {
        double critRate = attacker.MainWeapon.CritRate; // + attacker.Skill * 0.01
        double evasionRate = defender.Luck;
        return (int)(critRate - evasionRate);
    }
    */

}




