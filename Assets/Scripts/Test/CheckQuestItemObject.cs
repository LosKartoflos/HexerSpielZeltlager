using Hexerspiel.Items;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckQuestItemObject : MonoBehaviour
{
    [SerializeField]
    SO_questItem questItem;

    [SerializeField]
    SO_step_bringQuestItem bringQuestItem;

    private void Start()
    {
        bool stepIsSolved = false;

        SO_questStep nextStep;

        nextStep = bringQuestItem.GetNextStepIfSolved(null, null, questItem);

        Debug.Log("Step is solved: " + nextStep.name);
    }
}
