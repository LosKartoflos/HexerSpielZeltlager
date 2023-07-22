using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Hexerspiel
{
    //Nfc Type
    public enum NFCType { spot, shopItem, npc, questStart, questSolve}

    //Spells


    //Objects
    public enum ItemType { potion, gear, quest, misc, none }
    public enum GearType { armor, weapon, amulet, none }
    public enum WeaponRange { close, distant }
    public enum DamageType { normal, magical }

    //Character
    public enum CharacterType { normal, thickend, magical }
    public enum CharacterMovement { ground, air }


    //Quest
    public enum QuestTarget { collectMisc, bringQuestItem, goToPlace, goToNPC,fightAgainst, nfcTag, multipleChoiceAttribute, multipleChoiceQuiz, freeEntry, baseStep }
    public enum SocialInteraktionType { threaten, charm, debate }

    public enum RighAnswer { a,b,c,d, none}

    public enum AttributeTypes { Nichts, Körper, Geist, Charisma}

    //npc
    //public enum NPCTrade { yes}

    // Structs
   

   

}