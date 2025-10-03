using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class DestroyTracker : MonoBehaviour
    {
        public event System.Action<DestroyTracker> destroyed;
        public event System.Action<DestroyTracker> disabled;

        private void OnDestroy()
        {
            destroyed?.Invoke(this);
        }

        private void OnDisable()
        {
            disabled?.Invoke(this);
        }

        public static DestroyTracker GetTracker(GameObject obj)
        {
            DestroyTracker destroyTracker;
            if (!obj.TryGetComponent<DestroyTracker>(out destroyTracker))
            {
                destroyTracker = obj.AddComponent<DestroyTracker>();
            }
            return destroyTracker;
        }
    }
}