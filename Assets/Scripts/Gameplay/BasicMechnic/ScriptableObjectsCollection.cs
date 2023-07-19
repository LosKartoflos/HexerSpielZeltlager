using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Hexerspiel.Quests;

namespace Hexerspiel
{
    public class ScriptableObjectsCollection : MonoBehaviour
    {

        #region Variables
        private static ScriptableObjectsCollection instance;

        //to visit
        [SerializeField]
        private List<SO_spots> spots = new List<SO_spots>();
        [SerializeField]
        private List<SO_npc> npcs = new List<SO_npc>();

        //gear
        [SerializeField]
        private List<SO_weapon> weapons = new List<SO_weapon>();
        [SerializeField]
        private List<SO_amulet> amulets = new List<SO_amulet>();
        [SerializeField]
        private List<SO_armor> armors = new List<SO_armor>();
        [SerializeField]
        private List<SO_questItem> questItems = new List<SO_questItem>();
        [SerializeField]
        private List<SO_potion> potions = new List<SO_potion>();

        //other
        [SerializeField]
        private List<SO_questStartTag> questStartTags = new List<SO_questStartTag>();
        [SerializeField]
        private List<SO_questSolveValidation> questSolveValidations = new List<SO_questSolveValidation>();


        //quest
        [SerializeField]
        private List<SO_questStep> questSteps = new List<SO_questStep>();

        #endregion

        #region Accessors
        public static ScriptableObjectsCollection Instance { get => instance; }
        public List<SO_spots> Spots { get => spots; }
        public List<SO_npc> Npcs { get => npcs; }
        public List<SO_weapon> Weapons { get => weapons; }
        public List<SO_amulet> Amulets { get => amulets; }
        public List<SO_armor> Armors { get => armors; }
        public List<SO_questItem> QuestItems { get => questItems; }
        public List<SO_potion> Potions { get => potions; }
        public List<SO_questStartTag> QuestStartTags { get => questStartTags; }
        public List<SO_questSolveValidation> QuestSolveValidations { get => questSolveValidations; }


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

        private void Start()
        {
            StartCoroutine(LoadAllSOs());
        }
        #endregion

        #region Functions
        private IEnumerator LoadAllSOs()
        {
            spots = Resources.LoadAll<SO_spots>("ScriptableObjects/SO_spots").ToList();

            npcs = Resources.LoadAll<SO_npc>("ScriptableObjects/SO_npc").ToList();

            weapons = Resources.LoadAll<SO_weapon>("ScriptableObjects/SO_gear/SO_weapon").ToList();

            amulets = Resources.LoadAll<SO_amulet>("ScriptableObjects/SO_gear/SO_amulette").ToList();

            armors = Resources.LoadAll<SO_armor>("ScriptableObjects/SO_gear/SO_armor").ToList();

            questItems = Resources.LoadAll<SO_questItem>("ScriptableObjects/SO_questItem").ToList();

            potions = Resources.LoadAll<SO_potion>("ScriptableObjects/SO_potion").ToList();

            questStartTags = Resources.LoadAll<SO_questStartTag>("ScriptableObjects/SO_questStartTag").ToList();

            questSolveValidations = Resources.LoadAll<SO_questSolveValidation>("ScriptableObjects/SO_questSolveValidation").ToList();

            questSteps = Resources.LoadAll<SO_questStep>("ScriptableObjects/SO_questStep").ToList();

            SceneManager.LoadScene("MainScene");

            yield return null;
        }
        #endregion
    }
}
