using System.Collections;
using UnityEngine;

namespace Sailboat
{
    [RequireComponent(typeof(ParallaxTransform))]
    public class ParallaxGSetter : MonoBehaviour
    {
        ParallaxTransform parallaxTransform;

        private void Start()
        {
            parallaxTransform = GetComponent<ParallaxTransform>();
            parallaxTransform.target = G.Instance.player.bodyTransform;
        }
    }
}