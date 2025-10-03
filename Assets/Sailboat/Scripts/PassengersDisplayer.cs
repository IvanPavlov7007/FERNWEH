using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class PassengersDisplayer : MonoBehaviour
    {
        public Transform[] sits;
        public GameObject prefab;
        public string personItemId;

        public List<GameObject> currentPassengers = new List<GameObject>();

        private void OnEnable()
        {
            GameEvents.Instance.OnItemCollected += onItemAdded;
            GameEvents.Instance.OnItemLost += onItemRemoved;
            Redraw();
        }

        private void OnDisable()
        {
            if (GameEvents.Instance == null)
                return;
            GameEvents.Instance.OnItemCollected -= onItemAdded;
            GameEvents.Instance.OnItemLost -= onItemRemoved;
        }

        void Redraw()
        {
            clear();
            if(!G.Instance.inventory.HasItem(personItemId))
            {
                return;
            }
            int people = G.Instance.inventory.Amount(personItemId);
            int displayedCount = Mathf.Min(people, sits.Length);

            for (int i = 0; i < displayedCount; i++)
            {
                var inst = Instantiate(prefab, sits[i].position, Quaternion.identity, sits[i]);
                currentPassengers.Add(inst);
            }
        }

        void clear()
        {
            foreach (var ob in currentPassengers)
                Destroy(ob);
            currentPassengers.Clear();
        }


        void onItemAdded(string itemId, int amount)
        {
            Redraw();
        }

        void onItemRemoved(string itemId, int amount)
        {
            Redraw();
        }
    }
}