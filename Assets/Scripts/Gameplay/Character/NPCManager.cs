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
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            DestroyNPCManager();
        }


        #endregion

        #region Functions
        private void DestroyNPCManager()
        {
            currentNpc = null;
            Destroy(gameObject);
        }

        public static void LoadNPCScene()
        {
            SceneManager.LoadScene("NPCScene");
        }
        #endregion

    } 
}
