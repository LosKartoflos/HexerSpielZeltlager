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
    public enum ItemType { potion =2, gear = 0, quest =1, misc =3, none=4 }
    public enum GearType { armor, weapon, amulet, none }
    public enum WeaponRange { Nahkampf, Fernkampf }
    public enum DamageType { Normal, Magisch }

    //Character
    public enum CharacterType { Normal, Dickhäutig, Magisch }
    public enum CharacterMovement { Boden, Fliegend }


    //Quest
    public enum QuestTarget { collectMisc, bringQuestItem, goToPlace, goToNPC,fightAgainst, nfcTag, multipleChoiceAttribute, multipleChoiceQuiz, freeEntry, baseStep, payGold }
    public enum SocialInteraktionType { threaten, charm, debate }

    public enum RighAnswer { a,b,c,d, none}

    public enum AttributeTypes { Nichts, Körper, Geist, Charisma}

    //npc
    //public enum NPCTrade { yes}

    // Structs
   

   

}