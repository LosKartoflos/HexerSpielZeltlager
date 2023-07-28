using Hexerspiel;
using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Hexerspiel.Character.monster.MonsterCharacter;
using static Hexerspiel.Character.PlayerCharacterValues;

public class Player : MonoBehaviour
{

    public static bool isLoaded = false;
    #region Variables
    private static Player instance;

    public static event Action<int> levelUPEvent = delegate { };

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerCharacterValues playerValues;

    public static int xpPerBase = 100;
    public static int xpPerLevel = 25;

    public static int startLive = 50;
    public static int livePerLevel = 10;

    public static int startmana = 10;
    public static int manaPerlevel = 2;


    public Dictionary<DateTime, float> lastHealhtEvent = new Dictionary<DateTime, float>();

    public Dictionary<DateTime, float> lastManaEvent = new Dictionary<DateTime, float>();
    #endregion

    #region Accessors
    public static Player Instance { get => instance; }

    public Inventory Inventory { get => inventory; set => inventory = value; }
    public PlayerCharacterValues PlayerValues { get => playerValues; set => playerValues = value; }
    public string CollectedLoot { get => collectedLoot; }

    private string collectedLoot = "";

    private int lastXp = 0;

    public void Saver()
    {
        ES3.Save<PlayerCharacterValues>("playerValues", playerValues, "playerValues.es3");
        if (lastHealhtEvent != null)
            ES3.Save<Dictionary<DateTime, float>>("lastHealtEvent", lastHealhtEvent, "playerValues.es3");
        if (lastManaEvent != null)
            ES3.Save<Dictionary<DateTime, float>>("lastManaEvent", lastManaEvent, "playerValues.es3");
    }


    public void Loader()
    {

        if (isLoaded)
            return;

        isLoaded = true;

        if (ES3.KeyExists("playerValues", "playerValues.es3"))
            playerValues = (PlayerCharacterValues)ES3.Load("playerValues", "playerValues.es3");
        if (ES3.KeyExists("lastHealtEvent", "playerValues.es3"))
        {
            Debug.Log("loads key");
            lastHealhtEvent = (Dictionary<DateTime, float>)ES3.Load("lastHealtEvent", "playerValues.es3");
        }

        if (ES3.KeyExists("lastManaEvent", "playerValues.es3"))
            lastManaEvent = (Dictionary<DateTime, float>)ES3.Load("lastManaEvent", "playerValues.es3");


        CalculateAllPlayerValues();
        playerValues.basicStatsValue.health = GetLifeSinceLastLifeEvent();
        playerValues.playerStats.mana = GetManaSinceLastManaEvent();
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
        lastXp = playerValues.playerStats.xp;
    }


    private void Update()
    {
        if (playerValues.playerStats.xp >= xpPerBase)
            CheckForLevelUp();

        if (playerValues.GetLife() < playerValues.BasicStatsValue.healthMax)
        {
            playerValues.basicStatsValue.health = GetLifeSinceLastLifeEvent();
        }

        if (playerValues.GetMana() < playerValues.playerStats.manaMax)
        {
            playerValues.playerStats.mana = GetManaSinceLastManaEvent();
        }

        if (Input.GetKeyUp("k"))
        {
            Debug.Log("k up");
            SetLifeForPlayerOutsideFight(5);
        }
    }

    private void OnEnable()
    {
        PotionInventory.PotionUsed += DrinkPotion;
        GearInventory.EquipGearChanged += delegate { CalculateAllPlayerValues(); };

        MonsterCharacter.MonsterKilled += AddToMonsterKilledList;
    }



    private void OnDisable()
    {
        PotionInventory.PotionUsed -= DrinkPotion;
        GearInventory.EquipGearChanged -= delegate { CalculateAllPlayerValues(); };
        MonsterCharacter.MonsterKilled -= AddToMonsterKilledList;
    }

    #endregion

    #region Functions

    public float GetLifeSinceLastLifeEvent()
    {
        if (lastHealhtEvent == null || lastHealhtEvent.Count == 0)
            return playerValues.basicStatsValue.healthMax;

        float newLife = 0;
        TimeSpan ts = DateTime.Now - lastHealhtEvent.Keys.First();
        newLife = MathF.Floor(lastHealhtEvent.Values.First() + ((float)ts.TotalSeconds / 60 * playerValues.playerStats.healthRegen));
        if (newLife > playerValues.basicStatsValue.healthMax)
            newLife = playerValues.basicStatsValue.healthMax;

        return newLife;



    }

