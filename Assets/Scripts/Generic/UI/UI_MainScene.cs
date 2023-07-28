using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Fight;
using Hexerspiel.Quests;
using Hexerspiel.spots;
using Hexerspiel.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainScene : MonoBehaviour
{
    #region Variables


    [SerializeField]
    ItemEquipElement bt_seePlace, bt_seeNpc, bt_scan, bt_quest, bt_inventory;

    [SerializeField]
    TextMeshProUGUI label_Attributs;

    [SerializeField]
    Bar xpBar;

    #endregion

    #region Accessors
    #endregion

    #region LifeCycle
    #endregion

    #region Functions

    private void Start()
    {
        label_Attributs.text = string.Format("Körper: {0} | Geist: {1} | Charisma: {2}\nW: {3} | ES: +{4} | MW: {6} | MP: {7}| R: {5}",
            Player.Instance.PlayerValues.PlayerAttributesComplete.body.ToString(),
             Player.Instance.PlayerValues.PlayerAttributesComplete.mind.ToString(),
             Player.Instance.PlayerValues.PlayerAttributesComplete.charisma.ToString(),
              Player.Instance.PlayerValues.OffensivStatsValue.attackDice.ToString(),
               Player.Instance.PlayerValues.OffensivStatsValue.succesThreshold.ToString(),
                Player.Instance.PlayerValues.DeffensiveStatsValue.armor.ToString(),
                -1, -1);

        xpBar.SetValues(Player.Instance.PlayerValues.PlayerStats1.xp, Player.Instance.XPForNextlevel() , " XP");
        //xpBar.SetValues(7, 100, " XP");
    }

    private void Update()
    {
        if (QuestTracker.currentSpot != null && SceneManager.GetActiveScene().name == "MainScene")
        {
            if (bt_seePlace.enabled == false)
                bt_seePlace.Button.interactable = true;
            bt_seePlace.ChangeAppreance("Gehe zu " + QuestTracker.currentSpot.nfcTagInfos.name, "Noch am Ort für " + MainManager.timeRemainingAtSpot.ToString("0") + " Sek.");
        }
        else if (QuestTracker.currentSpot == null && SceneManager.GetActiveScene().name == "MainScene" && bt_seePlace.enabled)
        {
            bt_seePlace.Button.interactable = false;
            bt_seePlace.ChangeAppreance("Kein Ort vorhanden", "Scanne wieder einen Ort");
        }

        if (QuestTracker.currentNPC != null && SceneManager.GetActiveScene().name == "MainScene")
        {
            if (bt_seeNpc.enabled == false)
                bt_seeNpc.Button.interactable = true;
            bt_seeNpc.ChangeAppreance("Besuche " + QuestTracker.currentNPC.npcInformation.name, "Noch bei Person für " + MainManager.timeRemainingAtNPC.ToString("0") + "Sek");
        }
        else if (QuestTracker.currentNPC == null && SceneManager.GetActiveScene().name == "MainScene" && bt_seeNpc.enabled)
        {
            bt_seeNpc.Button.interactable = false;
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
