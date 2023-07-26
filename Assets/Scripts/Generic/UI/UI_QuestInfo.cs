using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UI_QuestInfo : MonoBehaviour
    {

        #region Variables
        public static event Action<bool> DecisionQuestAccept = delegate { };

        [SerializeField]
        GameObject containerButtonInfo, containerButtonAccept;

        [SerializeField]
        TextMeshProUGUI label_header, label_QuestInfo;

        [SerializeField]
        Button bt_accept, bt_decline, bt_okay;

        [SerializeField]
        ScrollRect scrollRect;


        #endregion

        #region Accessors

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
        #endregion

        #region Functions

        public void OpenAsInfoPanel(string info, string header)
        {
            label_header.text = header;
            label_QuestInfo.text = info;

            containerButtonInfo.SetActive(true);
            containerButtonAccept.SetActive(false);

            bt_okay.onClick.AddListener(delegate { Destroy(gameObject); });
        }

        public void OpenForAccept(string info, string header)
        {
            label_header.text = header;
            label_QuestInfo.text = info;

            containerButtonInfo.SetActive(false);
            containerButtonAccept.SetActive(true);

            bt_accept.onClick.AddListener(delegate { DecisionQuestAccept(true); Destroy(gameObject); });
            bt_decline.onClick.AddListener(delegate { DecisionQuestAccept(false); Destroy(gameObject); });
        }

        public void OpenForAcceptOnly(string info, string header)
        {
            label_header.text = header;
            label_QuestInfo.text = info;

            containerButtonInfo.SetActive(false);
            containerButtonAccept.SetActive(true);


            bt_decline.gameObject.SetActive(false);

            bt_accept.onClick.AddListener(delegate {  Destroy(gameObject); });
            
        }

        #endregion
    }
}
