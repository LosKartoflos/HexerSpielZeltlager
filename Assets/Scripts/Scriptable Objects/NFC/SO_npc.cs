using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SO_nfcTag;

[CreateAssetMenu(fileName = "NPC", menuName = "Hexer_ScriptableObjects/Spots and NPCs/Npc", order = 1)]
public class SO_npc : ScriptableObject
{
    public TagInfos npcInformation;

    public PlayerCharacterValues.PlayerAttributes attribute;

    public List<SO_questStep> questList = new List<SO_questStep>();

    //[Range(1, 100)]
    //public int friendshipLevel;

    public SO_Monster npcInFight;

    //public SO_socialInteractionOutcome socialThreaten;
    //public SO_socialInteractionOutcome socialCharm;
    //public SO_socialInteractionOutcome socialDebate;
}
