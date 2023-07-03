using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    public abstract class BasicCharacterValues : MonoBehaviour
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

        [SerializeField]
        private BasicStats characterStats;

        [SerializeField]
        private OffensivStats attackStats;

        [SerializeField]
        private DefensiveStats deffensiveStats;



        
    }
}
