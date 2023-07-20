
using Hexerspiel.Quests;
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

            uI_QuestItems[0].Bt_solve.onClick.AddListener(delegate { QuestTracker.Instance.SolveQuest(0); });
            uI_QuestItems[1].Bt_solve.onClick.AddListener(delegate { QuestTracker.Instance.SolveQuest(1); });
            uI_QuestItems[2].Bt_solve.onClick.AddListener(delegate { QuestTracker.Instance.SolveQuest(2); });

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

        #endregion
    }

}