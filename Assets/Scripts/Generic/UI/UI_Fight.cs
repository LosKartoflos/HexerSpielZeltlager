using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Fight : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Button bt_fight;

    [SerializeField]
    Button bt_run;

    [SerializeField]
    Button bt_usePotion;

    [SerializeField]
    Button bt_ChangeEquipment;

    [SerializeField]
    Button bt_useSpell;

    #endregion

    #region Accessors
    private void Start()
    {
        Debug.Log("UI fight is on" + gameObject.name);
        //Test potionuse

        //bt_usePotion.onClick.AddListener(delegate { });

        //if (bt_usePotion == null)
        //{
        //    Debug.Log("Bt us potion == null");
        //}
        //else
        //    bt_fight.onClick.AddListener(DrinkTestpotion);
    }


    #endregion

    #region LifeCycle
    #endregion

    #region Functions
    public void DrinkTestpotion()
    {
        Debug.Log("drink potion");
        Player.Instance.Inventory.PotionInventory.UsePotion(Player.Instance.Inventory.PotionInventory.PotionList[0]);
    }
    #endregion
}