    public float GetManaSinceLastManaEvent()
    {
        if (lastManaEvent == null || lastManaEvent.Count == 0)
            return playerValues.playerStats.manaMax;

        float newMana = 0;
        TimeSpan ts = DateTime.Now - lastManaEvent.Keys.First();
        newMana = MathF.Floor(lastManaEvent.Values.First() + ((float)ts.TotalSeconds / 60 * playerValues.playerStats.manaRegen));
        if (newMana > playerValues.playerStats.manaMax)
            newMana = playerValues.playerStats.manaMax;

        return newMana;



    }

    public int XPForNextlevel()
    {
        int xpNeeded = 0;
        for (int i = 0; i < playerValues.playerStats.level; i++)
        {
            xpNeeded += xpPerBase + (xpPerLevel * (i));
        }

        return xpNeeded;
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
        SetLifeForPlayerOutsideFight(playerValues.GetLife() + potionStats.addLife);
        SetManaForPlayerOutsideFight(playerValues.GetMana() + potionStats.addMana);

        Saver();
    }

    public void CheckForLevelUp()
    {
        if (playerValues.playerStats.xp >= XPForNextlevel())
        {
            while (playerValues.playerStats.xp >= XPForNextlevel())
            {
                LevelUp();
            }

        }
    }
    public void LevelUp()
    {
        playerValues.playerStats.level += 1;
        levelUPEvent(playerValues.playerStats.level);
    }

    public void AddBody()
    {
        playerValues.playerAttributesBasic.body += 1;
        CalculateAllPlayerValues();

    }

    public void AddCharisma()
    {
        playerValues.playerAttributesBasic.charisma += 1;
        CalculateAllPlayerValues();

    }

    public void AddMind()
    {
        playerValues.playerAttributesBasic.charisma += 1;
        CalculateAllPlayerValues();

    }

    public void CalculateAllPlayerValues()
    {
        CalculateCompleteAttributs();

        AttackandDefenseValue();

        CacluteHealthAndMana();

        Saver();
    }


    /// <summary>
    /// Calculates the offensive and defensive Values with attributes and gear
    /// </summary>
    public void AttackandDefenseValue()
    {
        //defense
        if (Inventory.GearInventory.ArmorEquiped == null)
        {
            playerValues.defensiveStatsValue.armor = ExtraArmor();
            playerValues.basicStatsValue.characterMovement = CharacterMovement.Boden;
            playerValues.basicStatsValue.characterType = CharacterType.Normal;
        }
        else if (Inventory.GearInventory.ArmorEquiped != null)
        {
            playerValues.defensiveStatsValue.armor = Inventory.GearInventory.ArmorEquiped.armorStats.armor + ExtraArmor();
            playerValues.basicStatsValue.characterMovement = Inventory.GearInventory.ArmorEquiped.armorMovementMod;
            playerValues.basicStatsValue.characterType = Inventory.GearInventory.ArmorEquiped.armorTypeMod;
        }

        //attack
        if (Inventory.GearInventory.WeaponEquipped != null)
        {
            playerValues.offensivStatsValue.attackDice = ExtraAttackWithoutWeapon();
            playerValues.offensivStatsValue.succesThreshold = 4;
            playerValues.offensivStatsValue.damageType = DamageType.Normal;
            playerValues.offensivStatsValue.weaponRange = WeaponRange.Nahkampf;
        }
        else if (Inventory.GearInventory.WeaponEquipped)
        {
            if (Inventory.GearInventory.WeaponEquipped.weaponStats.damageType == DamageType.Magisch)
                playerValues.offensivStatsValue.attackDice = ExtraAttackWithMagicWeapon() + Inventory.GearInventory.WeaponEquipped.weaponStats.attackDice;
            if (Inventory.GearInventory.WeaponEquipped.weaponStats.damageType == DamageType.Normal)
                playerValues.offensivStatsValue.attackDice = Inventory.GearInventory.WeaponEquipped.weaponStats.attackDice;

            playerValues.offensivStatsValue.succesThreshold = Inventory.GearInventory.WeaponEquipped.weaponStats.succesThreshold;
            playerValues.offensivStatsValue.damageType = Inventory.GearInventory.WeaponEquipped.weaponStats.damageType;
            playerValues.offensivStatsValue.weaponRange = Inventory.GearInventory.WeaponEquipped.weaponStats.weaponRange;
        }
    }

