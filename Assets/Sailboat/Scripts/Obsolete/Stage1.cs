using UnityEngine;
using System;

namespace Sailboat.Obsolete
{

    public class Stage1 : MonoBehaviour
    {
        public Action onEndPierEntered;

        [SerializeField]
        StayArea pierArea;

        bool playerBodyEnteredPierArea;

        private void Start()
        {
            pierArea.onStay.AddListener(pierAchieved);
        }

        private void pierAchieved(Entity entity)
        {
            if (Player.Instance.entityIsPlayer(entity))
            {
                if (onEndPierEntered != null)
                    onEndPierEntered();
            }
            Debug.Log("Pier achieved");
        }

    }
}