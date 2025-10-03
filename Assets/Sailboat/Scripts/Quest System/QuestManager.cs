using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System.Linq;
using System;

namespace Sailboat
{
    public class QuestManager : Singleton<QuestManager>
    {
        private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();
        private List<Quest> activeQuests = new List<Quest>();
        private List<Quest> completedQuests = new List<Quest>();

        public event Action<Quest> OnQuestStarted;
        public event Action<Quest> OnQuestUpdated;
        public event Action<Quest> OnQuestCompleted;

        private void OnEnable()
        {
            GameEvents.Instance.OnLocationReached += HandleLocationReached;
            GameEvents.Instance.OnItemCollected += HandleItemCollected;
            GameEvents.Instance.OnNPCInteractedWith += HandleNPCSpokenTo;
        }

        public void LoadAllQuests()
        {
            allQuests.Clear();
            var quests = Resources.LoadAll<Quest>("Scriptable Objects/Quests/");
            foreach (var q in quests)
                allQuests[q.questId] = q;
                //allQuests[q.questId] = Instantiate(q); // instantiate so we have runtime copies
        }

        public void ResetAllQuests()
        {
            activeQuests.Clear();
            completedQuests.Clear();
            foreach (var quest in allQuests.Values)
                ResetQuestProgress(quest);
        }

        private void ResetQuestProgress(Quest quest)
        {
            quest.state = QuestState.NotStarted;
            foreach (var task in quest.tasks)
            {
                task.isCompleted = false;
            }
        }


        public bool StartQuest(string questId, bool hidden = false)
        {
            if (!allQuests.TryGetValue(questId, out var quest)) return false;
            if (activeQuests.Contains(quest) || completedQuests.Contains(quest)) return false;

            StartQuest(quest, hidden);
            return true;
        }

        public void StartQuest(Quest quest, bool hidden = false)
        {
            if (quest.state == QuestState.NotStarted)
                //When "actually" starting the quest
            {
                quest.state = hidden? QuestState.NotStartedButListening : QuestState.InProgress;
                activeQuests.Add(quest);
                if(!hidden) OnQuestStarted?.Invoke(quest);
            }
            else if(quest.state == QuestState.NotStartedButListening && hidden == false)
                //When want to declare quest as running
            {
                quest.state = QuestState.InProgress;
                OnQuestStarted?.Invoke(quest);
            }
            // TODO: see if you actually need this
            CheckTaskAlreadyCompleted(quest);
        }

        public void CompleteTask(Quest quest, string taskId)
        {
            QuestTask task = quest.tasks.FirstOrDefault(t => t.taskId == taskId);
            if (task != null && !task.isCompleted)
            {
                task.isCompleted = true;
                if (task.startingQuest && quest.state == QuestState.NotStartedButListening)
                {
                    quest.state = QuestState.InProgress;
                    OnQuestStarted?.Invoke(quest);
                }
                CheckQuestCompletion(quest);
            }
        }

        private void CheckQuestCompletion(Quest quest)
        {
            if (quest.tasks.All(t => t.isCompleted))
            {
                quest.state = QuestState.Completed;
                activeQuests.Remove(quest);
                completedQuests.Add(quest);
                GiveReward(quest.reward);
                OnQuestCompleted?.Invoke(quest);
            }
            else
                OnQuestUpdated?.Invoke(quest);
        }

        private void GiveReward(Reward reward)
        {
            // Add resources, trigger dialogs, etc.
        }

        

        private void CheckTaskAlreadyCompleted(Quest quest)
        {
            //TODO expand this
            foreach (var task in quest.tasks)
            {
                if (task.taskType == QuestTaskType.CollectItem &&
                    G.Instance.inventory.HasItem(task.targetId, task.targetAmount))
                {
                    CompleteTask(quest, task.taskId);
                }
            }
        }

        private void HandleNPCSpokenTo(string npcId)
        {
            TaskCompletionQueue queue = new TaskCompletionQueue(this);
            foreach (var quest in activeQuests)
            {
                foreach (var task in quest.tasks)
                {
                    if (task.taskType == QuestTaskType.TalkToNPC && task.targetId == npcId)
                        queue.AddToComplete(quest, task.taskId);
                }
            }
            queue.completeAll();
        }

        private void HandleItemCollected(string itemId, int amount)
        {
            TaskCompletionQueue queue = new TaskCompletionQueue(this);
            int overallAmount = 0;
            foreach (var quest in activeQuests)
            {
                foreach (var task in quest.tasks)
                {
                    if (task.taskType == QuestTaskType.CollectItem && task.targetId == itemId 
                        && G.Instance.inventory.items.TryGetValue(itemId,out overallAmount) && overallAmount >= task.targetAmount)
                        queue.AddToComplete(quest, task.taskId);
                }
            }
            queue.completeAll();
        }

        private void HandleLocationReached(string locationId)
        {
            TaskCompletionQueue queue = new TaskCompletionQueue(this);

            foreach (var quest in activeQuests)
            {
                foreach (var task in quest.tasks)
                {
                    if (task.taskType == QuestTaskType.GoToLocation && task.targetId == locationId)
                        queue.AddToComplete(quest, task.taskId);
                }
            }

            queue.completeAll();
        }
        class TaskCompletionQueue
        {
            QuestManager questManager;

            List<(Quest quest, string taskId)> queue = new List<(Quest quest, string taskId)>();

            public TaskCompletionQueue(QuestManager questManager)
            {
                this.questManager = questManager;
            }

            public void AddToComplete(Quest quest, string taskId)
            {
                queue.Add((quest, taskId));
            }

            public void completeAll()
            {
                foreach (var (quest, taskId) in queue)
                {
                    questManager.CompleteTask(quest, taskId);
                }
            }
        }
    }
    
}