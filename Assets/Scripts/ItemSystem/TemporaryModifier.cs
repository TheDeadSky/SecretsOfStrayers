using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stats;

namespace ItemSystem
{
    public class TemporaryModifier : Modifier
    {
        
        public readonly float Duration;

        public TemporaryModifier(string name, string desc, Dictionary<string, float> changers) : base(name, desc, changers)
        {
        }

        public TemporaryModifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type) : base(name, desc, changers, type)
        {
        }

        public TemporaryModifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type, List<Modifier> postEffects) : base(name, desc, changers, type, postEffects)
        {
        }

        public IEnumerator Use(CharacerterStats receiver, StopUsingCallback callback)
        {
            owner = receiver;
            foreach (var changer in Changers)
            {
                Stat currentPropVal = (Stat)receiver.GetType().GetProperty(changer.Key).GetValue(receiver);
                currentPropVal.StatValue += changer.Value;
                receiver.GetType().GetProperty(changer.Key).SetValue(receiver, currentPropVal);
            }

            yield return new WaitForSeconds(Duration);

            callback(this);
        }
    }
}
