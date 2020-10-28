using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stats;


namespace ItemSystem
{
    public class TempPerTimeModifier : Modifier
    {
        private float Duration;
        public float delay = 1;

        public TempPerTimeModifier(string name, string desc, Dictionary<string, float> changers, float duration) : base(name, desc, changers)
        {
            this.Duration = duration;
        }

        public TempPerTimeModifier(string name, string desc, Dictionary<string, float> changers, float duration, ModifierTo type) : base(name, desc, changers, type)
        {
            this.Duration = duration;
        }

        public TempPerTimeModifier(string name, string desc, Dictionary<string, float> changers, float duration, ModifierTo type, List<Modifier> postEffects) : base(name, desc, changers, type, postEffects)
        {
            this.Duration = duration;
        }

        public IEnumerator Use(CharacerterStats receiver, StopUsingCallback callback)
        {
            float time = Duration;
            while (time > 0)
            {
                foreach (var changer in Changers)
                {
                    Stat currentPropVal = (Stat)receiver.GetType().GetProperty(changer.Key).GetValue(receiver);
                    currentPropVal.StatValue += changer.Value;
                    receiver.GetType().GetProperty(changer.Key).SetValue(receiver, currentPropVal);
                }
                time -= 1;
                yield return new WaitForSeconds(delay);
            }
            
            callback(this);
        }
    }
}
