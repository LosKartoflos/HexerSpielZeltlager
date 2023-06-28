using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_goToNPC", menuName = "Hexer_ScriptableObjects/QuestSteps/goToNPC")]
    public class SO_step_goToNpc : SO_questStep
    {
        protected override QuestTarget QuestStepTarget { get { return QuestTarget.goToNPC; } }

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