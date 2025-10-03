using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sailboat
{ 
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI textmesh;

        public List<ItemBase> itemBases = new List<ItemBase>();

        private void OnEnable()
        {
            GameEvents.Instance.OnItemCollected += handleItemCollected;
            GameEvents.Instance.OnItemLost += handleItemLost;
            updateText();
        }

        private void OnDisable()
        {
            GameEvents.Instance.OnItemCollected -= handleItemCollected;
            GameEvents.Instance.OnItemLost -= handleItemLost;
        }

        void handleItemCollected(string itemId, int amount)
        {
            updateText();
        }

        void handleItemLost(string itemId, int amount)
        {
            updateText();
        }

        void updateText()
        {
            StringBuilder sb = new StringBuilder();
            var items = G.Instance.inventory.items;
            foreach (var itemId in items.Keys)
            {
                var itemBase = UIManager.GetItemBase(itemId);
                if (itemBase == null)
                {
                    Debug.LogWarning($"{name}: couldn't find an itembase for {itemId}");
                    continue;
                }

                sb.AppendLine($"{UIManager.stringIconAndAmount(itemBase,items[itemId])} {itemBase.displayName}");
            }
            textmesh.text = sb.ToString();
        }

        

    }
}