using System.Collections;
using UnityEngine;
using System;

namespace Sailboat
{
    public class ReachTarget : MonoBehaviour
    {
        [SerializeField]
        StayArea stayArea;

        public event Action<ReachTarget> areaReached;

        private void OnEnable()
        {
            stayArea.onStay.AddListener(onStay);
        }

        private void OnDisable()
        {
            stayArea.onStay.RemoveListener(onStay);
        }

        private void onStay(Entity entity)
        {
            if (Player.Instance.entityIsPlayer(entity))
            {
                areaReached?.Invoke(this);
            }
        }
    }
}