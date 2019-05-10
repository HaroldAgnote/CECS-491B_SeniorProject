using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model {
    public class DamageCalculator 
    {
        public enum DamageType {
            Physical,
            Magical,
        };

        /// <summary>
        /// gets the damage done by the attacker to the defender
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <returns> the damage done </returns>
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

        /// <summary>
        /// gets the critical damage done by the attacker to the defender
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <returns> the cirtical damage done to the defending unit </returns>
        public static int GetCritDamage(Unit attacker, Unit defender)
        {
            int critMultiplier = 3;
            //assume physical for now. Maybe weapon determines damage type?
            //getDistance from each unit's position

            //check magic
            if (attacker.MainWeapon.DamageType == DamageType.Physical)
            {
                Debug.Log("physical");
                return critMultiplier * GetPhysicalDamage(attacker, defender);
            }

            if (attacker.MainWeapon.DamageType == DamageType.Magical)
            {
                return critMultiplier * GetMagicalDamage(attacker, defender);
            }

            return 0;
        }

        /// <summary>
        /// gets the physical damage done to the defender by the attacker
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <returns> the physical damage done </returns>
        public static int GetPhysicalDamage(Unit attacker, Unit defender)
        {
            int damageDone = attacker.Strength.Value - defender.Defense.Value;
            if (damageDone <= 0)
            {
                return 1;
            }
            return damageDone;
        }

        /// <summary>
        /// gets the magical damage done to the defender by the attacker
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <returns> the magical damage done </returns>
        public static int GetMagicalDamage(Unit attacker, Unit defender)
        {
            int damageDone = attacker.Magic.Value - defender.Resistance.Value;
            if (damageDone <= 0)
            {
                return 1;
            }
            return damageDone;
        }

        /// <summary>
        /// gets the offensive stat of the unit
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <returns> offsneisve stat of unit </returns>
        public static int GetOffensive(Unit attacker) {
            if (attacker.MainWeapon.DamageType == DamageType.Physical) 
            {
                return GetPhysicalOffensive(attacker);

            } else
            {
                return GetMagicalOffensive(attacker);
            }

        }

        /// <summary>
        /// gets the physical offsensive stat for the attacker
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <returns> the physical offensive stat </returns>
        public static int GetPhysicalOffensive(Unit attacker) {
            int damageDone = attacker.Strength.Value;
            return damageDone;
        }

        /// <summary>
        /// gets the magical offsensive stat for the attacker
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <returns> the magical offensive stat </returns>
        public static int GetMagicalOffensive(Unit attacker) {
            int damageDone = attacker.Magic.Value;
            return damageDone;
        }

        /// <summary>
        /// gets defensive stat of the defending unit
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <returns> the defensive stat for the unit </returns>
        public static int GetDefensive(Unit attacker, Unit defender) {
            if (attacker.MainWeapon.DamageType == DamageType.Physical) {
                return defender.Defense.Value;
            } else {
                return defender.Resistance.Value;
            }
        }

        /// <summary>
        /// gets the hit chance of the attacker against the defender
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <returns> the hit chance </returns>
        public static int GetHitChance(Unit attacker, Unit defender)
        {
            double hitRate = attacker.MainWeapon.HitRate;// + attacker.Skill * 0.01;
            double evasionRate = defender.Speed.Value + defender.Luck.Value;
            int hit = (int)(hitRate - evasionRate);
            if (hit > 0)
                return hit;
            return 0;
        }

        /// <summary>
        /// gets the crit rate of a unit
        /// </summary>
        /// <param name="attacker"> the attacking unit </param>
        /// <param name="defender"> the defending unit </param>
        /// <returns> the chance of crit of the attacking unit against the defending unit </returns>
        public static int GetCritRate(Unit attacker, Unit defender)
        {
            double critRate = attacker.MainWeapon.CritRate; // + attacker.Skill * 0.01
            double evasionRate = defender.Luck.Value;
            int crit = (int)(critRate - evasionRate);
            if (crit > 0)
                return crit;
            return 0;
        }
        /*
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
            
        */

        /// <summary>
        /// Gets the physical damage off a skill
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <param name="s"> the physical skill being used </param>
        /// <returns> the physical damange being dealt to the defender </returns>
        public static int GetPhysicalSkillDamage(Unit attacker, Unit defender, SingleDamageSkill s)
        {
            int damageDone = s.GetDamage(attacker, defender);
            if (damageDone <= 0) {
                return 1;
            }
            return damageDone;
        }

        /// <summary>
        /// Gets the magic damage off a skill
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <param name="s"> the magic skill being used </param>
        /// <returns> the magic damange being dealt to the defender </returns>
        public static int GetMagicalSkillDamage(Unit attacker, Unit defender, Skill s)
        {
            int damageDone = attacker.Magic.Value - defender.Resistance.Value;
            if (damageDone <= 0) {
                return 1;
            }
            return damageDone;
        }

        /// <summary>
        /// gets the success rate of attacker using Skill on defender
        /// </summary>
        /// <param name="attacker"> attacking unit </param>
        /// <param name="defender"> defending unit </param>
        /// <param name="s"> Skill in use </param>
        /// <returns> the success rate </returns>
        public static int GetSuccessRate(Unit attacker, Unit defender, Skill s)
        {
            //we gotta do some polymorphism shit on these boy-toys when we get other skills maybe
            SingleDamageSkill sd = s as SingleDamageSkill;
            int hit = sd.GetHitChance(attacker, defender);
            return hit;

        }

        /// <summary>
        /// Random number generator of game
        /// </summary>
        /// <param name="hitChance"> the chance the unit can hit the other unit </param>
        /// <returns> true if hit, false if miss </returns>
    public static bool DiceRoll(int hitChance)
    {
        int randHit = UnityEngine.Random.Range(0, 100);
        Debug.Log($"SuccessRate: {hitChance} \t randVal: {randHit} ");
        return (hitChance > randHit);
    }

    /*
    public static int GetPhysicalDamage(Unit attacker, Unit defender, Skill s)
    {
        int damageDone = attacker.MainWeapon.Might + attacker.Strength - defender.Defense;
        if (damageDone <= 0)
        {
            int randHit = UnityEngine.Random.Range(0, 100);
            Debug.Log($"SuccessRate: {hitChance} \t randVal: {randHit} ");
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




}
