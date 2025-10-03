using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sailboat.Obsolete
{

    //[CreateAssetMenu(fileName = "QuestBase", menuName = "Quests/QuestBase")]
    public abstract class QuestBase : ScriptableObject
    {
        [SerializeField]
        protected string questName;
        [SerializeField]
        protected QuestState currentState;

        public string QuestName => questName;
        public QuestState CurrentState => currentState;
        public abstract string displayQuestInfo();
        public abstract void InitialiseQuest();

    }
}