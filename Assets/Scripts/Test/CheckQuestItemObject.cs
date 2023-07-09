using Hexerspiel.Character;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckQuestItemObject : MonoBehaviour
{
    [SerializeField]
    SO_questItem questItem;

    [SerializeField]
    SO_spots spotToGo;

    [SerializeField]
    SO_npc npc;

    [SerializeField]
    SO_questStep questStep;

    [SerializeField]
    SO_weapon weapon;

    [SerializeField]
    SO_potion potion;

    [SerializeField]
    SO_questItem sO_QuestItem;

    private void Start()
    {

        //SO_questStep nextStep;

        //PlayerCharacter.Instance.Inventory.GetItem(weapon);

       Player.Instance.Inventory.PotionInventory.GetPotion(potion);
       //// PotionStats potionStats = PlayerCharacter.Instance.Inventory.PotionInventory.UsePotion(potion);



       Player.Instance.Inventory.QuestItemInventory.BuyQuestItem(sO_QuestItem);

       // PlayerCharacter.Instance.Inventory.QuestItemInventory.DropQuestItem(sO_QuestItem);
        // PlayerCharacter.Instance.Inventory.PotionInventory.SellPotion(potion);

        //  Debug.Log(potionStats.addMana);

        //nextStep = questStep.GetNextStepIfSolved(spotToGo, npc, null);

        //Debug.Log("next step:"+ nextStep);
    }
}
