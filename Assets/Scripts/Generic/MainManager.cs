using Hexerspiel.Character;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using Hexerspiel.spots;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    #region Variables
    public static bool mainManagerHasLoaded = false;

    private static MainManager instance;

    public static event Action<string> AlertLeft = delegate { };
    [SerializeField]
    double timeToStayInSec = 300;

    public static double timeRemainingAtSpot;
    public static double timeRemainingAtNPC;

    public static DateTime spotEntered;
    public static DateTime npcEntered;

    public void Saver()
    {
        ES3.Save("spotEntered", spotEntered);
        ES3.Save("npcEntered", npcEntered);

        Debug.Log("Save MainManager");
    }

    public void Loader()
    {
        if (ES3.KeyExists("spotEntered"))
            spotEntered = (DateTime)ES3.Load("spotEntered");
        if (ES3.KeyExists("npcEntered"))
            npcEntered = (DateTime)ES3.Load("npcEntered");

        mainManagerHasLoaded = true;

        Debug.Log("Load MainManager");
    }
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

        Loader();
    }

    private void Update()
    {
        if(QuestTracker.hasLoaded && mainManagerHasLoaded )

        if (QuestTracker.currentSpot != null)
        {
            TimeSpan ts = DateTime.Now - spotEntered;
            timeRemainingAtSpot = timeToStayInSec - ts.TotalSeconds;

            if (timeRemainingAtSpot <= 0)
            {
                DisableCurrentSpot();

            }


        }

        if (QuestTracker.currentNPC != null)
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
    public void DeletSafes()
    {
        foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
        {
            DirectoryInfo data_dir = new DirectoryInfo(directory);
            data_dir.Delete(true);
        }

        foreach (var file in Directory.GetFiles(Application.persistentDataPath))
        {
            FileInfo file_info = new FileInfo(file);
            file_info.Delete();
        }
    }

    public static void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ApplyCurrentSpot(SO_spots newSpot)
    {
        QuestTracker.currentSpot = newSpot;
        SpotManager.LoadSpotScene();
        spotEntered = DateTime.Now;
        QuestTracker.Instance.Saver();
        Saver();
    }



    public void DisableCurrentSpot()
    {
        AlertLeft("Du hast " + QuestTracker.currentSpot.nfcTagInfos.name + " verlassen");
        QuestTracker.currentSpot = null;
        if (QuestTracker.currentNPC == null || (0 == QuestTracker.currentNPC.gearToSell.Count && 0 == QuestTracker.currentNPC.potionToSell.Count && 0 == QuestTracker.currentNPC.questItemToSell.Count))
            UI_Inventory.atShop = false;

        if (SceneManager.GetActiveScene().name == "InventoryScene")
            MainManager.LoadMainScene();
        QuestTracker.Instance.Saver();
        Saver();

    }


    public void ApplyCurrentNPC(SO_npc newNPC)
    {
        QuestTracker.currentNPC = newNPC;
        NPCManager.LoadNPCScene();
        npcEntered = DateTime.Now;

        QuestTracker.Instance.Saver();
        Saver();
    }

    public void DisableCurrentNPC()
    {
        AlertLeft("Du bist nicht mehr bei " + QuestTracker.currentNPC.npcInformation.name);
        QuestTracker.currentNPC = null;
        QuestTracker.questsOfferedByNPC = null;

        if (QuestTracker.currentSpot == null || 0 == QuestTracker.currentSpot.shop.tier1Gear.Count)
            UI_Inventory.atShop = false;

        if (SceneManager.GetActiveScene().name == "NPCScene")
            MainManager.LoadMainScene();
        QuestTracker.Instance.Saver();
        Saver();
    }
    #endregion

}
