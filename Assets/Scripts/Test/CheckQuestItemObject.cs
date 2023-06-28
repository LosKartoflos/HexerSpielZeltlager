using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckQuestItemObject : MonoBehaviour
{
    [SerializeField]
    SO_questItem questItem;

    [SerializeField]
    SO_spots spotToGo;

    [SerializeField]
    SO_npc npc;

    [SerializeField]
    SO_questStep questStep;

    private void Start()
    {

        SO_questStep nextStep;

        

        nextStep = questStep.GetNextStepIfSolved(spotToGo, npc, null);

        Debug.Log("next step:"+ nextStep);
    }
}
