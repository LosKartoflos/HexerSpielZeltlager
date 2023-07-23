using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class FreeEntryPopUp : MonoBehaviour
    {
        #region Variables
        public static event Action<string, string> SubmitAnswer = delegate { };


        private string recieverId;

        [SerializeField]
        TextMeshProUGUI label;

        [SerializeField]
        TMP_InputField inputField;

        [SerializeField]
        Button bt_yes, bt_no;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Awake()
        {
            bt_yes.onClick.AddListener(delegate { SubmitAnswer(inputField.text, recieverId); Destroy(gameObject); });
            bt_no.onClick.AddListener(delegate {  Destroy(gameObject); });
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
            this.recieverId = _recieverId;
        }
        #endregion
    }
}
