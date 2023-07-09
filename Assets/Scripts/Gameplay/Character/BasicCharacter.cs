using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    [Serializable]
    public abstract class BasicCharacter
    {
        [Serializable]
        public struct BasicStats
        {
            public float health;
            public CharacterMovement characterMovement;
            public CharacterType characterType;
            
        }

        [Serializable]
        public struct OffensivStats
        {
            [Range(1, 6)]
            public int succesThreshold;
            [Range(1, 10)]
            public int attackDice;
            public DamageType damageType;
            public WeaponRange weaponRange;

        }

        [Serializable]
        public struct DefensiveStats
        {
            [Range(1, 10)]
            public int armor;
        }

        [SerializeField]
        protected BasicStats basicStatsValue;

        [SerializeField]
        protected OffensivStats offensivStatsValue;

        [SerializeField]
        protected DefensiveStats deffensiveStatsValue;


        public BasicStats BasicStatsValue { get => basicStatsValue; set => basicStatsValue = value; }
        public OffensivStats OffensivStatsValue { get => offensivStatsValue; set => offensivStatsValue = value; }
        public DefensiveStats DeffensiveStatsValue { get => deffensiveStatsValue; set => deffensiveStatsValue = value; }

        public abstract int[] Attack(int extraDice, int manipulationPoints, CharacterType enemyType, CharacterMovement enemyMovement);

        protected virtual int CalculateBonusDamage(CharacterType enemyType, CharacterMovement enemyMovement, WeaponRange ownRange, DamageType ownDamageType)
        {
            int extraDamage = 0;

            switch (enemyType)
            {
                case CharacterType.normal:
                    if (ownDamageType == DamageType.magical)
                        extraDamage += 1;
                    break;
                case CharacterType.thickend:
                    if (ownRange == WeaponRange.distant)
                        extraDamage -= 1;
                    break;
                case CharacterType.magical:
                    if (ownDamageType == DamageType.normal)
                        extraDamage -= 1;
                    break;
            }

            switch (enemyMovement)
            {
                case CharacterMovement.ground:
                    extraDamage += 0;
                    break;
                case CharacterMovement.air:
                    if (ownRange == WeaponRange.close)
                        extraDamage -= 1;
                    break;
            }

            return extraDamage;
        }
    }
}
