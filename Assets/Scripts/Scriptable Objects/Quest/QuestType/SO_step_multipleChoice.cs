using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Quests
{

    [CreateAssetMenu(fileName = "step_MultipleChoice", menuName = "Hexer_ScriptableObjects/QuestSteps/MultipleChoice")]
    public class SO_step_multipleChoice : SO_questStep
    {
        public RighAnswer rightAnswer = RighAnswer.a;
        public string answerA, answerB, answerC, answerD;


        public override SO_questStep GetNextStepIfSolved()
        {
            throw new System.NotImplementedException();
        }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (answerA == null)
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



            if ((SO_rightAnswer)possibleSolution[0] == new SO_rightAnswer(rightAnswer))
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