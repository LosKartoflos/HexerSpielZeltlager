using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainScene : MonoBehaviour
{
    [SerializeField]
    Button bt_fight;
    // Start is called before the first frame update
    private void Awake()
    {
        bt_fight.onClick.AddListener(delegate { SceneManager.LoadScene("FightScene"); });
    }
}
