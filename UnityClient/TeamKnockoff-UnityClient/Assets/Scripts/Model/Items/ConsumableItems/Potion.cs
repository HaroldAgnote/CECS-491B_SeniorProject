using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Items
{
    public class Potion : ConsumableItem, ITargetConsumable, ISelfConsumable
    {
        private const string ITEM_NAME = "Potion";
        private const int RANGE = 1;
        private const int HEAL_VALUE = 30;

        public Potion() : base(ITEM_NAME)
        {

        }

        public bool CanUse(Unit unit)
        {
            return unit.HealthPoints != unit.MaxHealthPoints.Value;
        }

        public bool CanUseOn(Unit usingUnit, Unit targetUnit)
        {
            return targetUnit.HealthPoints != targetUnit.MaxHealthPoints.Value;
        }

        public void UseItem(Unit unit)
        {
            if(CanUse(unit))
            {
                if((unit.HealthPoints + HEAL_VALUE) > unit.MaxHealthPoints.Value)
                {
                    unit.HealthPoints = unit.MaxHealthPoints.Value;
                }
                else
                {
                    unit.HealthPoints += HEAL_VALUE;
                }
            }
        }

        public void UseItemOn(Unit usingUnit, Unit targetUnit)
        {
            if (CanUseOn(usingUnit, targetUnit))
            {
                if ((targetUnit.HealthPoints + HEAL_VALUE) > targetUnit.MaxHealthPoints.Value)
                {
                    targetUnit.HealthPoints = targetUnit.MaxHealthPoints.Value;
                }
                else
                {
                    targetUnit.HealthPoints += HEAL_VALUE;
                }
            }
        }

        public int GetRange()
        {
            return RANGE;
        }

        public override Item Generate()
        {
            return new Potion();
        }
    }
}

