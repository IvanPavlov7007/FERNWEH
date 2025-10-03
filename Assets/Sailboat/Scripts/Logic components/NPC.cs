using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class NPC : Interactable
    {
        public string npcName;
        public Dialog currentDefaultDialog;
        public TradeOffer currentTradeOffer;

        private void Start()
        {
            CursorSelectEnded += GameManager.Instance.interactedWithNPC;
        }
    }
}