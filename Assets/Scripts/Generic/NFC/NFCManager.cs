using Hexerspiel.nfcTags;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Hexerspiel;
using Hexerspiel.spots;

public class NFCManager : MonoBehaviour
{
    public enum TagTask { spot, npc, questStart, questSolve, getGear, getQuestItem, none }

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
        //Debug.Log("Recieved tag " + nfcTag + "to parse;");

        //spot is an object to be searched
        if (nfcTag.StartsWith(PrefixQuest.spot))
        {
            // ScannedSpot(FindSpotByTag(nfcTag));
            SO_spots newSpot = FindSpotByTag(nfcTag.Substring(PrefixQuest.spot.Length));
            if (newSpot != null)
            {
                SpotManager.currentStpot = newSpot;
                parsingSuccesfull = true;
                parsedTagTask = TagTask.spot;
                return newSpot.nfcTagInfos.name;
            }


        }
        // npc is an object to be searched
        else if (nfcTag.StartsWith(PrefixQuest.npc))
        {

        }
        // object that has the quest linked
        else if (nfcTag.StartsWith(PrefixQuest.questStart))
        {

        }
        //the questtracker should be informend about the tag.
        else if (nfcTag.StartsWith(PrefixQuest.questSolve))
        {

        }
        //resolve by name (get gear)
        else if (nfcTag.StartsWith(PrefixQuest.getGear))
        {

        }
        //resolve by name (get quest item)
        else if (nfcTag.StartsWith(PrefixQuest.getQuestItem))
        {

        }

        parsingSuccesfull = false;
        return string.Format("{0} ist kein valider Tag!", nfcTag);
    }

    public void AcceptScanedTag()
    {
        switch (parsedTagTask)
        {
            case TagTask.spot:
                SpotManager.LoadSpotScene();
                break;
            case TagTask.npc:
                break;
            case TagTask.questStart:
                break;
            case TagTask.questSolve:
                break;
            case TagTask.getGear:
                break;
            case TagTask.getQuestItem:
                break;
            case TagTask.none:
                break;
        }
    }

    public SO_spots FindSpotByTag(string nfcTag)
    {
        SO_spots spot = ScriptableObjectsCollection.Instance.Spots.Where(obj => obj.name == nfcTag).SingleOrDefault();
        return spot;
    }

    public static void StartScanning()
    {
        SceneManager.LoadScene("NFCScanScene");
    }
    #endregion
}
