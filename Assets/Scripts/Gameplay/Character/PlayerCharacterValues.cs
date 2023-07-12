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
        protected PlayerStats playerStats;
        [SerializeField]
        protected PlayerAttributes playerAttributes;
        [SerializeField]
        protected SpellLevel spellLevel;




        //to check if a Monster has been beaten (for quests and drops)
        private Dictionary<MonsterCharacter, Time> killedMonsters = new Dictionary<MonsterCharacter, Time>();
        private Dictionary<Spot, Time> visitedSpots = new Dictionary<Spot, Time>();
        private Dictionary<NPC, Time> lastSocialInteraction = new Dictionary<NPC, Time>();

        public PlayerStats PlayerStats1 { get => playerStats; set => playerStats = value; }
        public PlayerAttributes PlayerAttributes1 { get => playerAttributes; set => playerAttributes = value; }
        public SpellLevel SpellLevel1 { get => spellLevel; set => spellLevel = value; }

        /// <summary>
        /// calculates the damage
        /// </summary>
        /// <param name="extraThreshhold">permanent modifier for the succestrehshold on the attackthrow</param>
        /// <param name="manipulationPoints">extra for player, malus for monster (negativ)</param>
        /// <param name="enemyType">type of enemy</param>
        /// <param name="enemyMovement">movement of enemy</param>
        /// <returns>first value is the combined applicable damage. The Second value is the modifier</returns>
        public override int[] Attack(int extraThreshhold, int manipulationPoints, int extraDice, CharacterType enemyType, CharacterMovement enemyMovement)
        {
            int damage = 0;
            int bonusDamage = 0;

            //To Do: attribute einbeziehen

            //normal damage
            Dice dice = new Dice();
            damage = dice.RollForSuccess((offensivStatsValue.attackDice + extraDice) < 1 ? 1 : (offensivStatsValue.attackDice + extraDice), offensivStatsValue.succesThreshold, extraThreshhold, manipulationPoints);

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

        public override void Died()
        {

            SetLife(0);

        }
    }
}