using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_QuestItem : MonoBehaviour
    {

        #region Variables
        [SerializeField]
        private TextMeshProUGUI label_QuestItemHeader;

        [SerializeField]
        private Button bt_info, bt_solve, bt_abort;

        public Button Bt_info { get => bt_info;  }
        public Button Bt_solve { get => bt_solve; }
        public Button Bt_abort { get => bt_abort;  }
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {
            //EnableAllButton(false);
        }
        #endregion

        #region Functions
        public void FillHeader(string text)
        {
            label_QuestItemHeader.text = text;
        }

        public void SetToDisabled()
        {
            EnableAllButton(false);
            FillHeader("Keine Quest gespeichert... Auf gehts!");
        }

        public void EnableAllButton(bool state)
        {
            bt_info.interactable = state;
            bt_abort.interactable = state;
            bt_solve.interactable = state;
        }

        public void EnableOnQuest()
        {
            Debug.Log("enable buttos");
            bt_info.interactable = true;
            bt_abort.interactable = true;
            bt_solve.interactable = false;
        }

        public void EnableBTSolve(bool state)
        {
            bt_solve.interactable = state;
        }
        #endregion
    }

}