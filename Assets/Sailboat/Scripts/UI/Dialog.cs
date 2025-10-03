using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    [CreateAssetMenu(menuName = "Game/Dialog")]
    public class Dialog : ScriptableObject
    {
        public string dialogId;
        public DialogNode startNode;
    }

    [System.Serializable]
    public class DialogNode
    {
        [TextArea(2, 5)] public string text;
        public bool openInventory;
        public List<DialogChoice> choices;
        
    }

    [System.Serializable]
    public class DialogChoice
    {
        public string text;

        public Quest questToStart;
        public DialogItem requiredItem;
        public string phraseId;
        public TradeOffer tradeToOpen;

        public DialogNode nextNode;
    }

    [System.Serializable]
    public class DialogItem
    {
        public string itemId;
        public int amount;
        public bool notEmpty()
        {
            return !string.IsNullOrEmpty(itemId);
        }
    }
}