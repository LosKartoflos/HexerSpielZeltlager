
using Hexerspiel.Character;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.spots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_goToPlace", menuName = "Hexer_ScriptableObjects/QuestSteps/goToPlace")]
    public class SO_step_goToPlace : SO_questStep
    {
        
        protected override QuestTarget QuestStepTarget { get { return QuestTarget.goToPlace; } }

        public override bool GetIfStepIsSolved()
        {
            bool stepIsSolved = false;
            TestIfStepIsSolved(SpotManager.currentStpot, NPCManager.currentNpc, out stepIsSolved, null);
            return stepIsSolved;
        }
        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(SpotManager.currentStpot, NPCManager.currentNpc, null);
        }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (spotToGO == null)
            {
                Debug.LogError("You forgot to add a PLaceTog in the bringQuest item Step: " + name);
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