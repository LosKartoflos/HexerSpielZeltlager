using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.Quests;
using Hexerspiel.spots;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hexerspiel.UI
{
    public class UI_Spot : MonoBehaviour
    {

        #region Variables
        private static UI_Spot instance;

        [SerializeField]
        ItemEquipElement bt_fight, bt_boss, bt_shop;

        [SerializeField]
        TextMeshProUGUI label_title;

        [SerializeField]
        GameObject nothingToDo;
        #endregion

        #region Accessors
        public static UI_Spot Instance { get => instance; }
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
        public void SetupSpotUI(
        List<SO_Monster> monsterList, SO_Monster boss, List<SO_item> itemsToShop, int min, int max)
        {

            bool nothingToDoTest = true;
            if (monsterList != null && monsterList.Count > 0)
            {
                bt_fight.Button.onClick.AddListener(delegate { SpotManager.Instance.StartFightWithRandomMonster(); });
                bt_fight.ChangeAppreance("Kämpfe gegen ein Monster", "Stufenempfehlung " + min.ToString() + " - " + max.ToString());
                nothingToDoTest = false;
            }
            else
            {
                bt_fight.gameObject.SetActive(false);
            }
            if (boss != null )
            {
                bt_boss.Button.onClick.AddListener(delegate { SpotManager.Instance.StartFightWithBoss(); });
                bt_boss.ChangeAppreance("Kämpfe gegen " + boss.monsterName,  "Es hat " + boss.offensivStats.attackDice + " Würfel und " + boss.basicStats.health + " Leben");
                nothingToDoTest = false;
            }
            else
            {
                bt_boss.gameObject.SetActive(false);
            }
            if(itemsToShop != null && itemsToShop.Count > 0)
            {
                bt_shop.Button.onClick.AddListener(delegate { InventorySceneManager.LoadScene(); });
                bt_shop.ChangeAppreance(null, null);
                nothingToDoTest = false;
            }
            else
            {
                bt_shop.gameObject.SetActive(false);
            }
            label_title.text = QuestTracker.currentSpot.nfcTagInfos.name;


            nothingToDo.SetActive(nothingToDoTest);
        }
        #endregion
    }

}