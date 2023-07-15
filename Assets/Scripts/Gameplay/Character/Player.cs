using Hexerspiel.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public static Player Instance { get => instance;  }

    public Inventory Inventory { get => inventory; set => inventory = value; }
    public PlayerCharacterValues PlayerValues { get => playerValues; set => playerValues = value; }

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
    #endregion
}
