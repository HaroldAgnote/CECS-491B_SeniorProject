using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Model.Items {
    [Serializable]
    public class ItemWrapper {
        [SerializeField]
        private string mItemName;

        public string ItemName { get { return mItemName; } }

        public ItemWrapper(Item item) {
            mItemName = item.ItemName;
        }
    }
}
