using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.Generic
{
    public class SaveManagment : MonoBehaviour
    {

        #region Variables
        private static SaveManagment instance;
        [SerializeField]
        ES3AutoSaveMgr es3autosaver;


        #endregion

        #region Accessors
        public static SaveManagment Instance { get => instance; }

        public static Action SaveEvent = delegate { };
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
            SceneManager.sceneUnloaded += Save;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= Save;
        }
        #endregion

        #region Functions
        public void Save(Scene scene)
        {
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                SaveEvent();
                Debug.Log("Save");
            }
               
        }
        #endregion
    }

}