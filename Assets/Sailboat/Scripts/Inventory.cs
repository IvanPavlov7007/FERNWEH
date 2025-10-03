using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Game/Inventory")]
    public class Inventory : ScriptableObject
    {
        public Dictionary<string, int> items = new Dictionary<string, int>();

        public void Reset()
        {
            items.Clear();
        }
        public void AddItem(string itemId, int amount, bool triggerEvent = true)
        {
            if (!items.ContainsKey(itemId))
                items.Add(itemId, 0);
            items[itemId] += amount;

            if (triggerEvent)
            {
                GameEvents.ItemCollected(itemId, amount);
            }
        }

        public bool HasItem(string itemId, int amount = 1)
        {
            if (!items.ContainsKey(itemId))
                return false;
            return items[itemId] >= amount;
        }

        public int Amount(string itemId)
        {
            return items[itemId];
        }

        public bool RemoveItem(string itemId, int amount = 1, bool triggerEvent = true)
        {
            if (!items.ContainsKey(itemId) || items[itemId] < amount)
                return false;
            items[itemId] -= amount;
            if (triggerEvent)
            {
                GameEvents.ItemRemoved(itemId, amount);
            }
            return true;
        }
    }
}