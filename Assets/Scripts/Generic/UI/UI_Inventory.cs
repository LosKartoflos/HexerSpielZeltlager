using Hexerspiel.Character;
using Hexerspiel.Quests;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_Inventory : MonoBehaviour
    {

        #region Variables
        private static UI_Inventory instance;

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
        #endregion

        #region Accessors
        public static UI_Inventory Instance { get => instance; }

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

            QuestTracker.AlertQuesTracker += ActivateAndFillRecievePanel;
        }

        private void UnsubsrcibeEvents()
        {
            BasicInventory.AlertBasicInventoryChange -= ActivateAndFillRecievePanel;
            GearInventory.AlertGearChanged -= ActivateAndFillRecievePanel;
            PotionInventory.AlertPotionChanged -= ActivateAndFillRecievePanel;
            QuestItemInventory.AlertQuestItemChanged -= ActivateAndFillRecievePanel;

            QuestTracker.AlertQuesTracker -= ActivateAndFillRecievePanel;
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