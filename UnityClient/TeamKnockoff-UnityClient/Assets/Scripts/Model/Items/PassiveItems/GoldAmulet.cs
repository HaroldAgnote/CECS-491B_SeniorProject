using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Items
{
    public class GoldAmulet : PassiveItem
    {
        private const string ITEM_NAME = "Gold Amulet";

        public static List<FieldSkill> itemEffects = new List<FieldSkill>() { new DefenseBoost(), new AttackDrop() };

        public GoldAmulet() : base(ITEM_NAME, itemEffects)
        {

        }

        public override Item Generate()
        {
            return new GoldAmulet();
        }
    }
}