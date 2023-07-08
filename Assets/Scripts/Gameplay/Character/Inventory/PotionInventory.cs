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
        [SerializeField]
        private List<SO_potion> potionList = new List<SO_potion>();

        public List<SO_potion> PotionList { get => potionList; }

        public void GetPotion(SO_potion newPotion)
        {
            potionList.Add(newPotion);
        }

        public PotionStats UsePotion(SO_potion usedPotion)
        {
            if (DropPotion(usedPotion))
            {
                
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
            }
            

            return soldSuccesfull;
        }

        public bool SellPotion(SO_potion potionToSell)
        {

            if (DropPotion(potionToSell))
            {
                PlayerCharacter.Instance.Inventory.BasicInventory.ChangeGold(potionToSell.valueSell);
                Debug.Log("Sell " + potionToSell.name + " for " + potionToSell.valueSell.ToString());
                return true;
            }

            return false;
        }

        public bool BuyPotion(SO_potion potionToBuy)
        {

            if (PlayerCharacter.Instance.Inventory.BasicInventory.ChangeGold(-potionToBuy.valueBuy))
            {
                GetPotion(potionToBuy);
                Debug.Log("Buy " + potionToBuy.name + " for " + potionToBuy.valueBuy.ToString());
                return true;
            }
            return false;
        }
    }
}