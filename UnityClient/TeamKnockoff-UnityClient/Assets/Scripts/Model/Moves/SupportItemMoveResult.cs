using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Items;

namespace Assets.Scripts.Model{
    public class SupportItemMoveResult : ItemMoveResult {
        public enum SupportItemStatus {
            Heal,
            Buff,
            None
        }

        private Item mUsedItem;
        private Vector2Int mSupporterPosition;
        private Vector2Int mSupportedPosition;

        private SupportItemStatus mItemStatus;

        private int mDamageHealed;

        public Item UsedItem => mUsedItem;
        public Vector2Int SupporterPosition => mSupporterPosition;
        public Vector2Int SupportedPosition => mSupportedPosition;

        public SupportItemStatus Result => mItemStatus;

        public int DamageHealed => mDamageHealed;

        public SupportItemMoveResult(Vector2Int supporterPosition, Vector2Int supportedPosition, Item usedItem, int damageHealed, SupportItemStatus itemStatus) {
            mUsedItem = usedItem;
            mSupporterPosition = supporterPosition;
            mSupportedPosition = supportedPosition;
            mDamageHealed = damageHealed;
            mItemStatus = itemStatus;
        }
    }
}
