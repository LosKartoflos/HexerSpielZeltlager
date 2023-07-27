using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.nfcTags
{
    //Starts a qeust with an NFC Tag
    [CreateAssetMenu(fileName = "qst_", menuName = "Hexer_ScriptableObjects/NFCTags/Quest Start")]
    public class SO_questStartTag : SO_nfcTag
    {
        public const NFCType nfcType = NFCType.questStart;
        public SO_questStep firstQuestStep;

        public SO_questStartTag(SO_questStep newStep)
        {
            firstQuestStep = newStep;
        }
    }

}
