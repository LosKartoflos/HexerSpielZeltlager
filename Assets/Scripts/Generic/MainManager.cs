using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region Functions
    #endregion

}
