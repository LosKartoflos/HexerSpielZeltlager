using Hexerspiel;
using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Hexerspiel.Character.monster.MonsterCharacter;
using static Hexerspiel.Character.PlayerCharacterValues;

public class Player : MonoBehaviour
{

    public static bool isLoaded = false;
    #region Variables
    private static Player instance;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerCharacterValues playerValues;

    public static int xpPerLevel = 100;
    #endregion

    #region Accessors
    public static Player Instance { get => instance; }

    public Inventory Inventory { get => inventory; set => inventory = value; }
    public PlayerCharacterValues PlayerValues { get => playerValues; }
    public string CollectedLoot { get => collectedLoot; }

    private string collectedLoot = "";

    [Header("Calculated Values with Gear")]

    [SerializeField]
    PlayerAttributes playerAttributesWithGear;

    public void Saver()
    {
        ES3.Save<PlayerCharacterValues>("playerValues", playerValues, "playerValues.es3");
    }


    public void Loader()
    {
        if (isLoaded)
            return;

        isLoaded = true;

        if (ES3.KeyExists("playerValues", "playerValues.es3"))
            playerValues = (PlayerCharacterValues)ES3.Load("playerValues", "playerValues.es3");
    }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

        Loader();
    }


    private void OnEnable()
    {
        PotionInventory.PotionUsed += DrinkPotion;
        MonsterCharacter.MonsterKilled += AddToMonsterKilledList;
    }



    private void OnDisable()
    {
        PotionInventory.PotionUsed -= DrinkPotion;
        MonsterCharacter.MonsterKilled -= AddToMonsterKilledList;
    }

    #endregion

    #region Functions

    public  int XPForNextlevel()
    {
        return xpPerLevel  + (xpPerLevel * playerValues.PlayerStats1.level - 1) /2;
    }

    private void AddToMonsterKilledList(string monsterName, DateTime deathTime)
    {
        if (playerValues.killedMonsters.ContainsKey(monsterName))
        {
            playerValues.killedMonsters[monsterName] = deathTime;
        }
        else
            playerValues.killedMonsters.Add(monsterName, deathTime);

        Saver();

    }

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
        playerAttributesWithGear.body = PlayerValues.PlayerAttributesComplete.body;
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
        return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributesComplete.body / Player.Instance.PlayerValues.PlayerAttributesComplete.attributAddThreshold);
    }

    //The extra Armor through the body
    public int ExtraAttackWithoutWeapon()
    {
        return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributesComplete.body / Player.Instance.PlayerValues.PlayerAttributesComplete.attributAddThreshold);
    }

    //recieveLoot
    public void RecieveLootMonster(MonsterStats loot)
    {
        collectedLoot = "Du hast erhalten: \n\n";
        GetGear(loot.dropedGear, true);
        GetQuestItems(loot.dropedQuestItems, true);
        GetPotions(loot.droppedPotion, true);
        ChangeMisc(loot.herbs, loot.meat, loot.magicEssence, true);
        ChangeGold(loot.gold, true);
        GetXp(loot.xp, true);
    }

    public void RecieveLootSpot(MonsterStats loot)
    {
        //collectedLoot = "Du hast erhalten: \n\n";
        //GetGear(loot.dropedGear, true);
        //GetQuestItems(loot.dropedQuestItems, true);
        //GetPotions(loot.droppedPotion, true);
        //ChangeMisc(loot.herbs, loot.meat, loot.magicEssence, true);
        //ChangeGold(loot.gold, true);
        //GetXp(loot.xp, true);
    }

    public void RecieveLootQuestStep(SO_questStep solvedStep)
    {
        collectedLoot = "Du hast erhalten: \n\n";
        GetGear(solvedStep.dropedGear, true);
        GetQuestItems(solvedStep.dropedQuestItems, true);
        ChangeMisc(solvedStep.rewards.miscItems.herbs, solvedStep.rewards.miscItems.meat, solvedStep.rewards.miscItems.magicEssence, true);
        ChangeGold(solvedStep.rewards.gold, true);
        GetXp(solvedStep.xp, true);
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

    public void GetGear(SO_gear gear, bool noteLoot = false)
    {
        if (gear != null)
        {

            Inventory.GearInventory.GetGear(gear);
            if (noteLoot == true)
                collectedLoot += "- " + gear.itemName + ("\n");

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

    public void GetQuestItem(SO_questItem qi, bool noteLoot = false)
    {
        Inventory.QuestItemInventory.GetQuestItem(qi);
        if (noteLoot == true)
            collectedLoot += "- " + qi.itemName + ("\n");
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

    public void GetPotion(SO_potion po, bool noteLoot = false)
    {
        Inventory.PotionInventory.GetPotion(po);
        if (noteLoot == true)
            collectedLoot += "- " + po.itemName + ("\n");
    }

    public void ChangeMisc(int herbs, int meat, int magiEssence, bool noteLoot = false)
    {
        Inventory.BasicInventory.ChangeHerbs(herbs);
        Inventory.BasicInventory.ChangeMeat(meat);
        Inventory.BasicInventory.ChangeMagicessence(magiEssence);

        if (noteLoot == true)
        {
            collectedLoot += string.Format("\nKräuter: {0}\nFleisch: {1}\nMagischEssenz: {2}\n", herbs, meat, magiEssence);
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
        if (noteLoot)
            collectedLoot += "XP: " + xpAmount;

        Saver();
    }

    public int ReturnAttributeValue(AttributeTypes attribute)
    {
        switch (attribute)
        {
            case AttributeTypes.Nichts:
                return 0;
                break;
            case AttributeTypes.Körper:
                return playerValues.PlayerAttributesComplete.body;
                break;
            case AttributeTypes.Geist:
                return playerValues.PlayerAttributesComplete.mind;
                break;
            case AttributeTypes.Charisma:
                return playerValues.PlayerAttributesComplete.charisma;
                break;

        }

        return 0;
        #endregion
    }
}