using Hexerspiel.nfcTags;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Hexerspiel;
using Hexerspiel.spots;
using Hexerspiel.Items;
using Hexerspiel.Character;
using Hexerspiel.Quests;

public class NFCManager : MonoBehaviour
{
    public enum TagTask { spot, npc, questStart, questSolve, getGear, getQuestItem, getPotion, none }

    #region Variables
    private static NFCManager instance;


    public static string nfcTagMessage;



    [Serializable]
    public struct PrefixQuest
    {
        public const string spot = "sp_"; //spot is an object to be searched
        public const string npc = "npc_";//npc is an object to be searched
        public const string questStart = "qst_"; // object that has the quest linked
        public const string questSolve = "qso_"; //the questtracker should be informend about the tag.
        public const string getGear = "gg_"; //resolve by name (get gear)
        public const string getQuestItem = "gqi_"; //resolve by name (get quest item)
    }



    // public static event Action<SO_spots> ScannedSpot = delegate { };
    public static event Action<String> TagParsed = delegate { };
    private bool parsingSuccesfull = false;
    private TagTask parsedTagTask = TagTask.none;

    private SO_spots spotToVisit;
    private SO_gear gearToCollect;
    private SO_potion potionToCollect;
    private SO_questItem questItemToCollect;
    private SO_npc npcToVisit;
    private SO_questStartTag questStartTagToBegin;
    private SO_questSolveValidation questSolveValidation;
    #endregion

