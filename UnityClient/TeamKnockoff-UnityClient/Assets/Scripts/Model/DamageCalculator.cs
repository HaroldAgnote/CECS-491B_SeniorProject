using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

public class DamageCalculator 
{
    public enum DamageType {
        Physical,
        Magical,
    };

    public static int GetDamage(Unit attacker, Unit defender)
    {
        //assume physical for now. Maybe weapon determines damage type?
        //getDistance from each unit's position

        //check magic
        if (attacker.MainWeapon.DamageType == DamageType.Physical) 
        {
            Debug.Log("physical");
            return GetPhysicalDamage(attacker, defender);
        }

        if (attacker.MainWeapon.DamageType == DamageType.Magical) 
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

    public static int GetOffensive(Unit attacker) {
        if (attacker.MainWeapon.DamageType == DamageType.Physical) 
        {
            return GetPhysicalOffensive(attacker);

        } else (attacker.MainWeapon.DamageType == DamageType.Magical) 
        {
            return GetMagicalOffensive(attacker);
        }

    }

    public static int GetPhysicalOffensive(Unit attacker) {
        int damageDone = attacker.MainWeapon.Might + attacker.Strength;
        return damageDone;
    }

    public static int GetMagicalOffensive(Unit attacker) {
        int damageDone = attacker.MainWeapon.Might + attacker.Magic;
        return damageDone;
    }

    public static int GetDefensive(Unit attacker, Unit defender) {
        if (attacker.MainWeapon.DamageType == DamageType.Physical) {
            return defender.Defense;
        } else {
            return defender.Resistance;
        }
    }

    public static int GetHitChance(Unit attacker, Unit defender)
    {
        double hitRate = attacker.MainWeapon.Hit;// + attacker.Skill * 0.01;
        double evasionRate = defender.Speed + defender.Luck;
        int hit = (int)(hitRate - evasionRate);
        if (hit > 0)
            return hit;
        return 0;
    }

    public static int GetCritRate(Unit attacker, Unit defender)
    {
        double critRate = attacker.MainWeapon.CritRate; // + attacker.Skill * 0.01
        double evasionRate = defender.Luck;
        int crit = (int)(critRate - evasionRate);
        if (crit > 0)
            return crit;
        return 0;
    }

    public static int GetSkillDamage(Unit attacker, Unit defender, Skill s)
    {
        //assume physical for now. Maybe weapon determines damage type?
        //getDistance from each unit's position

        //check magic
        SingleDamageSkill b = (SingleDamageSkill) s;
        if (b.DamageType == DamageType.Physical)
        {
            Debug.Log("physical");
            return GetPhysicalSkillDamage(attacker, defender, b);
        }

        if (b.DamageType == DamageType.Magical)
        {
            Debug.Log("magical");
            return GetMagicalSkillDamage(attacker, defender, b);
        }

        return 0;
        //we gotta do some polymorphism shit on these boy-toys when we get other skills maybe
    }

    public static int GetPhysicalSkillDamage(Unit attacker, Unit defender, SingleDamageSkill s)
    {
        int damageDone = s.GetDamage(attacker, defender);
        if (damageDone <= 0)
        {
            return 1;
        }
        return damageDone;
    }

    public static int GetMagicalSkillDamage(Unit attacker, Unit defender, Skill s)
    {
        int damageDone = attacker.MainWeapon.Might + attacker.Magic - defender.Resistance;
        if (damageDone <= 0)
        {
            return 1;
        }
        return damageDone;
    }


    public static int GetSuccessRate(Unit attacker, Unit defender, Skill s)
    {
        //we gotta do some polymorphism shit on these boy-toys when we get other skills maybe
        SingleDamageSkill sd = s as SingleDamageSkill;
        int hit = sd.GetHitChance(attacker, defender);
        return hit;

    }

    public static bool DiceRoll(int hitChance)
    {
        int randHit = UnityEngine.Random.Range(0, 100);
        Debug.Log($"hitChance: {hitChance} \t randHit: {randHit} ");
        return (hitChance > randHit);
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




