using Hexerspiel.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Items
{
    public class SO_gear : SO_item
    {
        [Serializable]
        public struct RegenBuffs
        {
            public float manaRegen;
            public float healthRegen;
        }

        protected override ItemType itemType => ItemType.gear;

        public PlayerCharacterValues.PlayerAttributes attributeBuffs;
        public RegenBuffs regenBuffs;
        public PlayerCharacterValues.SpellLevel spellLevelBuff;
        protected virtual GearType gearType => GearType.none;
        public GearType GearType { get => gearType; }

        public override ItemType Type { get { return ItemType.gear; } }

        //public  override string GetDescription()
        //{
        //    return "It's a gear";
        //}
        //public  override string GetDescriptionShort()
        //{
        //    return "It's a gear";
        //}
        public string GetAttributText()
        {
            return string.Format("Körper: {0} Geist: {1} Charisma {2}\nLeben/m: {3} Mana/m: {4} ", attributeBuffs.body, attributeBuffs.mind, attributeBuffs.charisma, regenBuffs.healthRegen, regenBuffs.manaRegen);
        }


    }
}