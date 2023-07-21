
using Hexerspiel.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.UI
{
    public class UI_QuestTracker : MonoBehaviour
    {

        #region Variables
        private static UI_QuestTracker instance;

        [SerializeField]
        private UI_QuestItem[] uI_QuestItems = new UI_QuestItem[3];

        [SerializeField]
        private GameObject questInfoPrefab;

        [SerializeField]
        private GameObject yesNoPopUPPrefab;

        [SerializeField]
        private RectTransform popUpPanel;

        
        #endregion

        #region Accessors
        public static UI_QuestTracker Instance { get => instance; }
        public UI_QuestItem[] QuestItems { get => uI_QuestItems; }

       

        #endregion

        #region LifeCycle
        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // In first scene, make us the singleton.
            }
            else if (instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {

            uI_QuestItems[0].Bt_solve.onClick.AddListener(delegate { SolveQuest(0); });
            uI_QuestItems[1].Bt_solve.onClick.AddListener(delegate { SolveQuest(1); });
            uI_QuestItems[2].Bt_solve.onClick.AddListener(delegate { SolveQuest(2); });

            uI_QuestItems[0].Bt_info.onClick.AddListener(delegate { CreateInfoPopUp(QuestTracker.Instance.GetFullInfoText(0), QuestTracker.QuestSteps[0].stepName); });
            uI_QuestItems[1].Bt_info.onClick.AddListener(delegate { CreateInfoPopUp(QuestTracker.Instance.GetFullInfoText(1), QuestTracker.QuestSteps[1].stepName); });
            uI_QuestItems[2].Bt_info.onClick.AddListener(delegate { CreateInfoPopUp(QuestTracker.Instance.GetFullInfoText(2), QuestTracker.QuestSteps[2].stepName); });

            uI_QuestItems[0].Bt_abort.onClick.AddListener(delegate { DeleteStep(0); });
            uI_QuestItems[1].Bt_abort.onClick.AddListener(delegate { DeleteStep(1); });
            uI_QuestItems[2].Bt_abort.onClick.AddListener(delegate { DeleteStep(2); });

        }

     
        private void OnDestroy()
        {
            DestroyUI_QuesTracker();
        }


        #endregion

        #region Functions
        public void DestroyUI_QuesTracker()
        {

            Destroy(gameObject);
        }

        public void InitializeOnStartup()
        {
            for (int i = 0; i < QuestTracker.maximalQuestsTracked; i++)
            {
                if (QuestTracker.QuestSteps[i] == null)
                {
                    QuestItems[i].SetToDisabled();
                }
                else if (QuestTracker.QuestSteps[i] != null)
                {
                    QuestItems[i].FillHeader(QuestTracker.QuestSteps[i].stepName);
                    if (QuestTracker.NextQuestStep[i] == null)
                    {
                        QuestItems[i].EnableOnQuest();
                    }
                    else if (QuestTracker.NextQuestStep[i] != null)
                    {
                        QuestItems[i].EnableAllButton(true);
                    }
                }
            }
        }

        public void CreateInfoPopUp(string info, string header)
        {
            GameObject questInfoObject = Instantiate(questInfoPrefab, popUpPanel, false);
            questInfoObject.SetActive(false);
            questInfoObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            questInfoObject.GetComponent<UI_QuestInfo>().OpenAsInfoPanel(info, header);
        }

        public void CreateQuestStartPopUp(string info, string header)
        {
            GameObject questInfoObject = Instantiate(questInfoPrefab, popUpPanel, false);
            questInfoObject.SetActive(false);
            questInfoObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            questInfoObject.GetComponent<UI_QuestInfo>().OpenForAccept(info, header);
        }

        private void DeleteStep(int index)
        {
            QuestTracker.questToDelete = index;

            GameObject yesNoPopUpObject = Instantiate(yesNoPopUPPrefab, popUpPanel, false);
            yesNoPopUpObject.SetActive(false);
            yesNoPopUpObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            yesNoPopUpObject.GetComponent<YesNoPopUP>().IntiatePopUp("Willst du " + QuestTracker.QuestSteps[index].stepName + " abbrechen?", QuestTracker.delteRecieverID);

        }

        private void SolveQuest(int index)
        {
            QuestTracker.questToSolve = index;

            GameObject yesNoPopUpObject = Instantiate(yesNoPopUPPrefab, popUpPanel, false);
            yesNoPopUpObject.SetActive(false);
            yesNoPopUpObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            yesNoPopUpObject.GetComponent<YesNoPopUP>().IntiatePopUp(QuestTracker.Instance.SolveQuestText(index), QuestTracker.solveRecieverID);

        }


        #endregion
    }

}