    public void CacluteHealthAndMana()
    {
        playerValues.basicStatsValue.healthMax = startLive + livePerLevel * (playerValues.playerStats.level - 1) + playerValues.playerAttributesComplete.body;
        playerValues.playerStats.healthRegen = playerValues.playerStats.level + playerValues.playerAttributesComplete.body;
        Debug.LogError(playerValues.playerStats.healthRegen + " = " + playerValues.playerStats.level + " + " + playerValues.playerAttributesComplete.body);


        playerValues.playerStats.manaMax = startmana + manaPerlevel * (playerValues.playerStats.level - 1) + playerValues.playerAttributesComplete.body;
        playerValues.playerStats.manaRegen = playerValues.playerStats.level + playerValues.playerAttributesComplete.body;
    }

    /// <summary>
    /// Calculates the Attribut Values with the gear applied
    /// </summary>
    public void CalculateCompleteAttributs()
    {
        playerValues.playerAttributesComplete.body = playerValues.playerAttributesBasic.body;
        playerValues.playerAttributesComplete.mind = playerValues.playerAttributesBasic.mind;
        playerValues.playerAttributesComplete.charisma = playerValues.playerAttributesBasic.charisma;

        playerValues.playerAttributesComplete.attributAddThreshold = playerValues.playerAttributesBasic.attributAddThreshold;

        if (Inventory.GearInventory.WeaponEquipped != null)
        {
            playerValues.playerAttributesComplete.body += Inventory.GearInventory.WeaponEquipped.attributeBuffs.body;
            playerValues.playerAttributesComplete.mind += Inventory.GearInventory.WeaponEquipped.attributeBuffs.mind;
            playerValues.playerAttributesComplete.charisma += Inventory.GearInventory.WeaponEquipped.attributeBuffs.charisma;
        }
        if (Inventory.GearInventory.ArmorEquiped != null)
        {
            playerValues.playerAttributesComplete.body += Inventory.GearInventory.ArmorEquiped.attributeBuffs.body;
            playerValues.playerAttributesComplete.mind += Inventory.GearInventory.ArmorEquiped.attributeBuffs.mind;
            playerValues.playerAttributesComplete.charisma += Inventory.GearInventory.ArmorEquiped.attributeBuffs.charisma;
        }
        if (Inventory.GearInventory.AmuletEquipped != null)
        {
            playerValues.playerAttributesComplete.body += Inventory.GearInventory.AmuletEquipped.attributeBuffs.body;
            playerValues.playerAttributesComplete.mind += Inventory.GearInventory.AmuletEquipped.attributeBuffs.mind;
            playerValues.playerAttributesComplete.charisma += Inventory.GearInventory.AmuletEquipped.attributeBuffs.charisma;
        }

    }

    public void SetLifeForPlayerOutsideFight(float lifeAmount)
    {
        Debug.Log("SetLifeForPlayerOutsideFight");

        playerValues.SetLife(lifeAmount);
        if (lastHealhtEvent != null)
            lastHealhtEvent.Clear();
        lastHealhtEvent.Add(DateTime.Now, lifeAmount);
        Saver();
    }

    public void SetManaForPlayerOutsideFight(float manaAmount)
    {
        playerValues.SetMana(manaAmount);
        if (lastManaEvent != null)
            lastManaEvent.Clear();
        lastManaEvent.Add(DateTime.Now, manaAmount);
        Saver();
    }

    //The extra Armor through the body
    public int ExtraArmor()
    {
        CalculateCompleteAttributs();
        return Mathf.FloorToInt((float)PlayerValues.PlayerAttributesComplete.body / (float)PlayerValues.PlayerAttributesComplete.attributAddThreshold);
    }

    //The extra Armor through the body
    public int ExtraAttackWithoutWeapon()
    {
        CalculateCompleteAttributs();
        return Mathf.FloorToInt((float)PlayerValues.PlayerAttributesComplete.body / (float)PlayerValues.PlayerAttributesComplete.attributAddThreshold);
    }

    public int ExtraAttackWithMagicWeapon()
    {
        CalculateCompleteAttributs();
        return Mathf.FloorToInt((float)PlayerValues.PlayerAttributesComplete.mind / (float)PlayerValues.PlayerAttributesComplete.attributAddThreshold);
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