using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Items
{
    interface ITargetConsumable
    {
        int GetRange();
        bool CanUseOn(Unit usingUsing, Unit targetUnit);
        void UseItemOn(Unit usingUsing, Unit targetUnit);
    }
}
