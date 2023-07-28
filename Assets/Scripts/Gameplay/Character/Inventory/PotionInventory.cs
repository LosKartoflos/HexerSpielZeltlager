using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    [Serializable]
    public struct PotionStats
    {
        public Dice.Manipulation diceManipulation;
        public float addMana, addLife;



        public PotionStats(Dice.Manipulation diceManipulation, float addMana, float addLife)
        {
            this.diceManipulation = diceManipulation;
            this.addMana = addMana;
            this.addLife = addLife;
        }
    }
    [RequireComponent(typeof(Inventory))]
    public class PotionInventory : MonoBehaviour
    {
        public static bool isLoaded = false;

        public static event Action<PotionStats> PotionUsed = delegate { };
        public static event Action<string> AlertPotionChanged = delegate { };
        [SerializeField]
        private List<SO_potion> potionList = new List<SO_potion>();

        public List<SO_potion> PotionList { get => potionList; }

        public void Saver()
        {
            ES3.Save("potionList", potionList, "gear.es3");

        }


        public void Loader()
        {
            if (isLoaded)
                return;

            isLoaded = true;

            if (ES3.KeyExists("potionList", "gear.es3"))
                potionList = (List<SO_potion>)ES3.Load("potionList", "gear.es3");
        }


        private void Awake()
        {
            Loader();
        }

        public void GetPotion(SO_potion newPotion)
        {
            potionList.Add(newPotion);
            AlertPotionChanged("Du hast " + newPotion.itemName + " erhalten!");
            Saver();
        }

        public PotionStats UsePotion(SO_potion usedPotion)
        {
            if (DropPotion(usedPotion))
            {
                PotionUsed(usedPotion.potionStats);
                Saver();
                AlertPotionChanged("Du hast " + usedPotion.itemName + " getrunken.\n\n " + usedPotion.GetDescription());
                return usedPotion.potionStats;

               
            }
            Debug.Log("potion not in inventory");


            return new PotionStats();
        }



        public bool DropPotion(SO_potion potionToDrop)
        {
            bool soldSuccesfull = false;

            if (potionList.Contains(potionToDrop))
            {
                potionList.Remove(potionToDrop);
                soldSuccesfull = true;
                Saver();
            }


            return soldSuccesfull;
        }

        public bool SellPotion(SO_potion potionToSell)
        {

            if (DropPotion(potionToSell))
            {
                Player.Instance.Inventory.BasicInventory.ChangeGold(potionToSell.valueSell);
                Debug.Log("Sell " + potionToSell.name + " for " + potionToSell.valueSell.ToString());
                AlertPotionChanged("Du hast " + potionToSell.itemName + " für " + potionToSell.valueSell + " verkauft!");
                Saver();
                return true;
            }

            return false;
        }

        public bool BuyPotion(SO_potion potionToBuy)
        {

            if (Player.Instance.Inventory.BasicInventory.ChangeGold(-potionToBuy.valueBuy))
            {
                GetPotion(potionToBuy);
                Debug.Log("Buy " + potionToBuy.name + " for " + potionToBuy.valueBuy.ToString());
                AlertPotionChanged("Du hast " + potionToBuy.itemName + " für " + potionToBuy.valueBuy + " gekauft!");
                Saver();
                return true;
            }
            return false;
        }
    }
}