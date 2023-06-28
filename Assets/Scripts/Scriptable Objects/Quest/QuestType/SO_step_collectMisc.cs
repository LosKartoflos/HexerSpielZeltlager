using Hexerspiel.Character;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Quests
{
    [CreateAssetMenu(fileName = "step_collectMisc", menuName = "Hexer_ScriptableObjects/QuestSteps/collectMisc")]
    public class SO_step_collectMisc : SO_questStep
    {
        public MiscItems miscItmesNeeded;
        protected override QuestTarget QuestStepTarget { get { return QuestTarget.collectMisc; } }

        public override void TestIfStepIsSolved(SO_spots spotCurrent, SO_npc npcCurrent, out bool stepIsSolved, params ScriptableObject[] possibleSolution)
        {
            if (PlayerCharacter.Instance.Inventory.BasicInventory == null)
            {
                Debug.LogError("No Basicinventory");
                stepIsSolved = false;
                return;
            }

            if (!PlayerCharacter.Instance.Inventory.BasicInventory.CheckIfMiscItemsManipulationIsPossible(miscItmesNeeded))
            {
                stepIsSolved = false;
                return;
            }

            base.TestIfStepIsSolved(spotCurrent, npcCurrent, out stepIsSolved, possibleSolution);

            if (!stepIsSolved)
                return;


            //the conditions are fullfilled. Decrease the amount and step is solved.
            PlayerCharacter.Instance.Inventory.BasicInventory.ControlledMiscManipulation(miscItmesNeeded);

            stepIsSolved = true;

        }
    }
}