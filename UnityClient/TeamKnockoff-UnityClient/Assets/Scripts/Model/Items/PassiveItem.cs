using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model.Items
{
    public abstract class PassiveItem : Item
    {
        public List<FieldSkill> Effects { get; private set; }

        public PassiveItem(string name, List<FieldSkill> mEffects) : base(name)
        {
            Effects = mEffects;
        }
        
        // TODO: Sebastian
        // PassiveItems now need to have a FieldSkill Field/Property
    }
}
