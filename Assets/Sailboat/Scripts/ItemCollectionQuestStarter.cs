using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class ItemCollectionQuestStarter : MonoBehaviour
    {

        public List<QuestTask> allTasks;
        private void OnEnable()
        {
            GameEvents.Instance.OnItemCollected += HandleItemCollected;
        }

        private void OnDisable()
        {
            GameEvents.Instance.OnItemCollected -= HandleItemCollected;
        }

        private void HandleItemCollected(string itemId, int amount)
        {
            //foreach (var itemQuest in itemQuests)
            //{
            //    if(itemId == itemQuest.itemId &&)
            //}
        }
    }
}