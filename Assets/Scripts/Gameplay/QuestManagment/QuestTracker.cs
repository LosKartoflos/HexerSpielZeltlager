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
        public static SO_questStep nextQuestStepMultipleChoice;

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

        [SerializeField]
        SO_spots testSpot;

        [SerializeField]
        SO_npc testNpc;


        public static SO_spots currentSpot;
        public static SO_npc currentNPC;

        public static string freeEntry;
        public static SO_rightAnswer givenAnswer;
        public static SO_questStartTag questStartTag;
        public static SO_questSolveValidation questSolveValidation;

        public static event Action<string> AlertQuestTracker = delegate { };

        public static int questToDelete = 0;
        public static int questToSolveIndex = 0;


        public const string delteRecieverID = "deleteQuest";
        public const string solveRecieverID = "solveQuest";
        public const string multipleChoiceRecieverID = "solveMultipleChoice";
        public const string freeEntryRecieverID = "freeEntry";

        #endregion

        #region Accessors
        public static QuestTracker Instance { get => instance; }
        public static SO_questStep[] QuestSteps { get => questSteps; }
        public static SO_questStep[] NextQuestStep { get => nextQuestStep; }
        public SO_questStep FinishStep { get => finishStep; }

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

        private void Update()
        {
            currentSpot = testSpot;
            currentNPC = testNpc;
        }


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            UI_QuestInfo.DecisionQuestAccept += AcceptQuestCheck;
            YesNoPopUP.YES += AbortAccepted;
            YesNoPopUP.YES += QuestSolveAccepted;
            MultipleChoicePopUp.ANSWERGIVEN += QuestSolveMultipleChoice;
            FreeEntryPopUp.SubmitAnswer += QuestSolveFreeEntry;

        }



        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            UI_QuestInfo.DecisionQuestAccept -= AcceptQuestCheck;
            YesNoPopUP.YES -= AbortAccepted;
            YesNoPopUP.YES -= QuestSolveAccepted;
            MultipleChoicePopUp.ANSWERGIVEN -= QuestSolveMultipleChoice;
            FreeEntryPopUp.SubmitAnswer -= QuestSolveFreeEntry;
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

        public void StartQuestFromOutside()
        {
            if (CheckIfQuestIsAllreadyUsed(questStartTag.firstQuestStep))
            {
                questStartTag = null;
                return;
            }

            LoadQuestScene();
            Debug.Log("StartQuestWihtTag");

            if (questStartTag.firstQuestStep.QuestStepTarget == QuestTarget.fightAgainst)
            {
                ((SO_step_fight)questStartTag.firstQuestStep).timeQuestAccepted = DateTime.Now;
            }


            StartCoroutine(WaitForUI_QuestTrackerToBeLoaded());
        }

        private IEnumerator WaitForUI_QuestTrackerToBeLoaded()
        {
            while (UI_QuestTracker.Instance == null)
            {
                yield return new WaitForEndOfFrame();
            }

            UI_QuestTracker.Instance.CreateQuestStartPopUp(questStartTag.firstQuestStep.questText + "\n\n" + (GetSolveText(questStartTag.firstQuestStep)) + "\n\n" + questStartTag.firstQuestStep.GetLootText() + "\n\n======================\n\n" + questStartTag.firstQuestStep.GetLootTextWhole(), questStartTag.firstQuestStep.stepName);

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


            int indexUsed = -1;


            if (questSteps[0] == null)
            {
                questSteps[0] = newQuest;
                nextQuestStep[0] = null;
                stepsDoneForQuestlineIndex0.Add(newQuest);
                indexUsed = 0;
            }
            else if (questSteps[1] == null)
            {
                questSteps[1] = newQuest;
                nextQuestStep[1] = null;
                stepsDoneForQuestlineIndex1.Add(newQuest);
                indexUsed = 1;
            }
            else if (questSteps[2] == null)
            {
                questSteps[2] = newQuest;
                nextQuestStep[2] = null;
                stepsDoneForQuestlineIndex2.Add(newQuest);
                indexUsed = 2;
            }
            else
            {
                AlertQuestTracker("Du hast schon 3 Quests!\nDu musst eine abschließen oder abbrechen!");
                return;
            }

            if (questSteps[indexUsed].QuestStepTarget == QuestTarget.fightAgainst)
            {
                ((SO_step_fight)questSteps[indexUsed]).timeQuestAccepted = DateTime.Now;
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

        /// <summary>
        /// weißt die nächsten questeps zu wenn lösbar. Wenn ein quest step zugewiesne ist gilt die aufgabe als gelöst
        /// Ausnahme Multiplice choice. da kommt erst die wahl und free entry wird auch gerprüft
        /// </summary>
        public void CheckIfStepsAreSovleable()
        {
            for (int i = 0; i < maximalQuestsTracked; i++)
            {
                if (questSteps[i] != null)
                {
                    //für multiple choice muss der Step vorhanden sein, damit der solve button verfügbar ist, weil quasi erst im Dialog gesolved wird
                    if (questSteps[i].QuestStepTarget == QuestTarget.multipleChoiceQuiz || QuestSteps[i].QuestStepTarget == QuestTarget.freeEntry)
                    {
                        nextQuestStep[i] = questSteps[i].nextQuestStep;
                    }
                    else if (QuestSteps[i].QuestStepTarget == QuestTarget.multipleChoiceAttribute)
                    {
                        if (((SO_step_mulitpleChoiceAttribute)questSteps[i]).answers.Count > 0)
                            nextQuestStep[i] = ((SO_step_mulitpleChoiceAttribute)questSteps[i]).answers[0].questStep;
                    }
                    else
                    {
                        nextQuestStep[i] = questSteps[i].GetNextStepIfSolved();
                    }



                }

            }
        }

        /// <summary>
        /// gets you the text for solving
        /// </summary>
        /// <returns></returns>
        public string GetSolveTextById(int index, bool onlyRemaining = false)
        {
            if (questSteps[index] == null)
            {
                Debug.LogError("no quest to text");
                return "no quest to text";
            }

            string dialogText = GetSolveText(questSteps[index], onlyRemaining);

            return dialogText;
        }

        public void SolveTextByIdAlert(int index)
        {
            // AlertQuestTracker(GetSolveTextById(index, true) );

            QuestIsSolved(index);
        }

        private static string GetSolveText(SO_questStep questStep, bool onlyRemaining = false)
        {

            string npcOrPlaceTask = "";

            if ((questStep.npcToInteract != null && questStep.npcToInteract != currentNPC))//(onlyRemaining && questStep.npcToInteract != null) || 
            {
                npcOrPlaceTask += "und gehe zu Person: " + questStep.npcToInteract.npcInformation.name;
            }

            else if ((questStep.npcToInteract != null && questStep.npcToInteract == currentNPC))
            {
                npcOrPlaceTask += "Du bist bei Person " + questStep.npcToInteract.npcInformation.name;
            }

            if ((questStep.spotToGO != null && questStep.spotToGO != currentSpot))// || (onlyRemaining && questStep.spotToGO != null);
            {
                if (npcOrPlaceTask != "")
                    npcOrPlaceTask += "\n";
                npcOrPlaceTask += "und gehe zum Ort: " + questStep.spotToGO.nfcTagInfos.name;
            }
            else if (questStep.spotToGO != null && questStep.spotToGO == currentSpot)
            {
                if (npcOrPlaceTask != "")
                    npcOrPlaceTask += "\n";
                npcOrPlaceTask += "Du bist am Ort " + questStep.spotToGO.nfcTagInfos.name;
            }


            if (npcOrPlaceTask != "")
                npcOrPlaceTask += " oder ";


            string dialogText = "";
            switch (questStep.QuestStepTarget)
            {
                case QuestTarget.collectMisc:
                    dialogText = string.Format("Gib {0} Kräuter, {1} Fleisch und {2} magische Essenzen ab.", ((SO_step_collectMisc)questStep).miscItmesNeeded.herbs, ((SO_step_collectMisc)questStep).miscItmesNeeded.meat, ((SO_step_collectMisc)questStep).miscItmesNeeded.magicEssence) + "\n\n" + npcOrPlaceTask;
                    break;
                case QuestTarget.bringQuestItem:
                    dialogText = "Gib " + ((SO_step_bringQuestItem)questStep).questItemNeeded.itemName + " ab." + "\n\n" + npcOrPlaceTask;
                    break;
                case QuestTarget.goToPlace:
                    dialogText = npcOrPlaceTask;// "Gehe zum Ort: " + ((SO_step_goToPlace)questStep).spotToGO.nfcTagInfos.name;
                    break;
                case QuestTarget.goToNPC:
                    dialogText = npcOrPlaceTask;//"Gehe zu Person: " + ((SO_step_goToNpc)questStep).npcToInteract.npcInformation.name;
                    break;
                case QuestTarget.fightAgainst:
                    dialogText = ("Du musst gegen eine " + ((SO_step_fight)questStep).monsterToFight.monsterName + " nach " + ((SO_step_fight)questStep).timeQuestAccepted.ToString("HH:mm") + " Uhr gekämpft haben.");
                    break;
                case QuestTarget.nfcTag:
                    dialogText = "Lasse dir die Aufgabe von der Spielleitung bestätigen." + "\n\n" + npcOrPlaceTask;
                    break;
                case QuestTarget.multipleChoiceAttribute:
                    dialogText = "Wähle eine Antwort!" + "\n\n" + npcOrPlaceTask;
                    break;
                case QuestTarget.multipleChoiceQuiz:
                    dialogText = "Wähle die richtige Antwort!" + "\n\n" + npcOrPlaceTask;
                    break;
                case QuestTarget.freeEntry:
                    dialogText = ((SO_step_FreeEntry)questStep).shortQuestion + " Gib die richtige Anwtort ein!" + "\n\n" + npcOrPlaceTask;
                    break;
                case QuestTarget.baseStep:
                    AlertQuestTracker("Das ist ein Base step. Das sollte nicht sein! Informiere die Spieleitung!");
                    dialogText = ("Da stimmt was nicht. Hier ist ein BaseStep. Gehe zur Spielleitung!");
                    break;
                case QuestTarget.payGold:
                    dialogText = "Bezahle " + ((SO_step_payGold)questStep).goldToPay + " Gold!" + "\n\n" + npcOrPlaceTask;
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

            questText = questSteps[index].questText + "\n\n" + GetSolveTextById(index) + "\n\n" + questSteps[index].GetLootText();

            return questText;
        }

        /// <summary>
        /// Returns the dialog to solve the quest
        /// </summary>
        /// <param name="index"></param>
        public string QuestSolvedText(int index)
        {

            if (nextQuestStep[index] == null)
            {
                return ("Du hast noch nicht alles um " + questSteps[index].stepName + "zu lösen");
            }

            string dialogText = GetSolveText(questSteps[index]);

            if (QuestSteps[index].QuestStepTarget != QuestTarget.multipleChoiceQuiz && QuestSteps[index].QuestStepTarget != QuestTarget.multipleChoiceAttribute && QuestSteps[index].QuestStepTarget != QuestTarget.freeEntry)
                dialogText += "\n... hast du erfüllt!\n\nMöchtest du den Schritt abschließen?";

            return dialogText;

        }


        public void QuestSolveAccepted(string recieverID)
        {
            if (recieverID != solveRecieverID)
                return;

            QuestIsSolved(questToSolveIndex);
        }

        private void QuestSolveFreeEntry(string answerText, string recieverID)
        {
            if (recieverID != freeEntryRecieverID)
                return;

            freeEntry = answerText;

            Debug.Log(" QuestSolveFreeEntry " + answerText);
            if (QuestSteps[questToSolveIndex].QuestStepTarget == QuestTarget.freeEntry)
            {
                ((SO_step_FreeEntry)QuestSteps[questToSolveIndex]).CheckIfStepIsSolved();
            }

            QuestIsSolved(questToSolveIndex);
        }

        public void QuestSolveMultipleChoice(RighAnswer answerGiven, string recieverID)
        {
            Debug.Log("Multiple choice Answer given " + answerGiven);

            if (recieverID != multipleChoiceRecieverID)
                return;

            givenAnswer = new SO_rightAnswer(answerGiven);

            if (QuestSteps[questToSolveIndex].QuestStepTarget == QuestTarget.multipleChoiceAttribute)
            {
                ((SO_step_mulitpleChoiceAttribute)QuestSteps[questToSolveIndex]).CheckIfStepIsSolved();
                Debug.Log(" QuestSolveMultipleChoice: " + nextQuestStep[questToSolveIndex]);
            }

            QuestIsSolved(questToSolveIndex);
        }

        /// <summary>
        /// things to do when quest is solved
        /// </summary>
        /// <param name="index">which quest is solved</param>
        public void QuestIsSolved(int index)
        {
            Debug.Log("solve index " + index);
            //check if quest is solved
            //if (nextQuestStep[index] == null)
            //{
            //    AlertQuestTracker("Etwas ist schiefgegangen beim lösen. Melde dich bei der Spielleitung");
            //    return;
            //}
            //check if can be payed solved
            if (!questSteps[index].PayQuestPriceAndEndStep())
            {
                string notAtPlaceOrNPC = "";
                if (questSteps[index].npcToInteract != null)
                {
                    if (questSteps[index].npcToInteract != currentNPC)
                        notAtPlaceOrNPC += "\nund/oder du bist nicht bei Person: " + questSteps[index].npcToInteract.npcInformation.name;
                }

                if (questSteps[index].spotToGO != null)
                {
                    if (questSteps[index].spotToGO != currentSpot)
                        notAtPlaceOrNPC += "\nund/oder du bist nicht am Ort: " + questSteps[index].spotToGO.nfcTagInfos.name;
                }

                switch (questSteps[index].QuestStepTarget)
                {
                    case QuestTarget.collectMisc:

                        AlertQuestTracker("Dir fehlen Resourcen " + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.bringQuestItem:
                        AlertQuestTracker("Dir fehlt " + ((SO_step_bringQuestItem)questSteps[index]).questItemNeeded.itemName + "." + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.goToPlace:
                        AlertQuestTracker(notAtPlaceOrNPC);//"Dir bist nicht am Ort: " + ((SO_step_goToPlace)questSteps[index]).spotToGO.nfcTagInfos.name +;
                        break;
                    case QuestTarget.goToNPC:
                        AlertQuestTracker(notAtPlaceOrNPC);//"Dir bist nicht bei Person: " + ((SO_step_goToNpc)questSteps[index]).npcToInteract.npcInformation.name + 
                        break;
                    case QuestTarget.fightAgainst:
                        AlertQuestTracker("Du musst gegen eine " + ((SO_step_fight)questSteps[index]).monsterToFight.monsterName + " nach " + ((SO_step_fight)questSteps[index]).timeQuestAccepted.ToString("HH:mm") + " Uhr gekämpft haben.");
                        break;
                    case QuestTarget.nfcTag:
                        AlertQuestTracker("Du brauchst die Bestätigung für " + ((SO_step_nfcTag)questSteps[index]).stepName + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.multipleChoiceAttribute:
                        AlertQuestTracker("Für diese Antwort ist dein verbundener Attribut wert zu niedrig!" + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.multipleChoiceQuiz:
                        AlertQuestTracker("Diese Antwort ist falsch!" + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.freeEntry:
                        AlertQuestTracker("Diese Antwort ist falsch!" + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.baseStep:
                        AlertQuestTracker("Das ist ein Base step. Das sollte nicht sein! Informiere die Spieleitung!" + notAtPlaceOrNPC);
                        break;
                    case QuestTarget.payGold:
                        AlertQuestTracker("Dir fehlt " + ((SO_step_payGold)questSteps[index]).goldToPay + " Gold ." + notAtPlaceOrNPC);
                        break;
                }
                return;
            }

            ////if no resolve text, use generic
            //if (questSteps[index].resolvedText == "" || questSteps[index].resolvedText == null)
            //{
            //    AlertQuestTracker("Du hast " + questSteps[index].stepName + " gelöst!");
            //}
            ////use resolve text
            //else
            //{
            //AlertQuestTracker(questSteps[index].resolvedText);



            //}
            if (questSteps[index].QuestStepTarget == QuestTarget.multipleChoiceAttribute)
                nextQuestStep[index] = nextQuestStepMultipleChoice;

            Player.Instance.RecieveLootQuestStep(questSteps[index]);
            //last step 
            if (nextQuestStep[index] == finishStep)
            {
                UI_QuestTracker.Instance.CreateInfoPopUpAcceptOnly(questSteps[index].resolvedText + "\n\n", "Quest Abgesschlossen");

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
                UI_QuestTracker.Instance.CreateInfoPopUpAcceptOnly(questSteps[index].resolvedText + "\n\n" + nextQuestStep[index].questText + "\n\n" + GetSolveText(nextQuestStep[index]), nextQuestStep[index].stepName);

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