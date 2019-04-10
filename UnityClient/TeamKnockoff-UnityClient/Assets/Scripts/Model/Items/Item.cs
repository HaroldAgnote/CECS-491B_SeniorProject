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

        public string ItemName { get { return mItemName; } }

        public Item(string name) {
            mItemName = name;
        }

        public abstract Item Generate();
    }
}
