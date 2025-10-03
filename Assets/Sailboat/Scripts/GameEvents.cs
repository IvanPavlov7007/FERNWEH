using System.Collections;
using UnityEngine;
using Pixelplacement;
using System;

namespace Sailboat
{
    public class GameEvents : Singleton<GameEvents>
    {
        public event Action<string> OnLocationReached;
        public event Action<string, int> OnItemCollected;
        public event Action<string, int> OnItemLost;
        public event Action<string> OnNPCInteractedWith;
        public event Action<string> OnPhraseSeen;
        public event Action<string> OnProgressionStateReached;

        public static void LocationReached(string locationId)
            => Instance.OnLocationReached?.Invoke(locationId);

        public static void ItemCollected(string itemId, int amount)
            => Instance.OnItemCollected?.Invoke(itemId, amount);

        public static void ItemRemoved(string itemId, int amount)
            => Instance.OnItemLost?.Invoke(itemId, amount);

        public static void NPCInteractedWith(string npcId)
            => Instance.OnNPCInteractedWith?.Invoke(npcId);

        public static void PhraseSeen(string phraseId)
            => Instance.OnPhraseSeen?.Invoke(phraseId);

        public static void ProgressionReaced(string progressionId)
            => Instance.OnProgressionStateReached?.Invoke(progressionId);
    }
}