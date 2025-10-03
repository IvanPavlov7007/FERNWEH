using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class PeriodicWindArea : WindArea
    {
        public float delayTimer;
        public float transitionTime;
        public List<PeriodicWind> periods;


        SimpleTimer currentPeriodTimer, transitionTimer;
        int currentPeriod = 0;
        Vector2 aWind, bWind;

        Vector2 currentWindSpeed = Vector2.zero;

        private void Start()
        {
            currentWindSpeed = periods[currentPeriod].speed;
            currentPeriodTimer = new SimpleTimer(periods[currentPeriod].time);
        }

        private void Update()
        {
            if (currentPeriodTimer != null)
            {
                if (currentPeriodTimer.tick(Time.deltaTime))
                {
                    transitionTimer = new SimpleTimer(transitionTime);
                    currentPeriodTimer = null;
                    aWind = currentWindSpeed;
                    if (++currentPeriod >= periods.Count) // starts from the first one
                    {
                        currentPeriod = 0;
                    }
                    bWind = periods[currentPeriod].speed;
                }
                else
                {
                    //during the period
                }
            }

            if (transitionTimer != null)
            {
                if (transitionTimer.tick(Time.deltaTime))
                {
                    transitionTimer = null;
                    currentPeriodTimer = new SimpleTimer(periods[currentPeriod].time);
                    currentWindSpeed = periods[currentPeriod].speed;
                }
                else
                {
                    //during the transition
                    currentWindSpeed = Vector3.Slerp(aWind, bWind, transitionTimer.currentTime / transitionTimer.timeoutTime.Value);
                }
            }
        }

        public override Vector2 getWindSpeed(Vector2 worldPos)
        {
            return currentWindSpeed;
        }

        [System.Serializable]
        public struct PeriodicWind
        {
            public Vector2 speed;
            public float time;
        }
    }
}