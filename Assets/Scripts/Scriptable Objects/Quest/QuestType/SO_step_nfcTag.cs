using Hexerspiel.Character;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.spots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    public class SO_step_nfcTag : SO_questStep
    {
        protected override QuestTarget QuestStepTarget { get { return QuestTarget.nfcTag; } }

        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(SpotManager.currentStpot, NPCManager.currentNpc, null);
        }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (QuestTracker.questSolveValidation == null)
            {
                Debug.LogError("You forgot to add a questSolveValidation in the bringQuest item Step: " + name);
                stepIsSolved = false;
                return;

            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);
            //place is tested here before nothing mor to do;

            if (!stepIsSolved)
                return;

            if (QuestTracker.questSolveValidation.questStepToSolve.stepName == this.stepName )
            {
                stepIsSolved = true;
                return;

            }

        }
    }
}