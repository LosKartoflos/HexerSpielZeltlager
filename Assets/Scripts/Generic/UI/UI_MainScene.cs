using Hexerspiel.Character.monster;
using Hexerspiel.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainScene : MonoBehaviour
{
    [SerializeField]
    Button bt_fight, bt_scan;

    [SerializeField]
    SO_Monster monsterTest;
    // Start is called before the first frame update
    private void Awake()
    {
        bt_fight.onClick.AddListener(delegate { Fight.StartFightingScene(monsterTest); });
        bt_scan.onClick.AddListener(delegate { NFCManager.StartScanning(); });
    }
}
