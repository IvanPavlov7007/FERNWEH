using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat.Misc
{
    public class SimpleTweenPosition : MonoBehaviour
    {
        public Vector2 endPos;
        public Vector2 startPos;
        public Tween.LoopType loopType;
        public float duration;
        public float delay;
        protected AnimationCurve curve = Tween.EaseIn;
        protected virtual void OnEnable()
        {
            Tween.LocalPosition(transform, startPos, endPos, duration, delay, curve, loop: loopType);
        }
    }
}