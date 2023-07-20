using Hexerspiel.nfcTags;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.Quests
{
    public class QuestTracker : MonoBehaviour
    {
        #region Variables
        private static QuestTracker instance;

        public const int maximalQuestsTracked = 3;

        [SerializeField]
        private static SO_questStep[] questSteps = new SO_questStep[maximalQuestsTracked];
        [SerializeField]
        private static SO_questStep[] nextQuestStep = new SO_questStep[maximalQuestsTracked];

        [SerializeField]
        private static List<SO_questStep> allreadyUsedSteps = new List<SO_questStep>();

        [SerializeField]
        private SO_questStep finishStep;

        private SO_spots currentSpot;
        private SO_npc currentNPC;

        public static SO_questStartTag questStartTag;
        public static SO_questSolveValidation questSolveValidation;

        public static event Action<string> AlertQuestTracker = delegate { };




        #endregion

        #region Accessors
        public static QuestTracker Instance { get => instance; }
        public static SO_questStep[] QuestSteps { get => questSteps; }
        public static SO_questStep[] NextQuestStep { get => nextQuestStep; }

        //public SO_spots CurrentSpot { get => currentSpot; }
        //public SO_npc CurrentNPC { get => currentNPC; }
        #endregion

        #region LifeCycle
        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // In first scene, make us the singleton.
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
                Destroy(gameObject);


        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }



        #endregion

        #region Functions
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("scene loaded " + scene.name);
            if (scene.name == "QuestScene")
            {
                CheckIfStepsAreSovleable();
                UI_QuestTracker.Instance.InitializeOnStartup();
            }
        }

        public void CheckQuests()
        {
            foreach (SO_questStep questStep in questSteps)
            {
                // if(questStep.TestIfStepIsSolved)
            }
        }

        public void CheckQuestSolverTag()
        {
            Debug.Log("CheckQuestSolverTag");
            questSolveValidation = null;
        }

        public bool CheckIfQuestIsAllreadyUsed(SO_questStep newQuest)
        {
            if (allreadyUsedSteps.Contains(newQuest))
            {
                AlertQuestTracker("Du hast diese Quest schon mal gestartet. Such die was neues!");
                return true;
            }
            else
                return false;
        }

        public void StartQuestWithTag()
        {
            if (CheckIfQuestIsAllreadyUsed(questStartTag.firstQuestStep))
            {
                questStartTag = null;
                return;
            }


            LoadQuestScene();
            Debug.Log("StartQuestWihtTag");
            StartQuest(questStartTag.firstQuestStep);
            questStartTag = null;

        }

        public void StartQuest(SO_questStep newQuest)
        {
            if (CheckIfQuestIsAllreadyUsed(newQuest))
                return;

            if (questSteps[0] == null)
            {
                questSteps[0] = newQuest;

            }
            else if (questSteps[1] == null)
            {
                questSteps[1] = newQuest;
                nextQuestStep[1] = null;
            }
            else if (questSteps[2] == null)
            {
                questSteps[2] = newQuest;
                nextQuestStep[2] = null;
            }
            else
            {
                AlertQuestTracker("Du hast schon 3 Quests!\nDu musst eine abschließen oder abbrechen!");
                return;
            }

            allreadyUsedSteps.Add(newQuest);

            //if allready in scene and next steps comes up
            if (UI_QuestTracker.Instance != null)
            {
                CheckIfStepsAreSovleable();
                UI_QuestTracker.Instance.InitializeOnStartup();
            }
            
            

            AlertQuestTracker("Du hast " + newQuest.stepName + " angenommen!");
        }


        public void CheckIfStepsAreSovleable()
        {
            for (int i = 0; i < maximalQuestsTracked; i++)
            {
                if (questSteps[i] != null)
                {
                    nextQuestStep[i] = questSteps[i].GetNextStepIfSolved();


                }

            }
        }

        /// <summary>
        /// things to do when quest is solved
        /// </summary>
        /// <param name="index">which quest is solved</param>
        public void SolveQuest(int index)
        {
            Debug.Log("solve index " + index);
            //check if quest is solved
            if (nextQuestStep[index] == null)
            {
                AlertQuestTracker("Du hast " + questSteps[index].stepName + " noch nicht gelöst");
                return;
            }
            //if no resolve text, use generic
            if (questSteps[index].resolvedText == "" || questSteps[index].resolvedText == null)
            {
                AlertQuestTracker("Du hast " + questSteps[index].stepName + " gelöst!");
            }
            //use resolve text
            else
            {
                AlertQuestTracker(questSteps[index].resolvedText);
            }

            //last step 
            if (questSteps[index] == finishStep)
            {
                questSteps[index] = null;
                nextQuestStep[index] = null;


                //to do: sort up
                //if(index == 0)
                //{
                //    if(questSteps[1] != null)

                //}

                UI_QuestTracker.Instance.QuestItems[index].SetToDisabled();
            }
            //load next step
            else
            {
                questSteps[index] = null;
                StartQuest(nextQuestStep[index]);
                nextQuestStep[index] = null;
                CheckIfStepsAreSovleable();
                UI_QuestTracker.Instance.InitializeOnStartup();
            }


            //to do: destroy if solved always!

        }

        public void DeleteStep(int questStepIndex)
        {

        }

        //public void ResetCurrentParameter()
        //{
        //    currentSpot = null;
        //    currentNPC = null;
        //}



        //public void SetCurrentSpot(SO_spots newSpot)
        //{
        //    currentSpot = newSpot;
        //}

        //public void SetCurrentNPC(SO_npc newNpc)
        //{
        //    currentNPC = newNpc;
        //}

        public static void LoadQuestScene()
        {
            SceneManager.LoadScene("QuestScene");

        }


        #endregion
    }

}