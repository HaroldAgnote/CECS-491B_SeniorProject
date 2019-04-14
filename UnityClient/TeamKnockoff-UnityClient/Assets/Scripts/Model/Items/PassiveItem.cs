using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Items
{
    public abstract class PassiveItem : Item
    {
        public List<FieldSkill> Effects { get; private set; }

        public PassiveItem(string name, int rarity, int buyingPrice, int sellingPrice, List<FieldSkill> mEffects)
            : base(name, rarity, buyingPrice, sellingPrice) {

            Effects = mEffects;
        }
    }
}
