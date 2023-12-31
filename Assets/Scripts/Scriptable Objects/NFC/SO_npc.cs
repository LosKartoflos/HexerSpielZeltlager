using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using Hexerspiel.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SO_nfcTag;

[CreateAssetMenu(fileName = "npc_", menuName = "Hexer_ScriptableObjects/Spots and NPCs/Npc", order = 1)]
public class SO_npc : ScriptableObject
{
    public TagInfos npcInformation;

    public PlayerCharacterValues.PlayerAttributes attribute;

    public List<SO_questStep> questList = new List<SO_questStep>();

    //[Range(1, 100)]
    //public int friendshipLevel;

    public SO_Monster npcInFight;


    public List<SO_gear> gearToSell = new List<SO_gear>();


    public List<SO_potion> potionToSell = new List<SO_potion>();


    public List<SO_questItem> questItemToSell = new List<SO_questItem>();

    //public SO_socialInteractionOutcome socialThreaten;
    //public SO_socialInteractionOutcome socialCharm;
    //public SO_socialInteractionOutcome socialDebate;
}
