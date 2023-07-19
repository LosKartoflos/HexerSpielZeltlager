using DigitsNFCToolkit;
using DigitsNFCToolkit.JSON;
using DigitsNFCToolkit.Samples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_NFCScanScene : MonoBehaviour
{
    #region Variables
    private static UI_NFCScanScene instance;

    [SerializeField]
    TextMeshProUGUI label_scanning;

    [SerializeField]
    TMP_InputField inputFieldDebug;
    #endregion

    #region Accessors
    public static UI_NFCScanScene Instance { get => instance; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
        inputFieldDebug.gameObject.SetActive(false);
#endif
    }

    private void Start()
    {
#if UNITY_EDITOR
        inputFieldDebug.onSubmit.AddListener(FakeNFCTAG);
#endif
    }

    private void OnEnable()
    {
        NFCReadControl.tagReadEvent += SetLabelToLoading;
    }
    #endregion

    #region Functions
    public void SetLabelToLoading()
    {
        label_scanning.text = "Verarbeitet Tag...";
    }

    public void FakeNFCTAG(string tag)
    {
        Debug.Log("Fake NFC tag: " + tag);
        NFCMessenger.Instance.FakeNFCTag(tag);
    }
    #endregion
}
