 using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.nfcTags
{
    //um spot als Person zu nutzen muss man einfach nur einen npc  hinzufügen . Die monster sollten dann leer sein. Der kampf gegen einen NPC wird in der app getriggerd
    [CreateAssetMenu(fileName = "sp_", menuName = "Hexer_ScriptableObjects/Spots and NPCs/spot", order = 1)]
    public class SO_spots : SO_nfcTag
    {
        public const NFCType nfcType = NFCType.questStart;

        [Serializable]
        public struct Reward
        {
            public int xp;
            public int gold;
            public int herbs, meat, magicEssence;
            public List<SO_item> items;
        }

        [Serializable]
        public struct OfferType
        {
            public bool cheaper, expensive;
        }

        [Serializable]
        public struct FightingGroundParameters
        {
            public bool fightingGround;
            //just a recommendation
            public int minLevel, maxLevel;
            //one random monster is choosen --> multiple of the same kind = higher probabilty
            public List<SO_Monster> monsters;
            //if a boss is available the boss is an extra point to fight. He is only beatable one time.
            public SO_Monster boss;

        }

        [Serializable]
        public struct ShopParameters
        {
            //public bool shop;
            //public bool basicItems;
            //the tier is assigend globally or by time
            public List<SO_item> tier1Gear;
            //public List<SO_gear> tier2Gear;
            //public List<SO_gear> tier3Gear;
        
            //public OfferType amuletOffer;
            //public OfferType armorOffer;
            //public OfferType potionOffer;
            //public OfferType weaponOffer;
            //public OfferType magicalOffer;
        }

        public Reward reward;

        public FightingGroundParameters fightingGround;

        public ShopParameters shop;

        //when only one Person social interaction directly
        public List<SO_npc> peopleAtPlace;
    }

}
