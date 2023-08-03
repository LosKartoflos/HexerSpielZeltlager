using Hexerspiel.Character.monster;
using Hexerspiel.nfcTags;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_fight", menuName = "Hexer_ScriptableObjects/QuestSteps/Fight")]
    public class SO_step_fight : SO_questStep
    {
        public SO_Monster monsterToFight;
        public DateTime timeQuestAccepted;

        public override QuestTarget QuestStepTarget { get { return QuestTarget.fightAgainst; } }

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
            if (Player.Instance.PlayerValues == null)
            {
                Debug.LogError("No PlayerValues");
                stepIsSolved = false;
                return;
            }



            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (!stepIsSolved)
                return;


            stepIsSolved = CheckIfMonsterHasbeenFoughtInTime();

           // stepIsSolved = true;

        }

        public bool CheckIfMonsterHasbeenFoughtInTime()
        {
            bool monsterHaseBeenFoughtInTime = false;

            if (Player.Instance.PlayerValues.killedMonsters.ContainsKey(monsterToFight.monsterName))
            {
                monsterHaseBeenFoughtInTime = true;
            }
               

            DateTime timeMonsterFought = new DateTime();
            Player.Instance.PlayerValues.killedMonsters.TryGetValue(monsterToFight.monsterName, out timeMonsterFought);

            if (timeMonsterFought >= timeQuestAccepted)
                monsterHaseBeenFoughtInTime = true;
            else
                monsterHaseBeenFoughtInTime = false;

            return monsterHaseBeenFoughtInTime;
        }

        


    }
}