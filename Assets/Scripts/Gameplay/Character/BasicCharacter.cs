using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    public abstract class BasicCharacter : MonoBehaviour
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

        }

        [Serializable]
        public struct DefensiveStats
        {
            [Range(1, 10)]
            public int armor;
        }


        protected BasicStats basicStatsValue;


        protected OffensivStats offensivStatsValue;


        protected DefensiveStats deffensiveStatsValue;

        public BasicStats BasicStatsValue { get => basicStatsValue; set => basicStatsValue = value; }
        public OffensivStats OffensivStatsValue { get => offensivStatsValue; set => offensivStatsValue = value; }
        public DefensiveStats DeffensiveStatsValue { get => deffensiveStatsValue; set => deffensiveStatsValue = value; }
    }
}
