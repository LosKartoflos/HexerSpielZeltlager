using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hexerspiel.Fight;
using System;
using Hexerspiel.Character;
using Hexerspiel.UI;

namespace Hexerspiel.spots
{
    public class SpotManager : MonoBehaviour
    {
        #region Variables
        private static SpotManager instance;
        public static event Action<string> AlertSpot;

        public static SO_spots currentStpot;

        [SerializeField]
        List<SO_Monster> monsterList = new List<SO_Monster>();

        [SerializeField]
        SO_Monster boss;

        [SerializeField]
        List<SO_item> itemsToShop = new List<SO_item>();

        int minLevel = 0, maxLevel = 0;


        #endregion

        #region Accessors
        public static SpotManager Instance { get => instance; }
        public List<SO_Monster> MonsterList { get => monsterList; }
        public SO_Monster Boss { get => boss; }
        public List<SO_item> ItemsToShop { get => itemsToShop; }
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

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnDestroy()
        {
            DestroySpotManager();
        }
        #endregion

        #region Functions
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("scene loaded " + scene.name);
            if (scene.name == "SpotScene")
            {
                SetupSpotParameters();
            }
        }
        public void SetupSpotParameters()
        {
            monsterList = new List<SO_Monster>();
            boss = null;
            itemsToShop = new List<SO_item>();

            if (QuestTracker.currentSpot == null)
            {

                return;
            }
            else if (QuestTracker.currentSpot != null)
            {
                if (QuestTracker.currentSpot.fightingGround.monsters != null)
                    monsterList.AddRange(QuestTracker.currentSpot.fightingGround.monsters);
                if (QuestTracker.currentSpot.fightingGround.boss != null)
                    boss = QuestTracker.currentSpot.fightingGround.boss;
                if (QuestTracker.currentSpot.shop.tier1Gear != null && QuestTracker.currentSpot.shop.tier1Gear.Count > 0)
                {
                    itemsToShop.AddRange(QuestTracker.currentSpot.shop.tier1Gear);
                    UI_Inventory.shopItemList = new List<SO_item>();
                    UI_Inventory.shopItemList.AddRange(itemsToShop);
                    UI_Inventory.atShop = true;
                }
                else
                {
                    UI_Inventory.atShop = true;
                    UI_Inventory.shopItemList = new List<SO_item>();
                }


                minLevel = QuestTracker.currentSpot.fightingGround.minLevel;
                maxLevel = QuestTracker.currentSpot.fightingGround.maxLevel;
            }

            UI_Spot.Instance.SetupSpotUI(monsterList, boss, itemsToShop, minLevel, maxLevel);
        }

        public void DestroySpotManager()
        {
            currentStpot = null;
            Destroy(gameObject);
        }

        public static void LoadSpotScene()
        {
            SceneManager.LoadScene("SpotScene");
        }

        public double CoolDownRemaining()
        {
            double cooldDown = 900;
            double cooldDownRemaining = 0;
            if (Player.Instance.PlayerValues.foughtAtSpot.ContainsKey(QuestTracker.currentSpot.name))
            {

                TimeSpan tsCooldown = DateTime.Now - Player.Instance.PlayerValues.foughtAtSpot[QuestTracker.currentSpot.name];
                cooldDownRemaining = cooldDown - tsCooldown.TotalSeconds;
                return cooldDownRemaining;
            }

            return 0;
        }

        public static bool CheckIfLastFightCooldown()
        {
            if (QuestTracker.currentSpot.name == null)
                return false;

            double cooldDown = 900;
            double cooldDownRemaining = 0;
            if (Player.Instance.PlayerValues.foughtAtSpot.ContainsKey(QuestTracker.currentSpot.name))
            {

                TimeSpan tsCooldown = DateTime.Now - Player.Instance.PlayerValues.foughtAtSpot[QuestTracker.currentSpot.name];
                cooldDownRemaining = cooldDown - tsCooldown.TotalSeconds;

                if (cooldDownRemaining <= 0)
                {
                    return true;

                }
                else
                    return false;

            }
            else
            {
                return true;
            }


        }

        public static SO_Monster GetRandomMonster(List<SO_Monster> monsterList)
        {
            if (monsterList == null || monsterList.Count == 0)
            {
                Debug.Log("The monsterList must not be empty or null.");
            }


            int randomIndex = DateTime.Now.Second % monsterList.Count;
            return monsterList[randomIndex];
        }

        public void StartFightWithRandomMonster()
        {
            if (CheckIfLastFightCooldown())
            {
                Player.Instance.AddToSpotFoughtList(QuestTracker.currentSpot.name, DateTime.Now);
                Hexerspiel.Fight.Fight.StartFightingScene(GetRandomMonster(monsterList));
            }

            else
                AlertSpot("Der Cooldown ist nicht vorbei. Komme in " + CoolDownRemaining().ToString("0") + " Sekunden wieder");
        }

        public void StartFightWithBoss()
        {
            if (CheckIfLastFightCooldown())
            {
                Player.Instance.AddToSpotFoughtList(QuestTracker.currentSpot.name, DateTime.Now);
                Hexerspiel.Fight.Fight.StartFightingScene(boss);
            }

            else
                AlertSpot("Der Cooldown ist nicht vorbei. Komme in " + CoolDownRemaining().ToString("0") + " Sekunden wieder");
        }

        #endregion
    }
}
