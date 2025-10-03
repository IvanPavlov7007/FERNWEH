using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Sailboat;

namespace Assets.Sailboat.Scripts.Controllers.Realistic
{
    public class SailController : MonoBehaviour, ISailController
    {
        public bool Tightening{ get; private set; }

        public bool Loosening { get; private set; }
        Sail sail;
        private void Awake()
        {
            Sail sail = GetComponent<Sail>();
            sail.sailController = this;
        }

        private void Update()
        {
            Tightening = G.Instance.playerInputController.tightenSails;
            Loosening = G.Instance.playerInputController.loosenSails;
        }
    }
}
