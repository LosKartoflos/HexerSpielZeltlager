using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.spots
{
    public class SpotManager : MonoBehaviour
    {
        #region Variables
        private static SpotManager instance;

        public static SO_spots currentStpot;

        #endregion

        #region Accessors
        public static SpotManager Instance { get => instance; }
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
            DestroySpotManager();
        }
        #endregion

        #region Functions
        public void DestroySpotManager()
        {
            currentStpot = null;
            Destroy(gameObject);
        }
        #endregion
    } 
}
