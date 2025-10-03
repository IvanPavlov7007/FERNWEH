using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Game/Quest")]
    public class Quest : ScriptableObject
    {
        public string questId;
        public string questTitle;
        [TextArea] public string questDescription;
        public QuestTask[] tasks;
        public QuestState state;
        public Quest[] nextQuests; // Which quests unlock when complete
        public Reward reward;
    }

    [System.Serializable]
    public class QuestTask
    {
        public string taskId;           // Unique ID for saving
        public string description;      // What the player sees
        public QuestTaskType taskType;  // e.g., GoToLocation, CollectItem, TalkToNPC
        public bool startingQuest; // switches quest state to InProgress upon completion
        public string targetId;         // Location ID, item ID, NPC ID
        public int targetAmount;        // For collection tasks
        public bool isCompleted;
    }

    public enum QuestTaskType
    {
        GoToLocation,
        CollectItem,
        TalkToNPC,
        BuildStructure,
        EscortNPC,
        ExploreArea,
        InteractWithNPC
    }

    public enum QuestState { NotStarted, NotStartedButListening, InProgress, Completed, Failed }


    #region Reward
    [System.Serializable]
    public class Reward
    {
        public List<ItemReward> items;        // Give items/resources
        public List<DialogueReward> dialogues; // Unlock/change NPC dialogues
        public List<Quest> unlockQuests;      // Start or unlock new quests
        public List<FlagReward> flags;        // Set world/game flags (karma, story triggers)
    }
    [System.Serializable]
    public class ItemReward
    {
        public string itemId;
        public int amount;
    }

    [System.Serializable]
    public class DialogueReward
    {
        public string npcId;
        public string dialogueId; // ID for dialogue system
    }

    [System.Serializable]
    public class FlagReward
    {
        public string flagId;
        public bool value;
    }
    #endregion

}