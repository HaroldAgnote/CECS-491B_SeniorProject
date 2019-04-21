using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Items;

namespace Assets.Scripts.Model{
    public class DamageItemMoveResult : ItemMoveResult {
        private Item mUsedItem;
        private Vector2Int mAttackerPosition;
        private Vector2Int mDefenderPosition;

        private int mDamageDealt;

        public Item UsedItem => mUsedItem;
        public Vector2Int AttackerPosition => mAttackerPosition;
        public Vector2Int DefenderPosition => mDefenderPosition;

        public int DamageDealt => mDamageDealt;

        public DamageItemMoveResult(Vector2Int attackerPosition, Vector2Int defenderPosition, Item usedItem, int damageDealt) {
            mUsedItem = usedItem;
            mAttackerPosition = attackerPosition;
            mDefenderPosition = defenderPosition;
            mDamageDealt = damageDealt;
        }
    }
}
