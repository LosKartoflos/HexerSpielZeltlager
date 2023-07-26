
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
        private GameObject multipleChoicePrefab;

        [SerializeField]
        private GameObject freeEntryPrefab;

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
                    QuestItems[i].FillHeader(QuestTracker.QuestSteps[i].stepName + " | " + QuestTracker.QuestSteps[i].GetShortLootText());
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

        public void CreateInfoPopUpAcceptOnly(string info, string header)
        {
            GameObject questInfoObject = Instantiate(questInfoPrefab, popUpPanel, false);
            questInfoObject.SetActive(false);
            questInfoObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            questInfoObject.GetComponent<UI_QuestInfo>().OpenForAcceptOnly(info, header);
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

        /// <summary>
        /// wir aufgerunfen wenn der Lösen Button gedrückt wird
        /// </summary>
        /// <param name="index"></param>
        private void SolveQuest(int index)
        {


            QuestTracker.questToSolveIndex = index;

            //QuestTracker.QuestSteps[index]???

    

            //multipleChoice
            if (QuestTracker.QuestSteps[index].QuestStepTarget == QuestTarget.multipleChoiceQuiz || QuestTracker.QuestSteps[index].QuestStepTarget == QuestTarget.multipleChoiceAttribute)
            {
                GameObject mulitpleChoiceObject = Instantiate(multipleChoicePrefab, popUpPanel, false);
                mulitpleChoiceObject.SetActive(false);
                mulitpleChoiceObject.SetActive(true);
                Canvas.ForceUpdateCanvases();
                //noch nicht gelöst

                if (QuestTracker.QuestSteps[index].QuestStepTarget == QuestTarget.multipleChoiceQuiz)
                    mulitpleChoiceObject.GetComponent<MultipleChoicePopUp>().IntiatePopUp(QuestTracker.Instance.QuestSolvedText(index), QuestTracker.multipleChoiceRecieverID, ((SO_step_multipleChoiceQuiz)QuestTracker.QuestSteps[index]).answersTexts);

                else if (QuestTracker.QuestSteps[index].QuestStepTarget == QuestTarget.multipleChoiceAttribute)
                {
                    mulitpleChoiceObject.GetComponent<MultipleChoicePopUp>().IntiatePopUp(QuestTracker.Instance.QuestSolvedText(index), QuestTracker.multipleChoiceRecieverID, ((SO_step_mulitpleChoiceAttribute)QuestTracker.QuestSteps[index]).answers);
                }
            }
            //free entry
            else if (QuestTracker.QuestSteps[index].QuestStepTarget == QuestTarget.freeEntry)
            {
                GameObject freeEntryObject = Instantiate(freeEntryPrefab, popUpPanel, false);
                freeEntryObject.SetActive(false);
                freeEntryObject.SetActive(true);
                Canvas.ForceUpdateCanvases();

                freeEntryObject.GetComponent<FreeEntryPopUp>().IntiatePopUp(((SO_step_FreeEntry)QuestTracker.QuestSteps[index]).shortQuestion + "\n gib die Lösung ein!", QuestTracker.freeEntryRecieverID);

            }
            //other
            else
            {
                if (!QuestTracker.QuestSteps[index].CheckIfStepIsSolved() && QuestTracker.QuestSteps[index].QuestStepTarget != QuestTarget.freeEntry)
                {
                    QuestTracker.Instance.SolveTextByIdAlert(index);
                    return;
                }

                GameObject yesNoPopUpObject = Instantiate(yesNoPopUPPrefab, popUpPanel, false);
                yesNoPopUpObject.SetActive(false);
                yesNoPopUpObject.SetActive(true);
                Canvas.ForceUpdateCanvases();
                yesNoPopUpObject.GetComponent<YesNoPopUP>().IntiatePopUp(QuestTracker.Instance.QuestSolvedText(index), QuestTracker.solveRecieverID);
            }



        }


        #endregion
    }

}