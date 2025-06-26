using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongNameGameIntiface.Model
{
    internal class SexToyTriggerDuration
    {
        private float tickTimer = 0.25f;

        private SexToyFunction SexToyFunction;
        private double magnitude = 0;
        private float duration = 0f;

        public double Magnitude { get { return magnitude; } }
        public float Duration { get { return duration; } }
        public uint functionId { get { return SexToyFunction.id; } }

        public SexToyTriggerDuration(SexToyFunction sexToyFunction, double magnitude, float duration)
        {
            this.SexToyFunction = sexToyFunction;
            this.magnitude = magnitude;
            this.duration = duration;
        }

        public void TickTime() { if (duration != -1f) { duration -= tickTimer; } }

    }
}
