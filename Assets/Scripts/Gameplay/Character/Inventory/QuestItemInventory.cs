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
        public static bool isLoaded = false;

        [SerializeField]
        private List<SO_questItem> questItemsList = new List<SO_questItem>();
        public static event Action<string> AlertQuestItemChanged = delegate { };

        public List<SO_questItem> QuestItemsList { get => questItemsList; }

        public void Saver()
        {
            ES3.Save("questItemsList", questItemsList, "gear.es3");

        }


        public void Loader()
        {
            if (isLoaded)
                return;

            isLoaded = true;

            if (ES3.KeyExists("questItemsList", "gear.es3"))
                questItemsList = (List<SO_questItem>)ES3.Load("questItemsList", "gear.es3");
        }

        private void Awake()
        {
            Loader();
        }

        public void GetQuestItem(SO_questItem newQuestitem)
        {
            questItemsList.Add(newQuestitem);
            AlertQuestItemChanged("Du hast " + newQuestitem.itemName + " erhalten!");
            Saver();
        }

        public string UseQuestItem(SO_questItem usedItem)
        {
            if (DropQuestItem(usedItem))
            {
                Saver();
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
                Saver();
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
                AlertQuestItemChanged("Du hast " + itemToBuy.itemName + " für " + itemToBuy.valueBuy + " gekauft!");
                Saver();
                return true;
            }
            return false;
        }
    }
}