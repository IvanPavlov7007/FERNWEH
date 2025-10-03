using System.Collections;
using UnityEngine;

namespace Assets.Sailboat.Scripts
{
    [ExecuteInEditMode]
    public class SortingLayerZaxis : MonoBehaviour
    {
        Renderer renderer;
        void Start()
        {
            renderer = GetComponent<Renderer>();
        }

        void Update()
        {
            renderer.sortingOrder = (int)-transform.position.z;
        }
    }
}