using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model{
    public class SupportItemMoveResult : ItemMoveResult {
        public enum SupportItemStatus {
            Heal,
            Buff,
            None
        }

        private Vector2Int mSupporterPosition;
        private Vector2Int mSupportedPosition;

        private SupportItemStatus mItemStatus;

        private int mDamageHealed;

        public Vector2Int SupporterPosition => mSupporterPosition;
        public Vector2Int SupportedPosition => mSupportedPosition;

        public SupportItemStatus Result => mItemStatus;

        public int DamageHealed => mDamageHealed;

        public SupportItemMoveResult(Vector2Int supporterPosition, Vector2Int supportedPosition, int damageHealed, SupportItemStatus itemStatus) {
            mSupporterPosition = supporterPosition;
            mSupportedPosition = supportedPosition;
            mDamageHealed = damageHealed;
            mItemStatus = itemStatus;
        }
    }
}
