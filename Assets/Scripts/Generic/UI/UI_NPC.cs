using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hexerspiel.UI
{
    public class UI_NPC : MonoBehaviour
    {

        #region Variables
        private static UI_NPC instance;


        [SerializeField]
        ItemEquipElement bt_fight, bt_quest, bt_shop;

        [SerializeField]
        TextMeshProUGUI label_title;

        [SerializeField]
        GameObject nothingToDo;
        #endregion

        #region Accessors
        public static UI_NPC Instance { get => instance; }
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
        #endregion

        #region Functions
        public void SetupNPCUI( SO_Monster npcMonster, List<SO_item> itemsToShop, List<SO_questStep> questsToBegin)
        {

            bool nothingToDoTest = true;
            if (npcMonster != null )
            {
                bt_fight.Button.onClick.AddListener(delegate { NPCManager.Instance.StartFightWithBoss(); });
                bt_fight.ChangeAppreance("Kämpfe gegen diese Person", "Sie hat " + npcMonster.offensivStats.attackDice + " Würfel und " + npcMonster.basicStats.health + " Leben");
                nothingToDoTest = false;
            }
            else
            {
                bt_fight.gameObject.SetActive(false);
            }

            if (itemsToShop != null && itemsToShop.Count > 0)
            {
                bt_shop.Button.onClick.AddListener(delegate { InventorySceneManager.LoadScene(); });
                bt_shop.ChangeAppreance(null, null);
                nothingToDoTest = false;
            }
            else
            {
                bt_shop.gameObject.SetActive(false);
            }
            if (questsToBegin != null && questsToBegin.Count > 0)
            {
                bt_quest.Button.onClick.AddListener(delegate { QuestTracker.LoadQuestScene(); });
                bt_quest.ChangeAppreance(null, null);
                nothingToDoTest = false;
            }
            else
            {
                bt_quest.gameObject.SetActive(false);
            }

            label_title.text = QuestTracker.currentNPC.npcInformation.name;


            nothingToDo.SetActive(nothingToDoTest);
        }
        #endregion
    } 
}
