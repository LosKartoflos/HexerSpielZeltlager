using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    public abstract class SO_questStep : ScriptableObject
    {
        public string stepName;
        public string image;

        //only needs a NFCTag if its a start quest;
        public string questText;
        public string resolvedText;

        //reward
        public int gold;
        public int xp;
        //Drops
        public int herbs, meat, magicEssence;
        public List<SO_gear> dropedGear;
        public List<SO_questItem> dropedQuestItems;



        bool lastQuestStep = false;
        public SO_questStep nextQuestStep;

        //Condtions (if not needed leave empty
        [Header("Conditions")]
        public SO_spots spotToGO;
        public SO_npc npcToInteract;

        //to do:
        // check if place or location is valid for quest

        public virtual void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            stepIsSolved = false;

            if (spotToGO == null || spotToGO == spotCurrent)
            {
                stepIsSolved = true;
            }
            else
            {
                return;
            }

            if (npcCurrent == null || npcToInteract == npcCurrent)
            {
                stepIsSolved = true;
            }
            else
            {
                return;
            }


        }

        public SO_questStep GetNextStepIfSolved(SO_spots spotCurrent, SO_npc npcCurrent, params ScriptableObject[] possibleSolution)
        {
            bool stepIsSolved = false;

            TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (stepIsSolved)
                return nextQuestStep;
            else
                return null;
        }
    }

}
