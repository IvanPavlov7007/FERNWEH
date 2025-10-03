using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sailboat
{
    public class ChangingWindArea : WindArea
    {
        public Vector2[] directions;
        public int[] weights;

        public float minSwitchTime;
        public float normalSwitchTime;
        public float maxSwitchTime;

        public Vector2 currentDirection;

        public override Vector2 getWindSpeed(Vector2 worldPos)
        {
            return currentDirection;
        }

        protected virtual void Start()
        {
            StartCoroutine(changingWindRandomly());
        }

        IEnumerator changingWindRandomly()
        {
            while (true)
            {
                currentDirection = CommonTools.RandomObject(directions, weights);
                yield return new WaitForSeconds(GetRandomWithBias(minSwitchTime, normalSwitchTime, maxSwitchTime));
            }
        }

        public float GetRandomWithBias(float min, float n, float max)
        {
            // Box-Muller transform to generate a Gaussian-distributed random number
            float u1 = Random.Range(0f, 1f);
            float u2 = Random.Range(0f, 1f);

            // Calculate the standard Gaussian (mean=0, std=1)
            float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            // Adjust to your desired mean (n) and standard deviation (sigma)
            float sigma = (max - min) / 6f; // Controls how spread out the numbers are (roughly, 99.7% of values within this range)
            float randomValue = n + z * sigma;

            // Clamp the value between min and max
            return Mathf.Clamp(randomValue, min, max);
        }
    }
}