using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Fight;
using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_Fight : MonoBehaviour
    {

        public const string PLAYER_INFO_FORMAT = "<b>Spieler:</b>\nHP: {0}\nMana{1}\n\nTyp: {12}\nBewegung: {13}\nWaffe: {14}, {15}\n\n<b>Verteidigung:</b>\nSchaden erhalten: {2} - {3} = {4}\n\n<b>Attacke:</b>\nWürfe: {5}\nErfolgsschwelle: {6}\nMod Punkte: {7}\nWürfe mod:{8}\n\n<b>Schaden:</b>\nErfolge:{9}\n+Waffenart und Gegnertypbonus: {10}\nGesamt: {11}";
        public const string Enemy_INFO_FORMAT = "<b>Gegner:</b>\nHP: {0}\n\n\nTyp: {11}\nBewegung: {12}\nWaffe: {13}, {14}\n\n<b>Verteidigung:</b>\nSchaden erhalten: {1} - {2} = {3}\n\n<b>Attacke</b>:\nWürfe: {4}\nErfolgsschwelle: {5}\nMod Punkte: {6}\nWürfe mod:{7}\n\n<b>Schaden:</b>\nErfolge:{8}\n+ Waffenart und Gegnertypbonus: {9}\nGesamt: {10}";

        #region Variables
        private static UI_Fight instance;
        public static event Action<string> AlertUIFight = delegate { };

        [SerializeField]
        FightManager fightManager;

        [Header("player info")]
        public Bar health_player;
        public Bar mana_player;
        public TextMeshProUGUI info_dice_player, info_succes_player, info_extraDice_player, info_diceMode_player, info_armor_player;
        public List<GameObject> damageType_player = new List<GameObject>();
        public List<GameObject> movementType_player = new List<GameObject>();
        public List<GameObject> chacterType_player = new List<GameObject>();
        public List<GameObject> range_player = new List<GameObject>();



        [Header("player roll")]

        public TextMeshProUGUI roll_dice_player;
        public TextMeshProUGUI roll_diceMod_player, roll_succesMod_player, roll_succesTotal_player, roll_extraDamage_player, roll_monsterArmor_player, roll_finalDamage_player;

        [Header("enemy info")]
        public Bar health_enemy;
        public TextMeshProUGUI monsterNAme;
        public TextMeshProUGUI info_dice_enemy, info_succes_enemy, info_extraDice_enemy, info_diceMode_enemy, info_armor_enemy;
        public List<GameObject> damageType_enemy = new List<GameObject>();
        public List<GameObject> movementType_enemy = new List<GameObject>();
        public List<GameObject> chacterType_enemy = new List<GameObject>();
        public List<GameObject> range_enemy = new List<GameObject>();


        [Header("enemy roll")]

        public TextMeshProUGUI roll_dice_enemy;
        public TextMeshProUGUI roll_diceMod_enemy, roll_succesMod_enemy, roll_succesTotal_enemy, roll_extraDamage_enemy, roll_playerArmor_enemy, roll_finalDamage_enemy;




        [Header("Main Fight")]
        [SerializeField]
        private GameObject panel_fightInteractions;
        [SerializeField]
        Button bt_fight;

        [SerializeField]
        Button bt_run;

        [SerializeField]
        Button bt_usePotion, bt_Exit;

        [SerializeField]
        GameObject bt_usePotionGO, finalObject;

        [SerializeField]
        TextMeshProUGUI label_fightOverInfo;

        //[SerializeField]
        //Button bt_ChangeEquipment;

        [SerializeField]
        Button bt_useSpell;

        [SerializeField]
        private TextMeshProUGUI label_playerInfo, label_enemyInfo, label_round;

        [Header("Potion")]
        [SerializeField]
        GameObject uiObjectPrefab;

        [SerializeField]
        RectTransform contentContainer;

        [SerializeField]
        GameObject overlay;
        [SerializeField]
        private Button bt_use;
        [SerializeField]
        private Button bt_look, bt_closeOverlay;

        private UIObjectItem currentitemSelected;

        private ItemType itemTypeselectedByObjectItem = ItemType.none;
        private GearType gearTypeSelectedByObjectItem = GearType.none;
        //[Header("WinScreen")]
        //[SerializeField]
        //private GameObject panel_AfterFightWon;

        //[SerializeField]
        //private TextMeshProUGUI label_won_info;

        //[SerializeField]
        //private TextMeshProUGUI label_won_lastHit;

        //[SerializeField]
        //private Button bt_continue_won;

        //[Header("LooseScreen")]
        //[SerializeField]
        //private GameObject panel_AfterFightLoose;

        //[SerializeField]
        //private TextMeshProUGUI label_loose_info;

        //[SerializeField]
        //private Button bt_continue_loose;

        //[SerializeField]
        //private TextMeshProUGUI label_loose_lastHit;



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
            closeOverlay();
            Debug.Log("UI fight is on" + gameObject.name);
            //Test potionuse

            bt_fight.onClick.AddListener(fightManager.ProgressFight);
            bt_Exit.onClick.AddListener(UI_HeaderBar.Instance.LoadLastScene);
            bt_run.onClick.AddListener(FightManager.Instance.RunAway);
            bt_Exit.gameObject.SetActive(false);
            //bt_continue_loose.onClick.AddListener(UI_HeaderBar.Instance.LoadLastScene);

            //if (bt_usePotion == null)
            //{
            //    Debug.Log("Bt us potion == null");
            //}
            //else
            //    bt_fight.onClick.AddListener(DrinkTestpotion);

            //panel_AfterFightLoose.SetActive(false);
            //panel_AfterFightWon.SetActive(false);
            panel_fightInteractions.SetActive(true);

            label_fightOverInfo.text = "";

            bt_use.onClick.AddListener(delegate { UseItem(currentitemSelected.ItemAttached); });
            bt_look.onClick.AddListener(delegate { lookAtItem(currentitemSelected.ItemAttached); });
            bt_usePotion.onClick.AddListener(delegate { OpenOverlay(ItemType.potion); });
            bt_closeOverlay.onClick.AddListener(closeOverlay);

        }

        private void OnEnable()
        {
            FightManager.PlayerWonEvent += WinScreen;
            FightManager.PlayerLostEvent += LooseScreen;
            UIObjectItem.RecieveSelectedUIObjectItem += UIObjectItemSelected;


        }


        private void OnDisable()
        {
            FightManager.PlayerWonEvent -= WinScreen;
            FightManager.PlayerLostEvent -= LooseScreen;
            UIObjectItem.RecieveSelectedUIObjectItem -= UIObjectItemSelected;

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
            finalObject.gameObject.SetActive(true);
            bt_fight.gameObject.SetActive(false);
            bt_run.gameObject.SetActive(false);
            bt_usePotion.gameObject.SetActive(false);
            bt_useSpell.gameObject.SetActive(false);
            bt_Exit.gameObject.SetActive(true);
            label_fightOverInfo.text = "Ihr habt " + monster.MonsterName + " besiegt und" + monster.MonsterStat.xp.ToString() + " XP bekommen.\n\n" + Player.Instance.CollectedLoot;
        }

        public void LooseScreen()
        {
            finalObject.gameObject.SetActive(true);
            bt_fight.gameObject.SetActive(false);
            bt_run.gameObject.SetActive(false);
            bt_usePotion.gameObject.SetActive(false);
            bt_useSpell.gameObject.SetActive(false);
            bt_Exit.gameObject.SetActive(true);
            label_fightOverInfo.text = "Leider seid ihr gestorben. Versucht es später noch einmal!";
        }
        public void DrinkTestpotion()
        {
            Debug.Log("drink potion");
            Player.Instance.Inventory.PotionInventory.UsePotion(Player.Instance.Inventory.PotionInventory.PotionList[0]);
        }




        public void CreateObjectItem(SO_item newItem, bool select = false)
        {
            GameObject contentItem = Instantiate(uiObjectPrefab, contentContainer, false);

            //Setup
            contentItem.GetComponent<UIObjectItem>().SetupObjecItem(newItem, newItem.itemImage);



            //Highlight
            //if (contentContainer.childCount == 1)
            //    contentItem.GetComponent<UIObjectItem>().SetHighlight(true);
        }

        public void closeOverlay()
        {

            overlay.SetActive(false);

            while (contentContainer.childCount > 0)
            {
                DestroyImmediate(contentContainer.GetChild(0).gameObject);
            }


        }

        public void FillPotion()
        {
            if (Player.Instance.Inventory.PotionInventory.PotionList.Count == 0)
                return;


            List<SO_potion> sortedList = Player.Instance.Inventory.PotionInventory.PotionList.OrderBy(o => o.itemName).ToList();
            foreach (SO_potion armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor);
            }
        }

        public void UseItem(SO_item itemToProcess)
        {
            FightManager.potionAvailable = false;
            Player.Instance.Inventory.PotionInventory.UsePotion((SO_potion)itemToProcess);
            currentitemSelected.DeleteItem();
            closeOverlay();

        }

        public void UIObjectItemSelected(UIObjectItem newItem)
        {
            currentitemSelected = newItem;
            itemTypeselectedByObjectItem = newItem.ItemAttached.ItemType;

            if (newItem.ItemAttached.ItemType == ItemType.gear)
            {
                gearTypeSelectedByObjectItem = ((SO_gear)newItem.ItemAttached).GearType;
            }
            else
            {
                gearTypeSelectedByObjectItem = GearType.none;
            }

        }


        public void lookAtItem(SO_item itemToLook)
        {
            if (currentitemSelected != null)
                AlertUIFight(itemToLook.itemName + "\n\n" + ((SO_potion)itemToLook).GetDescription());
            else
                AlertUIFight("kein Item ausgewählt");
        }


        public void OpenOverlay(ItemType itemType, GearType gearType = GearType.none)
        {
            if (!FightManager.potionAvailable)
            {
                AlertUIFight("Es ist nur ein Trank pro Runde erlaubt");
                return;
            }


            //menuOpendForItem = itemType;
            //menuOpenedForGear = gearType;

            //highlightFirstEquipped = false;

            overlay.SetActive(true);

            switch (itemType)
            {
                case ItemType.potion:
                    FillPotion();
                    break;

            }
        }


        #endregion
    }
}
