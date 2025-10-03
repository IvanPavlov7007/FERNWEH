using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class GameBootstrapper : MonoBehaviour
    {
        public QuestStartData[] questsToStart;
        public bool debug = false;

        private void Awake()
        {
            ResetStates();
            ResetInventory();
            ResetQuests();
            ResetFlags(); // Optional if you have a flag/world state system
        }

        private void ResetStates()
        {
            ProgressionManager.Instance.LoadAllStates();
            Debug.Log("[Bootstrapper] Progression states reset.");
        }

        private void DebugBootstrap()
        {
            if (!debug)
                return;
            G.Instance.inventory.AddItem("coin", 20);
            G.Instance.inventory.AddItem("wood", 4);
            G.Instance.inventory.AddItem("normal_fish", 10);
            foreach (var baseItem in G.Instance.inventoryUI.itemBases)
            {
                G.Instance.inventory.AddItem(baseItem.itemId, Random.Range(1,999));
            }
        }

        private void ResetInventory()
        {
            UIManager.Instance.LoadAllItems();
            G.Instance.inventory.Reset();
            Debug.Log("[Bootstrapper] Inventory reset.");
        }

        private void ResetQuests()
        {
            QuestManager.Instance.LoadAllQuests();
            QuestManager.Instance.ResetAllQuests();
            Debug.Log("[Bootstrapper] Quests reset.");
        }

        private void Start()
        {
            startQuests();
#if UNITY_EDITOR
            DebugBootstrap();
#endif
        }

        private void ResetFlags()
        {
            // If you use a WorldState/Flag system, reset here
            // Example: WorldState.Instance.ResetAll();
        }

        private void startQuests()
        {
            foreach (var data in questsToStart)
            {
                if (data != null && data.quest != null)
                    QuestManager.Instance.StartQuest(data.quest, data.startHidden);
            }
        }

        [System.Serializable]
        public class QuestStartData
        {
            public Quest quest;
            public bool startHidden;
        }
    }
}