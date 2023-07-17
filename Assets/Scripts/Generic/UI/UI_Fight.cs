using Hexerspiel.Character.monster;
using Hexerspiel.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Fight : MonoBehaviour
{

    public const string PLAYER_INFO_FORMAT = "<b>Spieler:</b>\nHP: {0}\nMana{1}\n\nTyp: {12}\nBewegung: {13}\nWaffe: {14}, {15}\n\n<b>Verteidigung:</b>\nSchaden erhalten: {2} - {3} = {4}\n\n<b>Attacke:</b>\nWürfe: {5}\nErfolgsschwelle: {6}\nMod Punkte: {7}\nWürfe mod:{8}\n\n<b>Schaden:</b>\nErfolge:{9}\n+Waffenart und Gegnertypbonus: {10}\nGesamt: {11}";
    public const string Enemy_INFO_FORMAT = "<b>Gegner:</b>\nHP: {0}\n\n\nTyp: {11}\nBewegung: {12}\nWaffe: {13}, {14}\n\n<b>Verteidigung:</b>\nSchaden erhalten: {1} - {2} = {3}\n\n<b>Attacke</b>:\nWürfe: {4}\nErfolgsschwelle: {5}\nMod Punkte: {6}\nWürfe mod:{7}\n\n<b>Schaden:</b>\nErfolge:{8}\n+ Waffenart und Gegnertypbonus: {9}\nGesamt: {10}";

    #region Variables
    private static UI_Fight instance;

    [SerializeField]
    FightManager fightManager;

    [Header("Main Fight")]
    [SerializeField]
    private GameObject panel_fightInteractions;
    [SerializeField]
    Button bt_fight;

    [SerializeField]
    Button bt_run;

    [SerializeField]
    Button bt_usePotion;

    [SerializeField]
    GameObject bt_usePotionGO;

    [SerializeField]
    Button bt_ChangeEquipment;

    [SerializeField]
    Button bt_useSpell;

    [SerializeField]
    private TextMeshProUGUI label_playerInfo, label_enemyInfo, label_round;

    [Header ("WinScreen")]
    [SerializeField]
    private GameObject panel_AfterFightWon;

    [SerializeField]
    private TextMeshProUGUI label_won_info;

    [SerializeField]
    private TextMeshProUGUI label_won_lastHit;

    [SerializeField]
    private Button bt_continue_won;

    [Header("LooseScreen")]
    [SerializeField]
    private GameObject panel_AfterFightLoose;

    [SerializeField]
    private TextMeshProUGUI label_loose_info;

    [SerializeField]
    private Button bt_continue_loose;

    [SerializeField]
    private TextMeshProUGUI label_loose_lastHit;



    #endregion

    #region Accessors
    public static UI_Fight Instance { get => instance; }
   
 
    #endregion

    #region LifeCycle

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("UI fight is on" + gameObject.name);
        //Test potionuse

        bt_usePotion.onClick.AddListener(DrinkTestpotion);
        bt_fight.onClick.AddListener(fightManager.ProgressFight);

        //if (bt_usePotion == null)
        //{
        //    Debug.Log("Bt us potion == null");
        //}
        //else
        //    bt_fight.onClick.AddListener(DrinkTestpotion);

        panel_AfterFightLoose.SetActive(false);
        panel_AfterFightWon.SetActive(false);
        panel_fightInteractions.SetActive(true);
    }

    private void OnEnable()
    {
        FightManager.PlayerWonEvent += WinScreen;
        FightManager.PlayerLostEvent += LooseScreen;
    }
    #endregion

    #region Functions
    public void UpdateAfterFightRound(int round, string playerInfo, string enemyInfo)
    {
        label_round.text = "Runde: " + round.ToString();
        label_playerInfo.text = playerInfo;
        label_enemyInfo.text = enemyInfo;
    }

    public void WinScreen(MonsterCharacter monster)
    {
        panel_fightInteractions.SetActive(false);
        panel_AfterFightWon.SetActive(true);

        label_won_lastHit.text = label_playerInfo.text;
        label_won_info.text = "Ihr habt " + monster.MonsterName + " besiegt!\n\n" + Player.Instance.CollectedLoot;
    }

    public void LooseScreen()
    {
        panel_fightInteractions.SetActive(false);
        panel_AfterFightLoose.SetActive(true);

        label_loose_lastHit.text = label_enemyInfo.text;
        label_loose_info.text = "Leider seid ihr gestorben. Versucht es später noch einmal!";
    }
    public void DrinkTestpotion()
    {
        Debug.Log("drink potion");
        Player.Instance.Inventory.PotionInventory.UsePotion(Player.Instance.Inventory.PotionInventory.PotionList[0]);
    }
    #endregion
}
