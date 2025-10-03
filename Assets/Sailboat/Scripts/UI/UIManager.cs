using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Pixelplacement;


namespace Sailboat
{
    public class UIManager : Singleton<UIManager>
    {
        public List<ItemBase> itemBases = new List<ItemBase>();

        public void LoadAllItems()
        {
            itemBases.AddRange(Resources.LoadAll<ItemBase>("Scriptable Objects/Items/"));

        }

        public static ItemBase GetItemBase(string itemId)
        {
            var item = Instance.itemBases.Find(x => x.itemId == itemId);
            if (item == null)
                Debug.LogWarning(Instance.name + " can't find ItemBase with an id: " + itemId);
            return item;
        }

        public string stringItemSpriteTag(string itemId)
        {
            return stringItemSpriteTag(GetItemBase(itemId));
        }

        public static string stringIconAndAmount(string itemId, int amount)
        {
            var itemBase = GetItemBase(itemId);
            if (itemBase == null)
            {
                return string.Empty;
            }
            return stringIconAndAmount(itemBase, amount);
        }

        public static string stringIconAndAmount(ItemBase itemBase, int amount)
        {
            return $"{amount} x {stringItemSpriteTag(itemBase)}";
        }

        //string spriteAssetName = "UIElements"
        public static string stringItemSpriteTag(ItemBase itemBase, string spriteAssetName = "UIElements")
        {
            var iconName = itemBase.iconName;
            if (string.IsNullOrEmpty(spriteAssetName))
                return $"<sprite name=\"{iconName}\">";
            else
                return $"<sprite=\"{spriteAssetName}\" name=\"{iconName}\">";
        }

    }
}