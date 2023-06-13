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
        public SO_spots.Reward moneyOrXp;
        public List<SO_item> itemsReward;
        public int friendshipLevel;
    }

    [Serializable]
    public struct NegativeOutcome
    {
        public SO_spots.Reward moneyOrXp;
        //maybe item for the future
        public int friendshipLevel;
    }

    public PositivOutcome positivOutcome;
    public NegativeOutcome negativeOutcome;
}
