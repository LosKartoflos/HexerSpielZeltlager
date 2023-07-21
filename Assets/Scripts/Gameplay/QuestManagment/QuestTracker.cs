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
        private static List<SO_questStep> stepsDoneForQuestlineIndex0 = new List<SO_questStep>();
        [SerializeField]
        private static List<SO_questStep> stepsDoneForQuestlineIndex1 = new List<SO_questStep>();
        [SerializeField]
        private static List<SO_questStep> stepsDoneForQuestlineIndex2 = new List<SO_questStep>();

        [SerializeField]
        private SO_questStep finishStep;

        private SO_spots currentSpot;
        private SO_npc currentNPC;

        public static SO_questStartTag questStartTag;
        public static SO_questSolveValidation questSolveValidation;

        public static event Action<string> AlertQuestTracker = delegate { };

        public static int questToDelete = 0;
        public static int questToSolve = 0;

        public const string delteRecieverID = "deleteQuest";
        public const string solveRecieverID = "solveQuest";




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
            UI_QuestInfo.DecisionQuestAccept += AcceptQuestCheck;
            YesNoPopUP.YES += AbortAccepted;
            YesNoPopUP.YES += QuestSolveAccepted;

        }


        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            UI_QuestInfo.DecisionQuestAccept -= AcceptQuestCheck;
            YesNoPopUP.YES -= AbortAccepted;
            YesNoPopUP.YES -= QuestSolveAccepted;
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

            StartCoroutine(WaitForUI_QuestTrackerToBeLoaded());
        }

        private IEnumerator WaitForUI_QuestTrackerToBeLoaded()
        {
            while (UI_QuestTracker.Instance == null)
            {
                yield return new WaitForEndOfFrame();
            }

            UI_QuestTracker.Instance.CreateQuestStartPopUp(questStartTag.firstQuestStep.questText + (GetSolveText(questStartTag.firstQuestStep)), questStartTag.firstQuestStep.stepName);

            yield return null;
        }

        public void AcceptQuestCheck(bool state)
        {
            if (state)
            {
                StartQuest(questStartTag.firstQuestStep);
                questStartTag = null;
            }
            else if (state)
            {
                questStartTag = null;
            }

        }

        /// <summary>
        /// startet wirklich die quest
        /// </summary>
        /// <param name="newQuest">queststeop zum starten</param>
        public void StartQuest(SO_questStep newQuest)
        {
            if (CheckIfQuestIsAllreadyUsed(newQuest))
                return;

            
            if (questSteps[0] == null)
            {
                questSteps[0] = newQuest;
                nextQuestStep[0] = null;
                stepsDoneForQuestlineIndex0.Add(newQuest);

            }
            else if (questSteps[1] == null)
            {
                questSteps[1] = newQuest;
                nextQuestStep[1] = null;
                stepsDoneForQuestlineIndex1.Add(newQuest);
            }
            else if (questSteps[2] == null)
            {
                questSteps[2] = newQuest;
                nextQuestStep[2] = null;
                stepsDoneForQuestlineIndex2.Add(newQuest);
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



            
          //  AlertQuestTracker("Du hast " + newQuest.stepName + " angenommen!");
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
        /// gets you the text for solving
        /// </summary>
        /// <returns></returns>
        public string GetSolveTextById(int index)
        {
            if (questSteps[index] == null)
            {
                Debug.LogError("no quest to text");
                return "no quest to text";
            }

            string dialogText = GetSolveText(questSteps[index]);

            return dialogText;
        }

        private static string GetSolveText(SO_questStep questStep)
        {
            string dialogText = "";
            switch (questStep.QuestStepTarget)
            {
                case QuestTarget.collectMisc:
                    dialogText = string.Format("Gib {0} Kräuter, {1} Fleisch und {2} magische Essenzen ab.", ((SO_step_collectMisc)questStep).miscItmesNeeded.herbs, ((SO_step_collectMisc)questStep).miscItmesNeeded.meat, ((SO_step_collectMisc)questStep).miscItmesNeeded.magicEssence);
                    break;
                case QuestTarget.bringQuestItem:
                    dialogText = "Gib " + ((SO_step_bringQuestItem)questStep).questItemNeeded.itemName + " ab.";
                    break;
                case QuestTarget.goToPlace:
                    dialogText = "Gehe zum Ort: " + ((SO_step_goToPlace)questStep).spotToGO.nfcTagInfos.name;
                    break;
                case QuestTarget.goToNPC:
                    dialogText = "Gehe zum Ort: " + ((SO_step_goToNpc)questStep).npcToInteract.npcInformation.name;
                    break;
                case QuestTarget.fightAgainst:

                    break;
                case QuestTarget.nfcTag:
                    break;
                case QuestTarget.multipleChoiceAttribute:
                    break;
                case QuestTarget.multipleChoiceQuiz:
                    break;
                case QuestTarget.freeEntry:
                    break;
                case QuestTarget.baseStep:
                    break;
            }

            return dialogText;
        }

        public string GetFullInfoText(int index)
        {
            if (questSteps[index] == null)
            {
                Debug.LogError("no quest to text");
                return ("Keine Quest verfügbar für slot " + index.ToString());
            }

            string questText = "";

            questText = questSteps[0].questText + "\n\n" + GetSolveTextById(0);

            return questText;
        }

        /// <summary>
        /// Returns the dialog to solve the quest
        /// </summary>
        /// <param name="index"></param>
        public string SolveQuestText(int index)
        {

            if (nextQuestStep[index] == null)
            {
                return ("Du hast noch nicht alles um " + questSteps[index].stepName + "zu lösen");
            }

            string dialogText = GetSolveText(questSteps[index]) + "\n... hast du erfüllt!\n\nMöchtest du den Schritt abschließen?" ;
            return dialogText;

        }


        public void QuestSolveAccepted(string recieverID)
        {
            if (recieverID != solveRecieverID)
                return;

            QuestIsSolved(questToSolve);
        }

        /// <summary>
        /// things to do when quest is solved
        /// </summary>
        /// <param name="index">which quest is solved</param>
        public void QuestIsSolved(int index)
        {
            Debug.Log("solve index " + index);
            //check if quest is solved
            if (nextQuestStep[index] == null)
            {
                AlertQuestTracker("Etwas ist schiefgegangen beim lösen. Melde dich bei der Spielleitung");
                return;
            }
            //check if can be payed solved
            if (!questSteps[index].PayQuestPriceAndEndStep())
            {
                switch (questSteps[index].QuestStepTarget)
                {
                    case QuestTarget.collectMisc:
                        AlertQuestTracker("Dir fehlen Resourcen.");
                        break;
                    case QuestTarget.bringQuestItem:
                        AlertQuestTracker("Dir fehlt " + ((SO_step_bringQuestItem)questSteps[index]).questItemNeeded.itemName + ".");
                        break;
                    case QuestTarget.goToPlace:
                        AlertQuestTracker("Dir bist nicht am Ort: " + ((SO_step_goToPlace)questSteps[index]).spotToGO.nfcTagInfos.name);
                        break;
                    case QuestTarget.goToNPC:
                        AlertQuestTracker("Dir bist nicht am Ort: " + ((SO_step_goToNpc)questSteps[index]).npcToInteract.npcInformation.name);
                        break;
                    case QuestTarget.fightAgainst:
                        break;
                    case QuestTarget.nfcTag:
                        break;
                    case QuestTarget.multipleChoiceAttribute:
                        break;
                    case QuestTarget.multipleChoiceQuiz:
                        break;
                    case QuestTarget.freeEntry:
                        break;
                    case QuestTarget.baseStep:
                        AlertQuestTracker("Das ist ein Base step. Das sollte nicht sein! Informiere die Spieleitung!");
                        break;
                }
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
            if (nextQuestStep[index] == finishStep)
            {
                //questSteps[index] = null;
                //nextQuestStep[index] = null;


                ////to do: sort up
                ////if(index == 0)
                ////{
                ////    if(questSteps[1] != null)

                ////}

                //UI_QuestTracker.Instance.QuestItems[index].SetToDisabled();

                DeleteStep(index);
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
        /// <summary>
        /// quest is aborted 
        /// </summary>
        /// <param name="id"></param>
        private void AbortAccepted(string id)
        {
            if (id == delteRecieverID)
            {
                Debug.Log("delete index " + questToDelete);
                DeleteForAbort(questToDelete);
            }
        }


        /// <summary>
        /// removes the queststeps from history
        /// </summary>
        /// <param name="index"></param>
        public void DeleteForAbort(int index)
        {

            if (index == 0)
                IterateCurrentUsedStepFromListToDeleteFromUsedList(stepsDoneForQuestlineIndex0);
            if (index == 1)
                IterateCurrentUsedStepFromListToDeleteFromUsedList(stepsDoneForQuestlineIndex1);
            if (index == 2)
                IterateCurrentUsedStepFromListToDeleteFromUsedList(stepsDoneForQuestlineIndex2);

            DeleteStep(index);
        }

        private void IterateCurrentUsedStepFromListToDeleteFromUsedList(List<SO_questStep> listToCheck)
        {
            foreach (SO_questStep qs in listToCheck)
            {

                if (allreadyUsedSteps.Contains(qs))
                {
                    allreadyUsedSteps.Remove(qs);
                }
            }

            listToCheck = null;
            listToCheck = new List<SO_questStep>();

        }

        public void DeleteStep(int index)
        {
            questSteps[index] = null;
            nextQuestStep[index] = null;

            //clean history
            if (index == 0)
            {
                stepsDoneForQuestlineIndex0 = null;
                stepsDoneForQuestlineIndex0 = new List<SO_questStep>();
            }

            if (index == 1)
            {
                stepsDoneForQuestlineIndex1 = null;
                stepsDoneForQuestlineIndex1 = new List<SO_questStep>();
            }
            if (index == 2)
            {
                stepsDoneForQuestlineIndex2 = null;
                stepsDoneForQuestlineIndex2 = new List<SO_questStep>();
            }

            UI_QuestTracker.Instance.QuestItems[index].SetToDisabled();
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