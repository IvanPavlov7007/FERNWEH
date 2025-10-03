using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Sailboat
{
    public class QuestLogUI : MonoBehaviour
    {

        public GameObject questEntryPrefab;
        public GameObject taskEntryPrefab;
        public Transform questListParent;

        private Dictionary<string, GameObject> activeQuestUI = new();

        private void OnEnable()
        {
            QuestManager.Instance.OnQuestStarted += AddQuestUI;
            QuestManager.Instance.OnQuestUpdated += UpdateQuestUI;
            QuestManager.Instance.OnQuestCompleted += RemoveQuestUI;
        }

        private void OnDisable()
        {
            QuestManager.Instance.OnQuestStarted -= AddQuestUI;
            QuestManager.Instance.OnQuestUpdated -= UpdateQuestUI;
            QuestManager.Instance.OnQuestCompleted -= RemoveQuestUI;
        }

        private void AddQuestUI(Quest quest)
        {
            var questGO = Instantiate(questEntryPrefab, questListParent);
            questGO.name = quest.questId;

            questGO.transform.Find("Title").GetComponent<TMP_Text>().text = quest.questTitle;
            questGO.transform.Find("Description").GetComponent<TMP_Text>().text = quest.questDescription;

            var tasksParent = questGO.transform.Find("Tasks");
            foreach (var task in quest.tasks)
            {
                var taskGO = Instantiate(taskEntryPrefab, tasksParent);
                taskGO.transform.Find("TaskText").GetComponent<TMP_Text>().text = task.description;
                taskGO.GetComponentInChildren<Toggle>().isOn = task.isCompleted;
            }

            activeQuestUI[quest.questId] = questGO;
        }

        private void UpdateQuestUI(Quest quest)
        {
            if (activeQuestUI.TryGetValue(quest.questId, out var questGO))
            {
                var tasksParent = questGO.transform.Find("Tasks");
                var taskToggles = tasksParent.GetComponentsInChildren<Toggle>();
                for (int i = 0; i < quest.tasks.Length; i++)
                {
                    taskToggles[i].isOn = quest.tasks[i].isCompleted;
                }
            }
        }

        private void RemoveQuestUI(Quest quest)
        {
            if (activeQuestUI.TryGetValue(quest.questId, out var questGO))
            {
                Destroy(questGO);
                activeQuestUI.Remove(quest.questId);
            }
        }
    }
}