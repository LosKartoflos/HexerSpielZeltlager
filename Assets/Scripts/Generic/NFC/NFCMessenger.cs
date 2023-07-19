using DigitsNFCToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NFCMessenger : MonoBehaviour
{
    #region Variables
    private static NFCMessenger instance;

    public static event Action<string> tagInfoEvent = delegate { };
    

    private const string TAG_INFO_FORMAT = "ID: {0}\nTechnologies: {1}\nManufacturer: {2}\nWritable: {3}\nMaxWriteSize: {4}";
    private const string TEXT_RECORD_FORMAT = "Type: {0}\nText: {1}\nLanguage Code: {2}\nText Encoding: {3}";

    private string nfcTagID;

 
    //privateList


   
    #endregion

    #region Accessors
    public static NFCMessenger Instance { get => instance; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Functions
    /// <summary>
    /// On tag read, get the manufacture infos from tag
    /// </summary>
    /// <param name="tag"></param>
    public void UpdateTagInfo(NFCTag tag)
    {
        string technologiesString = string.Empty;
        NFCTechnology[] technologies = tag.Technologies;
        int length = technologies.Length;
        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                technologiesString += ", ";
            }

            technologiesString += technologies[i].ToString();
        }

        string maxWriteSizeString = string.Empty;
        if (tag.MaxWriteSize > 0)
        {
            maxWriteSizeString = tag.MaxWriteSize + " bytes";
        }
        else
        {
            maxWriteSizeString = "Unknown";
        }

        string tagInfo = string.Format(TAG_INFO_FORMAT, tag.ID, technologiesString, tag.Manufacturer, tag.Writable, maxWriteSizeString);
    }


    /// <summary>
    /// Recieves and parses the nfc Tag message
    /// </summary>
    /// <param name="message"></param>
    public void UpdateNDEFMessage(NDEFMessage message)
    {

        float y = 0;
        List<NDEFRecord> records = message.Records;

        int length = records.Count;
        for (int i = 0; i < length; i++)
        {
            NDEFRecord record = records[i];
            switch (record.Type)
            {
                case NDEFRecordType.TEXT:
                    TextRecord textRecord = (TextRecord)record;
                    nfcTagID = textRecord.text;//(string.Format(TEXT_RECORD_FORMAT, NDEFRecordType.TEXT, textRecord.text, textRecord.languageCode, textRecord.textEncoding));
                    tagInfoEvent(nfcTagID);
                    SceneManager.LoadScene("NFCEvaluationScene");
                    break;
                default:
                    Debug.LogError("NFC tag has wrong format");
                    break;

            }

        }
    }

    public void FakeNFCTag(string tag)
    {   
 
        tagInfoEvent(tag);
        SceneManager.LoadScene("NFCEvaluationScene");
    }
    #endregion
}
