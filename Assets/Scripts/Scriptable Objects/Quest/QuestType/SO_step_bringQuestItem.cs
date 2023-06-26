
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_bringQuestItem", menuName = "Hexer_ScriptableObjects/QuestSteps/bringQuestItem")]
    public class SO_step_bringQuestItem : SO_questStep
    {
        public SO_questItem questItemNeeded;

        public override void  TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            //check if needed object is in the loop
            foreach(ScriptableObject solution in possibleSolution)
            {
                if(solution.name == questItemNeeded.name)
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