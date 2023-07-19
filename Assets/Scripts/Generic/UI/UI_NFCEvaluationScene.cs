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


        #endregion

        #region Accessors
       
        #endregion

        #region LifeCycle
        private void Start()
        {
            EnableControls(true);
            UpdateLabel(NFCManager.nfcTagMessage);
        }

        private void Awake()
        {
            EnableControls(false);
            bt_rescan.onClick.AddListener(StartScanningAgain);
            bt_accept.onClick.AddListener(AcceptTag);

        }

        private void OnEnable()
        {
           
        }

        private void OnDisable()
        {
      
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
            Debug.Log("Update Label " + nfcText);
            label_scanedInfo.text = nfcText;
            if (NFCManager.Instance.ParsingSuccesfull)
            {
                bt_accept.interactable = true;
            }
            else
            {
                bt_accept.interactable = false;
            }
        }

        public void StartScanningAgain()
        {
            SceneManager.LoadScene("NFCScanScene");
        }

        public void AcceptTag()
        {
            NFCManager.Instance.AcceptScanedTag();
        }

        #endregion
    }

}
