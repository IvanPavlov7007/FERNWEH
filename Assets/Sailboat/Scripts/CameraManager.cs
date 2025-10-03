using UnityEngine;
using Unity.Cinemachine;

namespace Sailboat
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineCamera followCamera;

        private void Update()
        {
            //if (followCamera.Target.TrackingTarget == null)
                followCamera.Target.TrackingTarget = G.Instance.player.bodyTransform;
        }
    }
}