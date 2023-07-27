using Hexerspiel.Character;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using Hexerspiel.spots;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    #region Variables
    private static MainManager instance;

    public static event Action<string> AlertLeft = delegate { };
    [SerializeField]
    double timeToStayInSec = 180;

    public static double timeRemainingAtSpot;
    public static double timeRemainingAtNPC;

    public static DateTime spotEntered;
    public static DateTime npcEntered;

    #endregion

    #region Accessors
    public static MainManager Instance { get => instance; }



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
    }

    private void Update()
    {
        if (QuestTracker.currentSpot != null)
        {
            TimeSpan ts = DateTime.Now - spotEntered;
            timeRemainingAtSpot = timeToStayInSec - ts.TotalSeconds;

            if (timeRemainingAtSpot <= 0)
            {
                DisableCurrentSpot();
            }

            
        }

        if(QuestTracker.currentNPC != null)
        {
            TimeSpan tsNPC = DateTime.Now - npcEntered;
            timeRemainingAtNPC = timeToStayInSec - tsNPC.TotalSeconds;

            if (timeRemainingAtNPC <= 0)
            {
                DisableCurrentNPC();
            }
        }
    }


    #endregion

    #region Functions
    public static void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ApplyCurrentSpot(SO_spots newSpot)
    {
        QuestTracker.currentSpot = newSpot;
        SpotManager.LoadSpotScene();
        spotEntered = DateTime.Now;
    }



    public void DisableCurrentSpot()
    {
        AlertLeft("Du hast " + QuestTracker.currentSpot.nfcTagInfos.name + " verlassen");
        QuestTracker.currentSpot = null;
        if (QuestTracker.currentNPC == null ||( 0 == QuestTracker.currentNPC.gearToSell.Count && 0 == QuestTracker.currentNPC.potionToSell.Count && 0 == QuestTracker.currentNPC.questItemToSell.Count))
            UI_Inventory.atShop = false;

        if (SceneManager.GetActiveScene().name == "InventoryScene")
            MainManager.LoadMainScene();

    }


    public void ApplyCurrentNPC(SO_npc newNPC)
    {
        QuestTracker.currentNPC = newNPC;
        NPCManager.LoadNPCScene();
        npcEntered = DateTime.Now;
    }

    public void DisableCurrentNPC()
    {
        AlertLeft("Du bist nicht mehr bei " + QuestTracker.currentNPC.npcInformation.name);
        QuestTracker.currentNPC = null;
        if (QuestTracker.currentSpot == null || 0 == QuestTracker.currentSpot.shop.tier1Gear.Count)
            UI_Inventory.atShop = false;

        if (SceneManager.GetActiveScene().name == "InventoryScene")
            MainManager.LoadMainScene();

    }
    #endregion

}
