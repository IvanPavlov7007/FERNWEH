using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat
{
    public class SimpleAnimation : MonoBehaviour
    {
        [System.Serializable]
        public class TweenOptions
        {
            public TweenFunction tweenFunction;
            public float duration;
            public float delay;
            public Vector3 value;
        }

        [System.Serializable]
        public enum TweenFunction { Rotate,Shake,Scale}

        public TweenOptions[] options;

        private void Start()
        {
            foreach(var opt in options)
            {
                switch (opt.tweenFunction)
                {
                    case TweenFunction.Rotate:
                        Tween.Rotate(transform, opt.value, Space.Self, opt.duration, opt.delay, loop: Tween.LoopType.PingPong);
                        break;
                    case TweenFunction.Shake:
                        Tween.Shake(transform,Vector3.zero,opt.value,opt.duration,opt.delay,loop:Tween.LoopType.PingPong);
                        break;
                    case TweenFunction.Scale:
                        Tween.LocalScale(transform, opt.value, opt.duration, opt.delay, loop: Tween.LoopType.PingPong);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}