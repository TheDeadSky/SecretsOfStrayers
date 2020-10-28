using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stats;


namespace ItemSystem
{
    public class PermPerTimeModifier : Modifier
    {
        private float delay = 1;
        public bool Working = true;

        public PermPerTimeModifier(string name, string desc, Dictionary<string, float> changers) : base(name, desc, changers)
        {
        }

        public PermPerTimeModifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type) : base(name, desc, changers, type)
        {
        }

        public PermPerTimeModifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type, List<Modifier> postEffects) : base(name, desc, changers, type, postEffects)
        {
        }

        public IEnumerator Use(CharacerterStats receiver, StopUsingCallback callback)
        {
            owner = receiver;
            while (Working)
            {
                foreach (var changer in Changers)
                {
                    Stat currentPropVal = (Stat)receiver.GetType().GetProperty(changer.Key).GetValue(receiver);
                    currentPropVal.StatValue += changer.Value * Time.deltaTime;
                    receiver.GetType().GetProperty(changer.Key).SetValue(receiver, currentPropVal);
                }
                yield return new WaitForSeconds(delay * Time.deltaTime);
            }

            callback(this);
        }

    }
}
