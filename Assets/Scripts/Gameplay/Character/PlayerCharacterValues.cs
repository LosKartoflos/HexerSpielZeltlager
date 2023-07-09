using Hexerspiel.Character.monster;
using Hexerspiel.spots;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Character
{
    [Serializable]
    public class PlayerCharacterValues : BasicCharacter
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


        
        [SerializeField]
        protected  PlayerStats playerStats;
        [SerializeField]
        protected  PlayerAttributes playerAttributes;
        [SerializeField]
        protected  SpellLevel spellLevel;
       



        //to check if a Monster has been beaten (for quests and drops)
        private  Dictionary<MonsterCharacter, Time> killedMonsters = new Dictionary<MonsterCharacter, Time>();
        private  Dictionary<Spot, Time> visitedSpots = new Dictionary<Spot, Time>();
        private  Dictionary<NPC, Time> lastSocialInteraction = new Dictionary<NPC, Time>();

        public  PlayerStats PlayerStats1 { get => playerStats; set => playerStats = value; }
        public  PlayerAttributes PlayerAttributes1 { get => playerAttributes; set => playerAttributes = value; }
        public  SpellLevel SpellLevel1 { get => spellLevel; set => spellLevel = value; }

        public override int[] Attack(int extraDice, int manipulationPoints, CharacterType enemyType, CharacterMovement enemyMovement)
        {
            int damage = 0;
            int bonusDamage = 0;

            //normal damage
            Dice dice = new Dice();
            damage = dice.RollForSuccess(offensivStatsValue.attackDice, offensivStatsValue.succesThreshold, extraDice, manipulationPoints);

            //extra or minusdamge
            bonusDamage += CalculateBonusDamage(enemyType, enemyMovement, offensivStatsValue.weaponRange, offensivStatsValue.damageType);

            //if one damage has been dealt an extra is positiv: add extra
            if (damage == 1 && bonusDamage >= 1)
            {
                damage += bonusDamage;
            }
            // if damage is bigger than one and there is applicable bonus damge (positiv or negativ)
            else if (damage > 1)
            {
                damage += bonusDamage;
                //at least one damage
                if (damage < 1)
                    damage = 1;
            }


            int[] finalDamge = { damage, bonusDamage };
            return finalDamge;
        }
    }
}