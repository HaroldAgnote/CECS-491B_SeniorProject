using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Items
{
    interface ISelfConsumable
    {
        bool CanUse(Unit unit);
        void UseItem(Unit unit);
    }
}

