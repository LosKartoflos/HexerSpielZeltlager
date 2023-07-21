using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class YesNoPopUP : MonoBehaviour
    {
        #region Variables
        public static event Action<string> YES = delegate { };
        public static event Action<string> NO = delegate { };

        private string revieverId;

        [SerializeField]
        TextMeshProUGUI label;

        [SerializeField]
        Button bt_yes, bt_no;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Awake()
        {
            bt_yes.onClick.AddListener(delegate { YES(revieverId); Destroy(gameObject); });
            bt_no.onClick.AddListener(delegate { NO(revieverId); Destroy(gameObject); });
        }
        #endregion

        #region Functions
        public void FillLabel(string text)
        {
            label.text = text;
        }

        public void IntiatePopUp(string text, string _recieverId)
        {
            FillLabel(text);
            this.revieverId = _recieverId;
        }
        #endregion
    }
}