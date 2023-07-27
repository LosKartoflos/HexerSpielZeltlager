using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.Quests;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.Character
{
    public class NPCManager : MonoBehaviour
    {

        #region Variables
        private static NPCManager instance;

        public static SO_npc currentNpc;
        [SerializeField]
        SO_Monster boss;

        [SerializeField]
        List<SO_item> itemsToShop = new List<SO_item>();

        [SerializeField]
        List<SO_questStep> questSteps = new List<SO_questStep>();
        #endregion

        #region Accessors
        public static NPCManager Instance { get => instance; }
        #endregion

        #region LifeCycle

        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // In first scene, make us the singleton.
            }
            else if (instance != this)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            DestroyNPCManager();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion

        #region Functions
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("scene loaded " + scene.name);
            if (scene.name == "NPCScene")
            {
                SetupSpotParameters();
            }
        }

        private void DestroyNPCManager()
        {
            currentNpc = null;
            Destroy(gameObject);
        }

        public static void LoadNPCScene()
        {
            SceneManager.LoadScene("NPCScene");
        }

        public void SetupSpotParameters()
        {

            boss = null;
            itemsToShop = new List<SO_item>();
            questSteps = new List<SO_questStep>();

            if (QuestTracker.currentNPC == null)
            {

                return;
            }
            else if (QuestTracker.currentNPC != null)
            {

                if (QuestTracker.currentNPC.npcInFight != null)
                    boss = QuestTracker.currentNPC.npcInFight;

                if ((QuestTracker.currentNPC.gearToSell != null && QuestTracker.currentNPC.gearToSell.Count > 0)
                    || (QuestTracker.currentNPC.potionToSell != null && QuestTracker.currentNPC.potionToSell.Count > 0)
                    ||(QuestTracker.currentNPC.questItemToSell != null && QuestTracker.currentNPC.questItemToSell.Count > 0))
                {
                    itemsToShop.AddRange((QuestTracker.currentNPC.gearToSell));
                    itemsToShop.AddRange((QuestTracker.currentNPC.questItemToSell));
                    itemsToShop.AddRange((QuestTracker.currentNPC.potionToSell));
                    UI_Inventory.shopItemList = new List<SO_item>();
                    UI_Inventory.shopItemList.AddRange(itemsToShop);
                    UI_Inventory.atShop = true;
                }
                else
                {
                    UI_Inventory.atShop = false;
                    UI_Inventory.shopItemList = new List<SO_item>();
                }

                if(QuestTracker.currentNPC.questList != null && QuestTracker.currentNPC.questList.Count > 0){
                    questSteps.AddRange(QuestTracker.currentNPC.questList );
                    QuestTracker.questsOfferedByNPC = new List<SO_questStep>();
                    QuestTracker.questsOfferedByNPC.AddRange(QuestTracker.currentNPC.questList);
                }
                else
                {
                    QuestTracker.questsOfferedByNPC = new List<SO_questStep>();
                }


                //minLevel = QuestTracker.currentSpot.fightingGround.minLevel;
                //maxLevel = QuestTracker.currentSpot.fightingGround.maxLevel;
            }

            UI_NPC.Instance.SetupNPCUI( boss, itemsToShop, questSteps);
        }

        public void StartFightWithBoss()
        {
            Hexerspiel.Fight.Fight.StartFightingScene(boss);
        }
        #endregion

    } 
}
