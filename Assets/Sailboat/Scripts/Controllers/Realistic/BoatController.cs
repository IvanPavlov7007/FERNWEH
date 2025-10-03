using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Sailboat;

namespace Assets.Sailboat.Scripts.Controllers.Realistic
{
    public class BoatController : MonoBehaviour, IBoatController
    {
        public float steering { get; private set; }
        Boat boat;
        private void Awake()
        {
            if (boat == null)
                boat = GetComponent<Boat>();
            boat.boatController = this;
        }
        private void Update()
        {
            steering = G.Instance.playerInputController.steering;
        }
    }
}
