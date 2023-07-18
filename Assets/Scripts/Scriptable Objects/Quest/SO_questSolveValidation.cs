using Hexerspiel;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.nfcTags
{
    [CreateAssetMenu(fileName = "qso_", menuName = "Hexer_ScriptableObjects/NFCTags/Quest Solve")]
    public class SO_questSolveValidation : SO_nfcTag
    {
        public const NFCType nfcType = NFCType.questSolve;
        public SO_questStep questStepToSolve;
    }
}
