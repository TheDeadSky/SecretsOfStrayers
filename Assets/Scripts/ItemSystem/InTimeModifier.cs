using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character.Stats;

namespace ItemSystem
{
    public class InTimeModifier : Modifier
    {
        public float duration;
        private string property;
        private float value;

        public InTimeModifier(string name, string desc, Dictionary<string, float> changers, float duration) : base(name, desc, changers)
        {
            property = changers.ElementAt(0).Key;
            value = changers.ElementAt(0).Value;
            this.duration = duration;
        }

        public InTimeModifier(string name, string desc, Dictionary<string, float> changers, float duration, ModifierTo type) : base(name, desc, changers, type)
        {
            property = changers.ElementAt(0).Key;
            value = changers.ElementAt(0).Value;
            this.duration = duration;
        }

        public InTimeModifier(string name, string desc, Dictionary<string, float> changers, float duration, ModifierTo type, List<Modifier> postEffects) : base(name, desc, changers, type, postEffects)
        {
            property = changers.ElementAt(0).Key;
            value = changers.ElementAt(0).Value;
            this.duration = duration;
        }

        public IEnumerator Use(CharacerterStats receiver, StopUsingCallback callback)
        {
            owner = receiver;
            float currentAdded = 0;
            while (Mathf.Abs(currentAdded) < Mathf.Abs(value))
            {
                float dtime = Time.deltaTime;
                Stat currentPropVal = (Stat)receiver.GetType().GetProperty(property).GetValue(receiver);
                currentPropVal.StatValue += (value * dtime) / duration;
                currentAdded += value * dtime / duration;

                receiver.GetType().GetProperty(property).SetValue(receiver, currentPropVal);

                yield return new WaitForSeconds(dtime / duration);
            }

            callback(this);
        }
    }
}