    #region Accessors
    public static NFCManager Instance { get => instance; }
    public bool ParsingSuccesfull { get => parsingSuccesfull; }
    public TagTask ParsedTagTask { get => parsedTagTask; }
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
        NFCMessenger.tagInfoEvent += RecieveNFCTag;
    }

    private void OnDisable()
    {
        NFCMessenger.tagInfoEvent -= RecieveNFCTag;
    }
    #endregion

    #region Functions

    public void RecieveNFCTag(string nfcTag)
    {
        nfcTagMessage = ParseNFC(nfcTag);
        TagParsed(nfcTagMessage);
        SceneManager.LoadScene("NFCEvaluationScene");
    }


    /// <summary>
    /// Parses the nfc tag and initiate next steps depending on the tag
    /// </summary>
    /// <param name="nfcTag">nfc tag</param>
    public string ParseNFC(string nfcTag)
    {
        parsedTagTask = TagTask.none;
        nfcTagMessage = nfcTag;


        //reset
        gearToCollect = null;
        questItemToCollect = null;
        npcToVisit = null;
        questStartTagToBegin = null;
        questSolveValidation = null;
        potionToCollect = null;
        spotToVisit = null;
        SpotManager.currentStpot = null;
        NPCManager.currentNpc = null;
        QuestTracker.questStartTag = null;
        QuestTracker.questSolveValidation = null;

        //Debug.Log("Recieved tag " + nfcTag + "to parse;");

        //spot is an object to be searched
        if (nfcTag.StartsWith(PrefixQuest.spot))
        {
            // ScannedSpot(FindSpotByTag(nfcTag));
            SO_spots newSpot = FindSpotByTag(nfcTag);
            if (newSpot != null)
            {
                spotToVisit = newSpot;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.spot;
                MainManager.Instance.ApplyCurrentSpot(newSpot);
                return "Du betrittst " + newSpot.nfcTagInfos.name;
            }
        }
        // npc is an object to be searched
        else if (nfcTag.StartsWith(PrefixQuest.npc))
        {
            SO_npc newNPC = FindNpcByTag(nfcTag);
            if (newNPC != null)
            {
                npcToVisit = newNPC;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.npc;
                MainManager.Instance.ApplyCurrentNPC(newNPC);
                return "Du begegnest " + newNPC.npcInformation.name;
            }
        }
        // object that has the quest linked
        else if (nfcTag.StartsWith(PrefixQuest.questStart))
        {
            SO_questStartTag newQuestStart = FindQuestStartByTag(nfcTag);
            if (newQuestStart != null)
            {
                questStartTagToBegin = newQuestStart;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.questStart;
                return "Du hast Queststart: " + newQuestStart.nfcTagInfos.name + " gefunden.";

            }
        }
        //the questtracker should be informend about the tag.
        else if (nfcTag.StartsWith(PrefixQuest.questSolve))
        {
            SO_questSolveValidation newSolver = FindQuestSolveByTag(nfcTag);
            if (newSolver != null)
            {
                questSolveValidation = newSolver;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.questSolve;
                return "Du kannst Questschritt " + newSolver.nfcTagInfos.name + " lösen.";
            }
        }
        //resolve by name (get gear)
        else if (nfcTag.StartsWith(PrefixQuest.getGear))
        {
            SO_gear newGear = FindGearByTag(nfcTag);
            if (newGear != null)
            {
                gearToCollect = newGear;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.getGear;
                return "Du kannst " + newGear.itemName + " erhalten.";
            }

            SO_potion newPotion = FindPotionByTag(nfcTag);
            if (newPotion != null)
            {
                potionToCollect = newPotion;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.getPotion;
                return "Du kannst " + newPotion.itemName + " erhalten.";
            }

        }
        //resolve by name (get quest item)
        else if (nfcTag.StartsWith(PrefixQuest.getQuestItem))
        {
            SO_questItem newQuestItem = FindQuestItemByTag(nfcTag);
            if (newQuestItem != null)
            {
                questItemToCollect = newQuestItem;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.getQuestItem;
                return "Du kannst " + newQuestItem.itemName + " erhalten.";
            }
        }

        parsingSuccesfull = false;
        return string.Format("{0} ist kein valider Tag!", nfcTag);
    }



    public void AcceptScanedTag()
    {
        switch (parsedTagTask)
        {
            case TagTask.spot:
                MainManager.Instance.ApplyCurrentSpot(spotToVisit);

                break;
            case TagTask.npc:
                NPCManager.currentNpc = npcToVisit;
                NPCManager.LoadNPCScene();
                break;
            case TagTask.questStart:
                QuestTracker.questStartTag = questStartTagToBegin;
                QuestTracker.Instance.StartQuestFromOutside();
                break;
            case TagTask.questSolve:
                QuestTracker.questSolveValidation = questSolveValidation;
                QuestTracker.Instance.CheckQuestSolverTag();
                break;
            case TagTask.getGear:
                SceneManager.LoadScene("MainScene");
                Player.Instance.GetGear(gearToCollect);
                break;
            case TagTask.getPotion:
                SceneManager.LoadScene("MainScene");
                Player.Instance.GetPotion(potionToCollect);
                break;
            case TagTask.getQuestItem:
                SceneManager.LoadScene("MainScene");
                Player.Instance.GetQuestItem(questItemToCollect);
                break;
            case TagTask.none:
                break;
        }
    }

    /// <summary>
    /// removes the prefix of the given prefixes for the nfc tags
    /// </summary>
    /// <param name="nfcTag"></param>
    /// <returns></returns>
    public static string RemovePrefix(string nfcTag)
    {
        string withoutPrefix = "";

        if (nfcTag.StartsWith(PrefixQuest.spot))
        {
            withoutPrefix = nfcTag.Substring(PrefixQuest.spot.Length);
        }
        // npc is an object to be searched
        else if (nfcTag.StartsWith(PrefixQuest.npc))
        {
            withoutPrefix = nfcTag.Substring(PrefixQuest.npc.Length);
        }
        // object that has the quest linked
        else if (nfcTag.StartsWith(PrefixQuest.questStart))
        {
            withoutPrefix = nfcTag.Substring(PrefixQuest.questStart.Length);
        }
        //the questtracker should be informend about the tag.
        else if (nfcTag.StartsWith(PrefixQuest.questSolve))
        {
            withoutPrefix = nfcTag.Substring(PrefixQuest.questSolve.Length);
        }
        //resolve by name (get gear)
        else if (nfcTag.StartsWith(PrefixQuest.getGear))
        {
            withoutPrefix = nfcTag.Substring(PrefixQuest.getGear.Length);

        }
        //resolve by name (get quest item)
        else if (nfcTag.StartsWith(PrefixQuest.getQuestItem))
        {
            withoutPrefix = nfcTag.Substring(PrefixQuest.getQuestItem.Length);
        }
        else
        {
            Debug.LogError("non removable prefix");
        }

        return withoutPrefix.ToLower();
    }

    public SO_spots FindSpotByTag(string nfcTag)
    {
        SO_spots spot = ScriptableObjectsCollection.Instance.Spots.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();
        return spot;
    }

    public SO_gear FindGearByTag(string nfcTag)
    {
        SO_gear newGear = ScriptableObjectsCollection.Instance.Weapons.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();
        if (newGear == null)
        {
            newGear = ScriptableObjectsCollection.Instance.Amulets.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();
        }
        if (newGear == null)
        {
            newGear = ScriptableObjectsCollection.Instance.Armors.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();
        }

        return newGear;
    }

    public SO_questItem FindQuestItemByTag(string nfcTag)
    {
        SO_questItem newGear = ScriptableObjectsCollection.Instance.QuestItems.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();

        return newGear;
    }

    private SO_potion FindPotionByTag(string nfcTag)
    {
        SO_potion newPotion = ScriptableObjectsCollection.Instance.Potions.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();

        return newPotion;
    }

    private SO_npc FindNpcByTag(string nfcTag)
    {
        SO_npc newNPC = ScriptableObjectsCollection.Instance.Npcs.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();

        return newNPC;
    }

    private SO_questStartTag FindQuestStartByTag(string nfcTag)
    {
        SO_questStartTag newQuestToStart = ScriptableObjectsCollection.Instance.QuestStartTags.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();

        return newQuestToStart;
    }

    private SO_questSolveValidation FindQuestSolveByTag(string nfcTag)
    {
        SO_questSolveValidation newQuestToSolve = ScriptableObjectsCollection.Instance.QuestSolveValidations.Where(obj => obj.name.ToLower() == RemovePrefix(nfcTag)).SingleOrDefault();

        return newQuestToSolve;
    }


    public static void StartScanning()
    {
        SceneManager.LoadScene("NFCScanScene");
    }
    #endregion
}
