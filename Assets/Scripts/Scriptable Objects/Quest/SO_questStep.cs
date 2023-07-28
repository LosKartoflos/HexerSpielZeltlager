using Hexerspiel.Character;
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
        public int xp;
        public BasicInventoryCounters rewards;
        //Drops
        public List<SO_gear> dropedGear;
        public List<SO_questItem> dropedQuestItems;



        bool lastQuestStep = false;
        public SO_questStep nextQuestStep;

        //Condtions (if not needed leave empty
        [Header("Conditions")]
        public SO_spots spotToGO;
        public SO_npc npcToInteract;

        public virtual QuestTarget QuestStepTarget { get { return QuestTarget.baseStep; } }



        public virtual void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            stepIsSolved = false;


            if (spotToGO != null && spotCurrent == null)
            {
                stepIsSolved = false;
                return;
            }
            else if (spotToGO == null)
            {
                stepIsSolved = true;
            }
            else if (spotToGO.nfcTagInfos.name == spotCurrent.nfcTagInfos.name)
            {
                stepIsSolved = true;
            }
            else if (spotToGO.nfcTagInfos.name != spotCurrent.nfcTagInfos.name)
            {
                stepIsSolved = false;
                return;
            }
            else
            {
                return;
            }


            //ther is an npc needed but no provided
            if (npcToInteract != null && npcCurrent == null)
            {
                stepIsSolved = false;
                return;
            }
            //no npc needed
            else if (npcToInteract == null)
            {
                stepIsSolved = true;
            }
            // npc is needed and provided
            else if (npcToInteract.npcInformation.name == npcCurrent.npcInformation.name)
            {
                stepIsSolved = true;
            }
            // npc is needed and provided but wrong
            else if (npcToInteract.npcInformation.name != npcCurrent.npcInformation.name)
            {
                stepIsSolved = false;
                return;
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

        protected int xpWhole, goldWhole, meatWhole, herbsWhole, magicEssnceWhole;
        protected List<SO_gear> gearWhole = new List<SO_gear>();

        public string GetLootTextWhole()
        {

            xpWhole = xp;
            goldWhole = rewards.gold;
            meatWhole = rewards.miscItems.meat;
            herbsWhole = rewards.miscItems.herbs;
            magicEssnceWhole = rewards.miscItems.magicEssence;
            gearWhole = null;
            gearWhole = new List<SO_gear>();
            gearWhole.AddRange(dropedGear);

            CollectWholeGear(nextQuestStep);

            string basisReward = string.Format("{0} XP und {1} Gold\n{2} Fleisch {3} Kräuter {4} Mag. Essenz", xpWhole, goldWhole, meatWhole, herbsWhole, magicEssnceWhole);
            string gearReward = "";
            foreach (SO_gear g in gearWhole)
            {
                gearReward += "\n" + g.itemName;
            }


            string final = "Du erhälst für die Gesamte Reihe:\n";
            if (xpWhole > 0 || goldWhole > 0 || meatWhole > 0 || herbsWhole > 0 || magicEssnceWhole > 0)
                final += basisReward;
            if (gearWhole.Count > 0)
                final += "\n Ausrüstung: " + gearReward;


            return final;
        }

        public void CollectWholeGear(SO_questStep nexStep)
        {
            if (nexStep == null || nexStep == QuestTracker.Instance.FinishStep)
                return;

            xpWhole += nexStep.xp;
            goldWhole += nexStep.rewards.gold;
            meatWhole += nexStep.rewards.miscItems.meat;
            herbsWhole += nexStep.rewards.miscItems.herbs;
            magicEssnceWhole += nexStep.rewards.miscItems.magicEssence;
            gearWhole.AddRange(nexStep.dropedGear);

            //Solange aufrufen bis an der Wuzrel
            if (nexStep.QuestStepTarget == QuestTarget.multipleChoiceAttribute && nexStep.nextQuestStep != QuestTracker.Instance.FinishStep)
            {
                if (((SO_step_mulitpleChoiceAttribute)nexStep).answers[0].questStep != null || ((SO_step_mulitpleChoiceAttribute)nexStep).answers[0].questStep != QuestTracker.Instance.FinishStep)
                    CollectWholeGear(((SO_step_mulitpleChoiceAttribute)nexStep).answers[0].questStep);
            }

            else if (nexStep.nextQuestStep != null || nexStep.nextQuestStep != QuestTracker.Instance.FinishStep)
            {
                CollectWholeGear(nexStep.nextQuestStep);
            }

        }

        public string GetLootText()
        {
            string basisReward = string.Format("{0} XP und {1} Gold\n{2} Fleisch {3} Kräuter {4} Mag. Essenz", xp, rewards.gold, rewards.miscItems.meat, rewards.miscItems.herbs, rewards.miscItems.magicEssence);
            string gearReward = "";
            foreach (SO_gear g in dropedGear)
            {
                gearReward += "\n" + g.itemName + "\n";
            }
            string itemReward = "";
            foreach (SO_questItem g in dropedQuestItems)
            {
                itemReward += "\n" + g.itemName;
            }

            string final = "======================\n\nDu erhälst für diese Aufgabe:\n";
            if (xp > 0 || rewards.gold > 0 || rewards.miscItems.meat > 0 || rewards.miscItems.herbs > 0 || rewards.miscItems.magicEssence > 0)
                final += basisReward;
            if (dropedGear.Count > 0)
                final += "\n Ausrüstung: " + gearReward;
            if (dropedQuestItems.Count > 0)
                final += "\nQuestitems: " + itemReward;

            return final;
        }

        public string GetShortLootText()
        {
            return string.Format("Loot: {0} XP {1} Gold", xp, rewards.gold);
        }

        public abstract SO_questStep GetNextStepIfSolved();
        public abstract bool CheckIfStepIsSolved();

        public abstract bool PayQuestPriceAndEndStep();

    }

}
