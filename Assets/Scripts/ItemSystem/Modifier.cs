using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public enum ModifierTo { self, tOwner, tOther };

    public class Modifier
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public Dictionary<string, float> Changers { get; protected set; }
        protected ModifierTo MType;
        protected List<Modifier> PostEffects;
        protected CharacerterStats owner;

        public delegate void StopUsingCallback(Modifier mod);

        public Modifier(string name, string desc, Dictionary<string, float> changers)
        {
            Name = name;
            Description = desc;
            Changers = changers;
            MType = ModifierTo.self;
            Debug.Log("Call first" + Name);
        }

        public Modifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type)
            : this(name, desc, changers)
        {
            MType = type;
            Debug.Log("Call second" + Name);
        }

        public Modifier(string name, string desc, Dictionary<string, float> changers, ModifierTo type, List<Modifier> postEffects)
            : this(name, desc, changers, type)
        {
            PostEffects = postEffects;
            Debug.Log("Call third" + Name);
        }

        public virtual void Add(string prop, float val)
        {
            if (Changers != null)
                Changers.Add(prop, val);
            else
            {
                Changers = new Dictionary<string, float>();
                Changers.Add(prop, val);
            }
        }

        public virtual void AddPostEffect(Modifier mod)
        {
            PostEffects.Add(mod);
        }

        protected virtual void StartPostEffects()
        {
            if (PostEffects != null)
            {
                foreach (var mod in PostEffects)
                {
                    owner.AddModifier(mod);
                }
            }
            Debug.Log("Destroyed" + Name);
        }
    }
}