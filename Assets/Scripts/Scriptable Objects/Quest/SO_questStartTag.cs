using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.nfcTags
{
    //Starts a qeust with an NFC Tag
    public class SO_questStartTag : SO_nfcTag
    {
        public const NFCType nfcType = NFCType.questStart;
        public SO_questStep firstQuestStep;
    }

}
