using Hexerspiel.Character;
using Hexerspiel.nfcTags;
using Hexerspiel.spots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Quests
{

    [CreateAssetMenu(fileName = "step_MultipleChoice", menuName = "Hexer_ScriptableObjects/QuestSteps/MultipleChoice")]
    public class SO_step_multipleChoiceQuiz : SO_questStep
    {
        public RighAnswer rightAnswer = RighAnswer.a;
        public List<string> answersTexts = new List<string>();


        public override QuestTarget QuestStepTarget { get { return QuestTarget.multipleChoiceQuiz; } }


        //not needed
        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(SpotManager.currentStpot, NPCManager.currentNpc, QuestTracker.givenAnswer);
        }

        public override bool CheckIfStepIsSolved()
        {
            bool stepIsSolved = false;
            TestIfStepIsSolved(SpotManager.currentStpot, NPCManager.currentNpc, out stepIsSolved, QuestTracker.givenAnswer);
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
            if (answersTexts[0] == null)
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



            if (((SO_rightAnswer)possibleSolution[0]).givenAnswer == new SO_rightAnswer(rightAnswer).givenAnswer)
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