using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Hexerspiel.Dice;

namespace Hexerspiel.Character.monster
{
    [Serializable]
    public class MonsterCharacter : BasicCharacter
    {


        [Serializable]
        public struct MonsterStats
        {
            //Basic
            [Range(1, 10)]
            public int level;
            public int gold;
            public int xp;
            //Drops
            public int herbs, meat, magicEssence;
            //these items are only droped the first time they beat the monster
            public List<SO_gear> dropedGear;
            public List<SO_potion> droppedPotion;
            public List<SO_questItem> dropedQuestItems;

        }


        #region Variables

        public static event Action<string, DateTime> MonsterKilled = delegate { };

        private MonsterStats monsterStat;

        private string monsterName;

        private Image monsterImage;

        public MonsterStats MonsterStat { get => monsterStat; }
        public string MonsterName { get => monsterName; }
        public Image ImagePath { get => monsterImage; }


        #endregion

        #region Accessors
        public MonsterCharacter(SO_Monster newMonster)
        {

            monsterStat = newMonster.monsterStats;
            basicStatsValue = newMonster.basicStats;
            defensiveStatsValue = newMonster.defensiveStats;
            offensivStatsValue = newMonster.offensivStats;

            monsterName = newMonster.monsterName;

            monsterImage = newMonster.monsterImage;
        }

        public MonsterCharacter()
        {

        }




        #endregion

        #region LifeCycle
        #endregion

        #region Functions
        /// <summary>
        /// calculates the damage
        /// </summary>
        /// <param name="extraThreshhold">permanent modifier for the succestrehshold on the attackthrow</param>
        /// <param name="manipulationPoints">extra for player, malus for monster (negativ)</param>
        /// <param name="enemyType">type of enemy</param>
        /// <param name="enemyMovement">movement of enemy</param>
        /// <returns>first value is the combined applicable damage. The Second value is the modifier</returns>
        public override int[] Attack(int extraThreshhold, int manipulationPoints, int extraDice, CharacterType enemyType, CharacterMovement enemyMovement, out RollInfos rollInfos)
        {
            int damage = 0;
            int bonusDamage = 0;

            //normal damage

            damage = Dice.Instance.RollForSuccess((offensivStatsValue.attackDice + extraDice) < 1 ? 1 : (offensivStatsValue.attackDice + extraDice), (offensivStatsValue.succesThreshold < 1 ? 1 : offensivStatsValue.succesThreshold), extraThreshhold, manipulationPoints, out rollInfos);

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


            int[] finalDamage = { damage, bonusDamage };
            return finalDamage;
        }

        public override void Died()
        {
            //TO DO: apply xp give loot

            MonsterKilled(monsterName, DateTime.Now);
            
        }



        #endregion
    }
}