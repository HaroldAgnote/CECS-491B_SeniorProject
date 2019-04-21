using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Items;

namespace Assets.Scripts.Model{
    public abstract class ItemMoveResult : GameMoveResult {
        private Item mItemUsed;

        public Item ItemUsed {
            get { return mItemUsed; }
            protected set {
                mItemUsed = value;
            }
        }
    }
}
