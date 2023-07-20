
using Hexerspiel.Character;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.spots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_bringQuestItem", menuName = "Hexer_ScriptableObjects/QuestSteps/bringQuestItem")]
    public class SO_step_bringQuestItem : SO_questStep
    {
        public SO_questItem questItemNeeded;
        protected override QuestTarget QuestStepTarget { get { return QuestTarget.bringQuestItem; } }

        public override bool GetIfStepIsSolved()
        {
            bool stepIsSolved = false;
            TestIfStepIsSolved(SpotManager.currentStpot, NPCManager.currentNpc, out stepIsSolved, Player.Instance.Inventory.QuestItemInventory.QuestItemsList.ToArray());
                return stepIsSolved;
        }

        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(SpotManager.currentStpot, NPCManager.currentNpc, Player.Instance.Inventory.QuestItemInventory.QuestItemsList.ToArray());
        }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (questItemNeeded == null)
            {
                Debug.LogError("You forgot to add a questItem in the bringQuest item Step: " + name);
                stepIsSolved = false;
                return;

            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (!stepIsSolved)
                return;


            //no object to check
            if(possibleSolution.Length == 0 && questItemNeeded)
            {
                stepIsSolved = false;
                return;
            }

            

            //check if needed object is in the loop
            foreach (SO_questItem solution in possibleSolution)
            {
                if(solution == null)
                {
                    stepIsSolved = false;
                }
                else if (solution.itemName == questItemNeeded.itemName)
                {
                    stepIsSolved = true;
                    break;
                }
                else
                {
                    stepIsSolved = false;
                }
            }

        }
    }
}