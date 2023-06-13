using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Socialinteraction", menuName = "Hexer_ScriptableObjects/Spots and NPCs/Socialinteraction", order = 3)]
public class SO_socialInteractionOutcome : ScriptableObject
{
   

    [Serializable]
    public struct PositivOutcome
    {
        public string description;
        public SO_spots.Reward reward;
        public int friendshipLevel;
    }

    [Serializable]
    public struct NegativeOutcome
    {
        public string description;
        public SO_spots.Reward loss;
        //maybe item for the future
        public int friendshipLevel;
    }

    public string outComeName;
    public PositivOutcome positivOutcome;
    public NegativeOutcome negativeOutcome;
}
