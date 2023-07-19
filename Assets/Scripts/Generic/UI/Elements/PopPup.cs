using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class PopPup : MonoBehaviour
    {

        #region Variables
        [SerializeField]
        TextMeshProUGUI label;

        [SerializeField]
        Button bt_close;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Awake()
        {
            bt_close.onClick.AddListener(delegate { Destroy(gameObject); });
        }
        #endregion

        #region Functions
        public void FillLabel(string text)
        {
            label.text = text;
        }
        #endregion
    }

}