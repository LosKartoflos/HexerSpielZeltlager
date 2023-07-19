using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    [RequireComponent(typeof(Inventory))]
    public class QuestItemInventory : MonoBehaviour
    {

        [SerializeField]
        private List<SO_questItem> questItemsList = new List<SO_questItem>();
        public static event Action<string> AlertQuestItemChanged = delegate { };

        public List<SO_questItem> QuestItemsList { get => questItemsList; }

        public void GetQuestItem(SO_questItem newQuestitem)
        {
            questItemsList.Add(newQuestitem);
            AlertQuestItemChanged("Du hast " + newQuestitem.itemName + " erhalten!");
        }

        public string UseQuestItem(SO_questItem usedItem)
        {
            if (DropQuestItem(usedItem))
            {
                return usedItem.name;
            }
            Debug.Log("no fitting Item in inventory");
            return "";
        }



        public bool DropQuestItem(SO_questItem itemToDrop)
        {
            bool dropedSuccessfull = false;

            if (questItemsList.Contains(itemToDrop))
            {
                questItemsList.Remove(itemToDrop);
                dropedSuccessfull = true;
            }


            return dropedSuccessfull;
        }

        //public bool SellQuestItem(SO_questItem itemToSell)
        //{

        //    if (DropQuestItem(itemToSell))
        //    {
        //        PlayerCharacter.Instance.Inventory.BasicInventory.ChangeGold(itemToSell.valueSell);
        //        Debug.Log("Sell " + itemToSell.name + " for " + itemToSell.valueSell.ToString());
        //        return true;
        //    }

        //    return false;
        //}

        public bool BuyQuestItem(SO_questItem itemToBuy)
        {

            if (Player.Instance.Inventory.BasicInventory.ChangeGold(-itemToBuy.valueBuy))
            {
                GetQuestItem(itemToBuy);
                Debug.Log("Buy " + itemToBuy.name + " for " + itemToBuy.valueBuy.ToString());
                AlertQuestItemChanged("Du hast " + itemToBuy.itemName + " f�r " + itemToBuy.valueBuy + " gekauft!");
                return true;
            }
            return false;
        }
    }
}