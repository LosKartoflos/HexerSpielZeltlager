using Hexerspiel.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #endregion
}
