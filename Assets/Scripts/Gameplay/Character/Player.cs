using Hexerspiel.Character;
using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Hexerspiel.Character.monster.MonsterCharacter;
using static Hexerspiel.Character.PlayerCharacterValues;

public class Player : MonoBehaviour
{


    #region Variables
    private static Player instance;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerCharacterValues playerValues;
    #endregion

    #region Accessors
    public static Player Instance { get => instance; }

    public Inventory Inventory { get => inventory; set => inventory = value; }
    public PlayerCharacterValues PlayerValues { get => playerValues; set => playerValues = value; }
    public string CollectedLoot { get => collectedLoot; }

    private string collectedLoot = "";

    [Header("Calculated Values with Gear")]

    [SerializeField]
    PlayerAttributes playerAttributesWithGear;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);


    }

    private void OnEnable()
    {
        PotionInventory.PotionUsed += DrinkPotion;
    }
    private void OnDisable()
    {
        PotionInventory.PotionUsed -= DrinkPotion;
    }

    #endregion

    #region Functions
    /// <summary>
    /// use a potion outside of fight
    /// </summary>
    /// <param name="potion">potion to use</param>
    public void DrinkPotion(PotionStats potionStats)
    {
        playerValues.AddLife(potionStats.addLife);
        playerValues.AddMana(potionStats.addMana);
    }

    /// <summary>
    /// Calculates the Attribut Values with the gear applied
    /// </summary>
    public void CaclutlatePlayerAttributesWithGear()
    {
        playerAttributesWithGear.body = PlayerValues.PlayerAttributes1.body;
    }

    /// <summary>
    /// Calculates the offensive and defensive Values with attributes and gear
    /// </summary>
    public void CalculateFightingValues()
    {

    }

    //The extra Armor through the body
    public int ExtraArmor()
    {
        return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributes1.body / Player.Instance.PlayerValues.PlayerAttributes1.attributAddThreshold);
    }

    //The extra Armor through the body
    public int ExtraAttackWithoutWeapon()
    {
        return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributes1.body / Player.Instance.PlayerValues.PlayerAttributes1.attributAddThreshold);
    }

    //recieveLoot
    public void RecieveLoot(MonsterStats loot)
    {
        collectedLoot = "Du hast erhalten: \n\n";
        GetGear(loot.dropedGear, true);
        GetQuestItems(loot.dropedQuestItems, true);
        GetPotions(loot.droppedPotion, true);
        GetMisc(loot.herbs, loot.meat, loot.magicEssence, true);
        ChangeGold(loot.gold, true);
        GetXp(loot.xp, true);
    }

    public void GetGear(List<SO_gear> loot, bool noteLoot = false)
    {
        if (loot != null)
        {
            foreach (SO_gear gear in loot)
            {
                Inventory.GearInventory.GetGear(gear);
                if (noteLoot == true)
                    collectedLoot += "- " + gear.itemName + ("\n");
            }
        }
    }

    public void GetQuestItems(List<SO_questItem> loot, bool noteLoot = false)
    {
        if (loot != null)
        {
            foreach (SO_questItem qi in loot)
            {
                Inventory.QuestItemInventory.GetQuestItem(qi);
                if (noteLoot == true)
                    collectedLoot += "- " + qi.itemName + ("\n");
            }
        }

    }

    public void GetPotions(List<SO_potion> loot, bool noteLoot = false)
    {
        if (loot != null)
        {
            foreach (SO_potion po in loot)
            {
                Inventory.PotionInventory.GetPotion(po);
                if (noteLoot == true)
                    collectedLoot += "- " + po.itemName + ("\n");
            }
        }
    }

    public void GetMisc(int herbs, int meat, int magiEssence, bool noteLoot = false)
    {
        Inventory.BasicInventory.ChangeHerbs(herbs);
        Inventory.BasicInventory.ChangeMeat(meat);
        Inventory.BasicInventory.ChangeMagicessence(magiEssence);

        if (noteLoot == true)
        {
            collectedLoot += string.Format("\nKr�uter: {0}\nFleisch: {1}\nMagischEssenz: {2}\n",herbs, meat,magiEssence);
        }          
    }

    public bool ChangeGold(int goldAmount, bool noteLoot = false)
    {
        return Inventory.BasicInventory.ChangeGold(goldAmount);

        if (noteLoot)
        {
            collectedLoot += "Gold: " + goldAmount;
        }
    }

    public void GetXp(int xpAmount, bool noteLoot = false)
    {
        playerValues.GetXp(xpAmount);
        if (noteLoot )
            collectedLoot += "XP: " + xpAmount;
    }
    #endregion
}
