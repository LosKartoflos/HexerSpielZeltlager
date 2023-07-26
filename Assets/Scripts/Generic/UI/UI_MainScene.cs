using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Fight;
using Hexerspiel.Quests;
using Hexerspiel.spots;
using Hexerspiel.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainScene : MonoBehaviour
{
    #region Variables
    

    [SerializeField]
    ItemEquipElement bt_seePlace, bt_seeNpc, bt_scan, bt_quest, bt_inventory;

    #endregion

    #region Accessors
    #endregion

    #region LifeCycle
    #endregion

    #region Functions


    private void Update()
    {
        if (QuestTracker.currentSpot != null && SceneManager.GetActiveScene().name == "MainScene")
        {
            if (bt_seePlace.enabled == false)
                bt_seePlace.enabled = true;
            bt_seePlace.ChangeAppreance("Gehe zu " + QuestTracker.currentSpot.nfcTagInfos.name, "Noch am ort für " + MainManager.timeRemainingAtSpot.ToString("0") + " Sek.");
        }
        else if (QuestTracker.currentSpot == null && SceneManager.GetActiveScene().name == "MainScene" && bt_seePlace.enabled)
        {
            bt_seePlace.enabled = false;
            bt_seePlace.ChangeAppreance("Kein Ort vorhanden", "Scanne wieder einen Ort");
        }

        if (QuestTracker.currentNPC != null && SceneManager.GetActiveScene().name == "MainScene")
        {
            if (bt_seeNpc.enabled == false)
                bt_seeNpc.enabled = true;
            //bt_seePlace.ChangeAppreance("Gehe zu " + QuestTracker.currentSpot.nfcTagInfos.name, "Noch bei Person für " + MainManager.timeRemainingAtSpot.ToString());
        }
        else if (QuestTracker.currentNPC == null && SceneManager.GetActiveScene().name == "MainScene" && bt_seeNpc.enabled)
        {
            bt_seeNpc.enabled = false;
            bt_seeNpc.ChangeAppreance("Keine Person vorhanden", "Scanne wieder eine Person");
        }


    }
    #endregion


    // Start is called before the first frame update
    private void Awake()
    {
        bt_scan.Button.onClick.AddListener(delegate { NFCManager.StartScanning(); });
        bt_quest.Button.onClick.AddListener(delegate { QuestTracker.LoadQuestScene(); });
        bt_inventory.Button.onClick.AddListener(delegate { InventorySceneManager.LoadScene(); });
        bt_seeNpc.Button.onClick.AddListener(delegate { NPCManager.LoadNPCScene(); });
        bt_seePlace.Button.onClick.AddListener(delegate { SpotManager.LoadSpotScene(); });
    }


}
