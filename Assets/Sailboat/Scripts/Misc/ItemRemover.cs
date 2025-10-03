using System.Collections;
using UnityEngine;

namespace Assets.Sailboat.Scripts.Misc
{
    [RequireComponent(typeof(StayArea))]
    public class ItemRemover : MonoBehaviour
    {
        public string itemId;
        public int amount;
    }
}