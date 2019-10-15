using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public enum ModifierType { self, tOwner, tOther };

    public interface IModifier
    {
        string Name { get; set; }
        string Description { get; set; }
        ModifierType Type { get; set; }
        List<IModifier> PostEffects { get; set; }

        void Use();
    }
}