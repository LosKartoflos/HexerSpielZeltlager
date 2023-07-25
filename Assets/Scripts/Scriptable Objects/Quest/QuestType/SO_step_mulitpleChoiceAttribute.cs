using Hexerspiel.Character;
using Hexerspiel.nfcTags;
using Hexerspiel.spots;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Quests
{

    [CreateAssetMenu(fileName = "step_MultipleChoiceAttribute", menuName = "Hexer_ScriptableObjects/QuestSteps/Choice by Attribute")]
    public class SO_step_mulitpleChoiceAttribute : SO_questStep
    {

        [Serializable]
        public struct AttributAnswer
        {
            public string Text;
            public AttributeTypes attribut;
            public int level;
            public SO_questStep questStep;
        }


        public List<AttributAnswer> answers = new List<AttributAnswer>();

        public override QuestTarget QuestStepTarget { get { return QuestTarget.multipleChoiceAttribute; } }

        public override SO_questStep GetNextStepIfSolved()
        {
            return GetNextStepIfSolved(QuestTracker.currentSpot, QuestTracker.currentNPC, QuestTracker.givenAnswer);
        }

        //not needed
        public override bool CheckIfStepIsSolved()
        {
            bool stepIsSolved = false;
            TestIfStepIsSolved(QuestTracker.currentSpot, QuestTracker.currentNPC, out stepIsSolved, null);
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

            if (answers.Count == 0)
            {
                Debug.LogError("You forgot to add a Answer : " + name);
                stepIsSolved = false;
                return;

            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (!stepIsSolved)
                return;

            nextQuestStep = null;



            switch (QuestTracker.givenAnswer.givenAnswer)
            {
                case RighAnswer.a:
                    if (Player.Instance.ReturnAttributeValue(answers[0].attribut) >= answers[0].level || answers[0].attribut == AttributeTypes.Nichts)
                    {
                        QuestTracker.nextQuestStepMultipleChoice = answers[0].questStep;
                        QuestTracker.givenAnswer = new SO_rightAnswer(RighAnswer.a);
                        stepIsSolved = true;
                        return;
                    }
                    break;
                case RighAnswer.b:
                    if (Player.Instance.ReturnAttributeValue(answers[1].attribut) >= answers[1].level || answers[1].attribut == AttributeTypes.Nichts)
                    {
                        QuestTracker.nextQuestStepMultipleChoice = answers[1].questStep;
                        QuestTracker.givenAnswer = new SO_rightAnswer(RighAnswer.b);
                        stepIsSolved = true;
                        return;
                    }
                    break;
                case RighAnswer.c:
                    if (Player.Instance.ReturnAttributeValue(answers[2].attribut) >= answers[2].level || answers[2].attribut == AttributeTypes.Nichts)
                    {
                        QuestTracker.nextQuestStepMultipleChoice = answers[2].questStep;
                        QuestTracker.givenAnswer = new SO_rightAnswer(RighAnswer.c);
                        stepIsSolved = true;
                        return;
                    }
                    break;
                case RighAnswer.d:
                    if (Player.Instance.ReturnAttributeValue(answers[3].attribut) >= answers[3].level || answers[3].attribut == AttributeTypes.Nichts)
                    {
                        QuestTracker.nextQuestStepMultipleChoice = answers[3].questStep;
                        QuestTracker.givenAnswer = new SO_rightAnswer(RighAnswer.d);
                        stepIsSolved = true;
                        return;
                    }
                    break;
            }



            stepIsSolved = false;
            return;


        }

       
    }
}