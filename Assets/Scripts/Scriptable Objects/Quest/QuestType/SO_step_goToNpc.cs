using Hexerspiel.Character;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.spots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_goToNPC", menuName = "Hexer_ScriptableObjects/QuestSteps/goToNPC")]
    public class SO_step_goToNpc : SO_questStep
    {
        public override QuestTarget QuestStepTarget { get { return QuestTarget.goToNPC; } }

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
                return true;
            }
        }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (npcToInteract == null)
            {
                Debug.LogError("You forgot to add a npcToInteract in the bringQuest item Step: " + name);
                stepIsSolved = false;
                return;

            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);
            //place is tested here before nothing mor to do;

            if (!stepIsSolved)
                return;




        }
    }
}