using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities.Generator;

namespace Assets.Scripts.Model.Items
{
    public abstract class Item : IGenerator<Item> 
    {
        private readonly string mItemName;

        private readonly int mItemRarity;

        private readonly int mBuyingPrice;
        private readonly int mSellingPrice;

        public string ItemName { get { return mItemName; } }

        public int ItemRarity { get { return mItemRarity; } }

        public int BuyingPrice { get { return mBuyingPrice; } }

        public int SellingPrice { get { return mSellingPrice; } }

        public bool IsBuyable => mBuyingPrice > 0;

        public bool IsSellable => mSellingPrice > 0;

        public Item(string name, int rarity, int buyingPrice, int sellingPrice) {
            mItemName = name;
            mItemRarity = rarity;
            mBuyingPrice = buyingPrice;
            mSellingPrice = sellingPrice;
        }

        public abstract Item Generate();
    }
}
