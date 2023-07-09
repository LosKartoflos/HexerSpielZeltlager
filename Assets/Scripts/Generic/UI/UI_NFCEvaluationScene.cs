using DigitsNFCToolkit.Samples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_NFCEvaluationScene : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        TextMeshProUGUI label_scanedInfo;

        [SerializeField]
        Button bt_accept, bt_rescan;

        [SerializeField]
        NFCManager nfcManager;

        #endregion

        #region Accessors
        private void Awake()
        {
            EnableControls(false);
            bt_rescan.onClick.AddListener(StartScanningAgain);
           
        }

        private void OnEnable()
        {
            //NFCMessenger.tagInfoEvent += UpdateLabel;
            //NFCReadControl.tagReadEvent += delegate { EnableControls(true); };
        }
        #endregion

        #region LifeCycle
        private void Start()
        {
            UpdateLabel(NFCManager.nfcTagMessage);
            EnableControls(true);
        }
        #endregion

        #region Functions
        public void EnableControls(bool state)
        {
            bt_accept.interactable = state;
            bt_rescan.interactable = state;
        }

        public void UpdateLabel(string nfcText)
        {
            label_scanedInfo.text = nfcText;
        }

        public void StartScanningAgain()
        {
            SceneManager.LoadScene("NFCScanScene");
        }

        #endregion
    }

}
