using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    #region Variables
    private static MainManager instance;


    #endregion

    #region Accessors
    public static MainManager Instance { get => instance;}
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
    public static void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    #endregion

}
