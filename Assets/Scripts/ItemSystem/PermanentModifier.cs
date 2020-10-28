using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character.Stats;

namespace ItemSystem
{
    public class PermanentModifier : Modifier
    {
        public bool Working = true;

        public PermanentModifier(string name, string desc, Dictionary<string, float> changers) : base(name, desc, changers)
        {
        }

        public PermanentModifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type) : base(name, desc, changers, type)
        {
        }

        public PermanentModifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type, List<Modifier> postEffects) : base(name, desc, changers, type, postEffects)
        {
        }

        ~PermanentModifier()
        {
            foreach (var changer in Changers)
            {
                try
                {
                    Stat currentPropVal = (Stat)owner.GetType().GetProperty(changer.Key).GetValue(owner);
                    currentPropVal.StatValue -= changer.Value;
                    owner.GetType().GetProperty(changer.Key).SetValue(owner, currentPropVal);
                }
                catch
                {
                    continue;
                }
            }

            StartPostEffects();
        }

        public void Use(CharacerterStats receiver, StopUsingCallback callback=null)
        {
            owner = receiver;

            foreach (var changer in Changers)
            {
                try
                {
                    Stat currentPropVal = (Stat)receiver.GetType().GetProperty(changer.Key).GetValue(receiver);
                    currentPropVal.StatValue += changer.Value;
                    receiver.GetType().GetProperty(changer.Key).SetValue(receiver, currentPropVal);
                }
                catch
                {
                    continue;
                }
            }

            callback?.Invoke(this);
        }
    }
}
