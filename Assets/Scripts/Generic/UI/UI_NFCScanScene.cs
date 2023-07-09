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
    #endregion

    #region Accessors
    public static UI_NFCScanScene Instance { get => instance; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        instance = this;
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
    #endregion
}
