using System.Collections;
using System.Text;
using UnityEngine;

namespace Sailboat.Obsolete
{
    //[CreateAssetMenu(fileName = "FirstVillageQuest", menuName = "Quests/FirstVillageQuest")]
    public class TravelToFirstVillageQuest : QuestBase
    {
        public string TaskHint = "Travel somewhere far away";

        public override string displayQuestInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(QuestName).AppendLine(TaskHint);
            return sb.ToString();
        }

        public override void InitialiseQuest()
        {
            //currentState = QuestState.Started;
        }

        void onFirstVillageAreaEnter()
        {
            //currentState = QuestState.Finished;
        }    
    }
}