using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel
{
    //Nfc Type
    public enum NFCType { spot, shopItem, npc, questStart}

    //Spells


    //Objects
    public enum ItemType { utility, gear, quest, misc }
    public enum GearType { armor, weapon, amulet }
    public enum WeaponRange { close, distant }
    public enum DamageType { normal, magical }

    //Character
    public enum CharacterType { normal, thickend, magical }
    public enum CharacterMovement { ground, air }


    //Quest
    public enum QuestTarget { collectMisc, bringQuestItem, goToPlace, fightAgainst, questItem, social, multipleChoiceAttribute, multipleChoiceQuiz, freeEntry }
    public enum SocialInteraktionType { threaten, charm, debate }

    //npc
    //public enum NPCTrade { yes}

}