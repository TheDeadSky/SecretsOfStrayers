using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Stats
{
    public class HealthStat : Stat
    {
        private float curHealth;

        public float MaxHealth 
        { 
            get { return statValue; }  
            set { statValue = value; } 
        }

        public float CurHealth 
        {
            get { return curHealth; }
            set { curHealth = value; }
        }

        public HealthStat(float curHealth, float maxHealth) : base(maxHealth)
        {
            this.curHealth = curHealth;
            this.statValue = maxHealth;
        }
    }
}

