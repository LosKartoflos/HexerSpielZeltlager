using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Character
{
    [Serializable]
    public struct MiscItems
    {
        public int herbs, meat, magicEssence;
    }


    /// <summary>
    /// Class for the basic inventory values which act like currencies
    /// </summary>
    public class BasicInventory : MonoBehaviour
    {

        [SerializeField]
        private BasicInventoryCounters amount = new BasicInventoryCounters();

        public BasicInventory(BasicInventoryCounters amount)
        {
            this.amount = amount;
        }

        public BasicInventory(int gold)
        {
            amount.gold = gold;
            amount.miscItems.herbs = 0;
            amount.miscItems.magicEssence = 0;
            amount.miscItems.meat = 0;
        }

        public BasicInventory(int gold, MiscItems miscItems)
        {
            amount.gold = gold;
            amount.miscItems = miscItems;
        }

        public BasicInventory(int gold, int herbs, int magicEssence, int meat)
        {
            amount.gold = gold;
            amount.miscItems.herbs = herbs;
            amount.miscItems.magicEssence = magicEssence;
            amount.miscItems.meat = meat;
        }

        public BasicInventory()
        {
            amount.gold = 0;
            amount.miscItems.herbs = 0;
            amount.miscItems.magicEssence = 0;
            amount.miscItems.meat = 0;
        }

        public BasicInventoryCounters Amount { get => amount; }


        /// <summary>
        /// Just changes the given amount by the given ManipulatioAmmount. can slide into minus. check before
        /// </summary>
        /// <param name="currentAmount">amount to manipulate</param>
        /// <param name="manipulationAmount">amount delta</param>
        /// <returns></returns>
        public static BasicInventoryCounters ManipulationOutcome(BasicInventoryCounters currentAmount, BasicInventoryCounters manipulationAmount)
        {
            BasicInventoryCounters newAmount = new BasicInventoryCounters(
            currentAmount.gold + manipulationAmount.gold,
            currentAmount.miscItems.herbs + manipulationAmount.miscItems.herbs,
            currentAmount.miscItems.magicEssence + manipulationAmount.miscItems.magicEssence,
            currentAmount.miscItems.meat + manipulationAmount.miscItems.meat);

            return newAmount;
        }

        /// <summary>
        /// checks if a substractions would cause a minus. the amount hast to be in minus
        /// </summary>
        /// <param name="inventoryCounters">the given ammount</param>
        /// <param name="manipulationAmount">delta</param>
        /// <returns></returns>
        public static bool CheckIfManipulationIsPossible(BasicInventoryCounters inventoryCounters, BasicInventoryCounters manipulationAmount)
        {
            if (inventoryCounters.gold + manipulationAmount.gold < 0)
                return false;
            else if (manipulationAmount.miscItems.herbs + inventoryCounters.miscItems.herbs < 0)
                return false;
            else if (manipulationAmount.miscItems.magicEssence + inventoryCounters.miscItems.magicEssence < 0)
                return false;
            else if (manipulationAmount.miscItems.meat + inventoryCounters.miscItems.meat < 0)
                return false;

            return true;
        }

        /// <summary>
        /// substracts BasicInventory Counters if possible
        /// </summary>
        /// <param name="manipulationAmount">The changes of the inventory</param>
        /// <returns></returns>
        public bool ControlledManipulation(BasicInventoryCounters manipulationAmount)
        {
            if (!CheckIfManipulationIsPossible(amount, manipulationAmount))
            {
                return false;
            }

            amount = ManipulationOutcome(amount, manipulationAmount);

            return true;
        }

        /// <summary>
        /// substracts Misc Counters if possible
        /// </summary>
        /// <param name="manipulationAmount">The changes of the MiscItems in the Inventory</param>
        /// <returns></returns>
        public bool ControlledMiscManipulation(MiscItems manipulationAmount)
        {
            if (!CheckIfMiscItemsManipulationIsPossible(manipulationAmount))
            {
                return false;
            }

            BasicInventoryCounters bicAmount = new BasicInventoryCounters(0, manipulationAmount);

            ControlledManipulation(bicAmount);

            return true;
        }

        public bool CheckIfManipulationIsPossible(BasicInventoryCounters manipulationAmount)
        {

            return CheckIfManipulationIsPossible(amount, manipulationAmount);
        }

        public bool CheckIfMiscItemsManipulationIsPossible(MiscItems manipulationAmount)
        {
            BasicInventoryCounters bic = new BasicInventoryCounters(0, manipulationAmount);

            return CheckIfManipulationIsPossible(amount, bic);
        }


        //Gold

        public bool CheckIfGoldManipulationIsPossible(int goldAmount)
        {
            BasicInventoryCounters bic = new BasicInventoryCounters(goldAmount);

            return CheckIfManipulationIsPossible(bic);
        }

        public bool ChangeGold(int amount)
        {
            if (!CheckIfGoldManipulationIsPossible(amount))
                return false;

            this.amount.gold += amount;

            return true;
        }

        public bool ChanageMeat(int amount)
        {
            MiscItems miscItems;
            miscItems.herbs = 0;
            miscItems.magicEssence = 0;
            miscItems.meat = amount;

            if (!CheckIfMiscItemsManipulationIsPossible(miscItems))
                return false;

            this.amount.miscItems.meat += amount;

            return true;
        }

        public bool ChanageMagicessence(int amount)
        {
            MiscItems miscItems;
            miscItems.herbs = 0;
            miscItems.magicEssence = amount;
            miscItems.meat = 0;

            if (!CheckIfMiscItemsManipulationIsPossible(miscItems))
                return false;

            this.amount.miscItems.magicEssence += amount;

            return true;
        }

        public bool ChanageHerbs(int amount)
        {
            MiscItems miscItems;
            miscItems.herbs = amount;
            miscItems.magicEssence = 0;
            miscItems.meat = 0;

            if (!CheckIfMiscItemsManipulationIsPossible(miscItems))
                return false;

            this.amount.miscItems.herbs += amount;

            return true;
        }
    }
}