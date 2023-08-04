using Hexerspiel.Character;
using Hexerspiel.Fight;
using Hexerspiel.Quests;
using Hexerspiel.spots;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_AlertPanel : MonoBehaviour
    {

        #region Variables
        private static UI_AlertPanel instance;

        [Header("Inventory")]
        [SerializeField]
        private CanvasGroup inventoryPanel;

        [Header("Recieve Panel")]
        [SerializeField]
        private GameObject recievePanelPrefab;
        [SerializeField]
        private RectTransform recievePanel;
        private Button bt_closeRecievePanel;

        private List<GameObject> recievePanels = new List<GameObject>();

        [Header("Level Up")]
        [SerializeField]
        private GameObject levelUpPrefab;

        #endregion

        #region Accessors
        public static UI_AlertPanel Instance { get => instance; }

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

        private void OnEnable()
        {
            SubsrcibeEvents();
        }

        private void OnDisable()
        {
            UnsubsrcibeEvents();
        }

        private void Start()
        {
            AssignButtons();

            UI_Tools.SetCanvasGroup(inventoryPanel, false);
        }
        #endregion

        #region Functions
        //================================================================================
        //General
        //================================================================================
        private void AssignButtons()
        {

        }

        private void SubsrcibeEvents()
        {
            BasicInventory.AlertBasicInventoryChange += ActivateAndFillRecievePanel;
            GearInventory.AlertGearChanged += ActivateAndFillRecievePanel;
            PotionInventory.AlertPotionChanged += ActivateAndFillRecievePanel;
            QuestItemInventory.AlertQuestItemChanged += ActivateAndFillRecievePanel;

            QuestTracker.AlertQuestTracker += ActivateAndFillRecievePanel;
            UI_Inventory.AlertLookUp += ActivateAndFillRecievePanel;

            MainManager.AlertLeft += ActivateAndFillRecievePanel;
            SpotManager.AlertSpot += ActivateAndFillRecievePanel;
            Player.levelUPEvent += CreateLevelUpPopUp;

            UI_Fight.AlertUIFight += ActivateAndFillRecievePanel;
            Spells.AlertSpell += ActivateAndFillRecievePanel;
        }

       

        private void UnsubsrcibeEvents()
        {
            BasicInventory.AlertBasicInventoryChange -= ActivateAndFillRecievePanel;
            GearInventory.AlertGearChanged -= ActivateAndFillRecievePanel;
            PotionInventory.AlertPotionChanged -= ActivateAndFillRecievePanel;
            QuestItemInventory.AlertQuestItemChanged -= ActivateAndFillRecievePanel;

            QuestTracker.AlertQuestTracker -= ActivateAndFillRecievePanel;
            UI_Inventory.AlertLookUp -= ActivateAndFillRecievePanel;

            MainManager.AlertLeft -= ActivateAndFillRecievePanel;
            SpotManager.AlertSpot -= ActivateAndFillRecievePanel;
            Player.levelUPEvent -= CreateLevelUpPopUp;

            UI_Fight.AlertUIFight -= ActivateAndFillRecievePanel;
            Spells.AlertSpell += ActivateAndFillRecievePanel;
        }


        private void CreateLevelUpPopUp(int obj)
        {
            GameObject newLevelUPopUP = Instantiate(levelUpPrefab, recievePanel);
            newLevelUPopUP.GetComponent<LevelUpPopUp>().FillLabel();
            recievePanels.Add(newLevelUPopUP);
        }
        //================================================================================
        //Inventory
        //================================================================================


        //================================================================================
        //Recieve Panel
        //================================================================================

        public void ActivateAndFillRecievePanel(string text)
        {
            Debug.Log("Creat RecievePanl with: " + text);
            GameObject newRecevieObject = Instantiate(recievePanelPrefab, recievePanel);
            newRecevieObject.GetComponent<PopPup>().FillLabel(text);
            newRecevieObject.transform.SetAsLastSibling();
            recievePanels.Add(newRecevieObject);

        }

        public void DestroyAllRecievePanel()
        {
            if(recievePanels.Count > 0)
            {
                foreach(GameObject go in recievePanels)
                {
                    Destroy(go);
                }
            }
        }
        #endregion
    }
}