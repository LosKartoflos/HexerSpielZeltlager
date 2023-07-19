using Hexerspiel.nfcTags;
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

        private SO_questStep[] questSteps = new SO_questStep[3];

        private SO_spots currentSpot;
        private SO_npc currentNPC;

        public static SO_questStartTag questStartTag;
        public static SO_questSolveValidation questSolveValidation;
        

    #endregion

        #region Accessors
        public static QuestTracker Instance { get => instance; }
        public SO_spots CurrentSpot { get => currentSpot; }
        public SO_npc CurrentNPC { get => currentNPC;  }
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
        #endregion

        #region Functions
        public void CheckQuests()
        {
            foreach(SO_questStep questStep in questSteps)
            {
               // if(questStep.TestIfStepIsSolved)
            }
        }

        public void CheckQuestSolverTag()
        {
            Debug.Log("CheckQuestSolverTag");
            questSolveValidation = null;
        }

        public void StartQuestWihtTag()
        {
            Debug.Log("StartQuestWihtTag");
            questStartTag = null;
        }

        public void ResetCurrentParameter()
        {
            currentSpot = null;
            currentNPC = null; 
        }
        
        

        public void SetCurrentSpot(SO_spots newSpot)
        {
            currentSpot = newSpot;
        }

        public void SetCurrentNPC(SO_npc newNpc)
        {
            currentNPC = newNpc;
        }

        public static void LoadQuestScene()
        {
            SceneManager.LoadScene("QuestScene");
        }


        #endregion
    }

}