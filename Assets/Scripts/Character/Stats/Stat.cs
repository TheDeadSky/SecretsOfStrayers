using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character.Stats
{
    public class Stat
    {
        protected float statValue;

        public float StatValue 
        {
            get { return statValue; }
            set { statValue = value; }
        }

        public Stat(float statV)
        {
            statValue = statV;
        }
    }
}
