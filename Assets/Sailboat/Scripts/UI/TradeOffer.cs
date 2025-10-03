using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    [CreateAssetMenu(menuName = "Game/TradeOffer")]
    public class TradeOffer : ScriptableObject
    {
        public List<TradeEntry> tradeEntries;
    }

    [System.Serializable]
    public class TradeEntry
    {
        public string giveItemId;
        public int giveAmount;
        public string getItemId;
        public int getItemAmount;
    }
}