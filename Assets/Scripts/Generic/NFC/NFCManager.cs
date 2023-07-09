using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NFCManager : MonoBehaviour
{
    #region Variables
    private static NFCManager instance;

    public static string nfcTagMessage;




    #endregion

    #region Accessors
    public static NFCManager Instance { get => instance; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region Functions
    public void EvaluateNfc()
    {

    }

    public static void StartScanning()
    {
        SceneManager.LoadScene("NFCScanScene");
    }
    #endregion
}
