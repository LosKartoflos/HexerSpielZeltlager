using Hexerspiel.Character;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Quests
{
    public abstract class SO_questStep : ScriptableObject
    {
      
        public string stepName;
        public string image;

        //only needs a NFCTag if its a start quest;
        public string questText;
        public string resolvedText;

        //reward
        public int xp;
        public BasicInventoryCounters rewards;
        //Drops
        public List<SO_gear> dropedGear;
        public List<SO_questItem> dropedQuestItems;



        bool lastQuestStep = false;
        public SO_questStep nextQuestStep;

        //Condtions (if not needed leave empty
        [Header("Conditions")]
        public SO_spots spotToGO;
        public SO_npc npcToInteract;

        protected virtual QuestTarget QuestStepTarget { get { return QuestTarget.baseStep; } }

        

        public virtual void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            stepIsSolved = false;

      
            if (spotToGO != null && spotCurrent == null)
            {
                stepIsSolved = false;
                return;
            }
            else if (spotToGO == null)
            {
                stepIsSolved = true;
            }
            else if (spotToGO.nfcTagInfos.name == spotCurrent.nfcTagInfos.name)
            {
                stepIsSolved = true;
            }
            else if (spotToGO.nfcTagInfos.name != spotCurrent.nfcTagInfos.name)
            {
                stepIsSolved = false;
                return;
            }
            else
            {
                return;
            }


            //ther is an npc needed but no provided
            if (npcToInteract != null && npcCurrent == null)
            {
                stepIsSolved = false;
                return;
            }
            //no npc needed
            else if (npcToInteract == null)
            {
                stepIsSolved = true;
            }
            // npc is needed and provided
            else if (npcToInteract.npcInformation.name == npcCurrent.npcInformation.name)
            {
                stepIsSolved = true;
            }
            // npc is needed and provided but wrong
            else if (npcToInteract.npcInformation.name != npcCurrent.npcInformation.name)
            {
                stepIsSolved = false;
                return;
            }
            else
            {
                return;
            }


        }

        public SO_questStep GetNextStepIfSolved(SO_spots spotCurrent, SO_npc npcCurrent, params ScriptableObject[] possibleSolution)
        {
            bool stepIsSolved = false;

            TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (stepIsSolved)
                return nextQuestStep;
            else
                return null;
        }

        public abstract SO_questStep GetNextStepIfSolved();
        public abstract bool GetIfStepIsSolved();

    }

}
