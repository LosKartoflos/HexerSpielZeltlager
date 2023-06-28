using Hexerspiel.spots;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Character
{
    public class PlayerCharacter : BasicCharacterValues
    {

        [Serializable]
        public struct PlayerStats
        {
            public float mana;
            public float manaRegen;
            public float healthRegen;
            public int xp;
            public int level;
        }

        [Serializable]
        public struct PlayerAttributes
        {
            public int body;
            public int mind;
            public int charisma;
        }

        [Serializable]
        public struct SpellLevel
        {
            public int lifeSpell;
            public int mindSpell;
            public int charismaSpell;
        }

        private static PlayerCharacter instance;
        
        [SerializeField]
        private PlayerStats playerStats;
        [SerializeField]
        private PlayerAttributes playerAttributes;
        [SerializeField]
        private SpellLevel spellLevel;
        [SerializeField]
        private InventoryCollection inventory;



        //to check if a Monster has been beaten (for quests and drops)
        private Dictionary<MonsterCharacter, Time> killedMonsters = new Dictionary<MonsterCharacter, Time>();
        private Dictionary<Spot, Time> visitedSpots = new Dictionary<Spot, Time>();
        private Dictionary<NPC, Time> lastSocialInteraction = new Dictionary<NPC, Time>();

        
        public static PlayerCharacter Instance { get => instance;  }
        public InventoryCollection Inventory { get => inventory; set => inventory = value; }

        private void Awake()
        {
            instance = this;
        }
    }
}