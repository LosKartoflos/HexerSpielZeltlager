using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_HeaderBar : MonoBehaviour
    {

        #region Variables
        private static UI_HeaderBar instance;

        [SerializeField]
        Button bt_back;

        [SerializeField]
        TextMeshProUGUI label_health, label_mana, label_level;

        [SerializeField]
        CanvasGroup header_cg;

        string newScene;
        string lastScene;

        private float lasthealth = 0, lastMana = 0;
        private int lastLevel = 0;
        #endregion

        #region Accessors
        public static UI_HeaderBar Instance { get => instance; set => instance = value; }

        
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
            bt_back.onClick.AddListener(MainManager.LoadMainScene);
            newScene = SceneManager.GetActiveScene().name;


        }

        private void Update()
        {
            if(lastLevel != Player.Instance.PlayerValues.PlayerStats1.level)
            {
                lastLevel = Player.Instance.PlayerValues.PlayerStats1.level;
                label_level.text = Player.Instance.PlayerValues.PlayerStats1.level.ToString();
            }

            if (lasthealth != Player.Instance.PlayerValues.BasicStatsValue.health)
            {
                lasthealth = Player.Instance.PlayerValues.BasicStatsValue.health;
                label_health.text = Player.Instance.PlayerValues.BasicStatsValue.health.ToString("0") + "/" + Player.Instance.PlayerValues.BasicStatsValue.healthMax.ToString("0");
            }

            if (lastMana != Player.Instance.PlayerValues.PlayerStats1.mana)
            {
                lastMana = Player.Instance.PlayerValues.PlayerStats1.mana;
                label_mana.text = Player.Instance.PlayerValues.PlayerStats1.mana.ToString("0") + "/" + Player.Instance.PlayerValues.PlayerStats1.manaMax.ToString("0");
            }

        }


        private void OnEnable()
        {
            SceneManager.activeSceneChanged += UpdateLastScene;
            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= UpdateLastScene;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        
        #endregion

        #region Functions
       
        private void UpdateLastScene(Scene current, Scene next)
        {
         
            lastScene = newScene;
            newScene = next.name;
           // Debug.Log("Lastscene  " + lastScene);
        }

        public void LoadLastScene()
        {
            SceneManager.LoadScene(lastScene);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("scene loaded " + scene.name);
            if (scene.name == "FightScene")
            {
                UI_Tools.SetCanvasGroup(header_cg, false);
            }
            else
            {
                UI_Tools.SetCanvasGroup(header_cg, true);
            }
        }
        #endregion
    }
}