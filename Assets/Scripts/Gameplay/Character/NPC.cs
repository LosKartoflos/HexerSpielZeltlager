using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SO_nfcTag;

namespace Hexerspiel.spots
{
    public class NPC : MonoBehaviour
    {
        //questTracker needs to keep track which quests are done
        public TagInfos npcInformation;

        public PlayerCharacterValues.PlayerAttributes attribute;

        public List<SO_questStartTag> questToStartList = new List<SO_questStartTag>();

        [Range(1, 100)]
        public int friendshipLevel;

        public SO_Monster npcInFight;

        public SO_socialInteractionOutcome socialThreaten;
        public SO_socialInteractionOutcome socialBeNice;
        public SO_socialInteractionOutcome socialVerzaubern;


    }

}
