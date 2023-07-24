using Hexerspiel.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    public class InventorySceneManager : MonoBehaviour
    {

        #region Variables
        private static InventorySceneManager instance;

        public static UIObjectItem currentObjectItem;
        #endregion

        #region Accessors
        public static InventorySceneManager Instance { get => instance; }
        #endregion

        #region LifeCycle
        private void Awake()
        {
            instance = this;
        }
        #endregion

        #region Functions
        #endregion
    }

}