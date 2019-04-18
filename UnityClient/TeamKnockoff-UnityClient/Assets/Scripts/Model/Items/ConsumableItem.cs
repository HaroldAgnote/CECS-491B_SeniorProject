using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Items
{
    public abstract class ConsumableItem : Item
    {
        public ConsumableItem(string itemName, int rarity, int buyingPrice, int sellingPrice) 
            : base(itemName, rarity, buyingPrice, sellingPrice) { }
    }
}

