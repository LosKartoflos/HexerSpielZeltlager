using Hexerspiel.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Items
{
    public class SO_gear : SO_item
    {
        public struct RegenBuffs
        {           
            public float manaRegen;
            public float healthRegen;
        }

        public const ItemType itemType = ItemType.gear;
        public PlayerCharacter.PlayerAttributes attributeBuffs;
        public RegenBuffs regenBuffs;
        public PlayerCharacter.SpellLevel spellLevelBuff;

    }
}