using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Items
{
    public class GoldAmulet : PassiveItem
    {
        private const string ITEM_NAME = "Gold Amulet";

        private const int RARITY = 3;

        private const int BUY_PRICE = 1000;
        private const int SELL_PRICE = 500;

        public static List<FieldSkill> itemEffects = new List<FieldSkill>() { new DefenseBoost(), new AttackDrop() };

        public GoldAmulet() : base(ITEM_NAME, RARITY, BUY_PRICE, SELL_PRICE, itemEffects) {

        }

        public override Item Generate()
        {
            return new GoldAmulet();
        }
    }
}