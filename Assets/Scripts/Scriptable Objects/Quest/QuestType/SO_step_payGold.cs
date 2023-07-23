using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_payGold", menuName = "Hexer_ScriptableObjects/QuestSteps/payGold")]
    public class SO_step_payGold : SO_questStep
    {
        public int goldToPay;
        public override QuestTarget QuestStepTarget { get { return QuestTarget.payGold; } }

        public override bool CheckIfStepIsSolved()
        {
            bool stepIsSolved = false;
            TestIfStepIsSolved(QuestTracker.currentSpot, QuestTracker.currentNPC, out stepIsSolved, null);
            return stepIsSolved;
        }

        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(QuestTracker.currentSpot, QuestTracker.currentNPC, null);
        }

        public override bool PayQuestPriceAndEndStep()
        {
            if (CheckIfStepIsSolved() == false)
            {
                return false;
            }
            else
            {
                return Player.Instance.Inventory.BasicInventory.ChangeGold(-goldToPay);
            }
        }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (Player.Instance.Inventory.BasicInventory == null)
            {
                Debug.LogError("No Basicinventory");
                stepIsSolved = false;
                return;
            }

            if (!Player.Instance.Inventory.BasicInventory.CheckIfGoldManipulationIsPossible(-goldToPay))
            {
                stepIsSolved = false;
                return;
            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (!stepIsSolved)
                return;


            //the conditions are fullfilled. Decrease the amount and step is solved.


            stepIsSolved = true;

        }
    }
}
