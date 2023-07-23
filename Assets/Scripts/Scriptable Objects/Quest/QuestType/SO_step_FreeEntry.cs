using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_freeEntry", menuName = "Hexer_ScriptableObjects/QuestSteps/Free Entry")]
    public class SO_step_FreeEntry : SO_questStep
    {
        public string shortQuestion;
        public string answer;


        public override QuestTarget QuestStepTarget { get { return QuestTarget.freeEntry; } }


        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(QuestTracker.currentSpot, QuestTracker.currentNPC, new SO_freeEntry( QuestTracker.freeEntry));
        }

        public override bool CheckIfStepIsSolved()
        {
            bool stepIsSolved = false;
            TestIfStepIsSolved(QuestTracker.currentSpot, QuestTracker.currentNPC, out stepIsSolved, new SO_freeEntry(QuestTracker.freeEntry));
            return stepIsSolved;
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
            if (answer == "")
            {
                Debug.LogError("You forgot to add a Answer : " + name);
                stepIsSolved = false;
                return;

            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (!stepIsSolved)
                return;


            //no object to check
            if (possibleSolution.Length == 0)
            {
                stepIsSolved = false;
                return;
            }



            if (((SO_freeEntry)possibleSolution[0]).entry == answer)
            {
                stepIsSolved = true;
                return;
            }
            else
            {
                stepIsSolved = false;
                return;
            }

        }
    }
}