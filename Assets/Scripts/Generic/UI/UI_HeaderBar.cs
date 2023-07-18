using System;
using System.Collections;
using System.Collections.Generic;
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


        string newScene;
        string lastScene;


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
            bt_back.onClick.AddListener(LoadLastScene);
            newScene = SceneManager.GetActiveScene().name;


        }
        private void OnEnable()
        {
            SceneManager.activeSceneChanged += UpdateLastScene;
            
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= UpdateLastScene;
        }

        #endregion

        #region Functions
        private void UpdateLastScene(Scene current, Scene next)
        {
         
            lastScene = newScene;
            newScene = next.name;
            Debug.Log("Lastscene  " + lastScene);
        }

        public void LoadLastScene()
        {
            SceneManager.LoadScene(lastScene);
        }
        #endregion
    }
}