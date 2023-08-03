using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Hexerspiel.Dice;

namespace Hexerspiel.Character
{
    [Serializable]
    public abstract class BasicCharacter
    {
        [Serializable]
        public struct BasicStats
        {
            public float health;
            public float healthMax;
            public CharacterMovement characterMovement;
            public CharacterType characterType;

        }

        [Serializable]
        public struct OffensivStats
        {
            [Range(1, 6)]
            public int succesThreshold;
            [Range(1, 100)]
            public int attackDice;
            public DamageType damageType;
            public WeaponRange weaponRange;

        }

        [Serializable]
        public struct DefensiveStats
        {
            [Range(0, 100)]
            public int armor;
        }

        [SerializeField]
        public BasicStats basicStatsValue;

        [SerializeField]
        public OffensivStats offensivStatsValue;

        [SerializeField]
        public DefensiveStats defensiveStatsValue;


        public BasicStats BasicStatsValue { get => basicStatsValue; set => basicStatsValue = value; }
        public OffensivStats OffensivStatsValue { get => offensivStatsValue; set => offensivStatsValue = value; }
        public DefensiveStats DeffensiveStatsValue { get => defensiveStatsValue; set => defensiveStatsValue = value; }

        public abstract int[] Attack(int extraThreshhold, int manipulationPoints, int extraDice, CharacterType enemyType, CharacterMovement enemyMovement, out RollInfos rollInfos);

        public virtual int Defend(int damageDealt)
        {
            int damageFinal = 0;
            // wenn schaden negativ soll man kein leben dazubekommen, daher das ?:
            if (damageDealt - defensiveStatsValue.armor > 0)
                damageFinal = defensiveStatsValue.armor;
            Debug.Log("Damage Final " + damageFinal.ToString());
            AddLife(-damageFinal);
            return damageFinal;
        }

        public virtual void SetLife(float healthAmount)
        {
            basicStatsValue.health = healthAmount;
        }

        public virtual void AddLife(float healthAmount)
        {
            basicStatsValue.health += healthAmount;

            if (basicStatsValue.health > basicStatsValue.healthMax)
                basicStatsValue.health = basicStatsValue.healthMax;
        }

        public virtual float GetLife()
        {
            return basicStatsValue.health;


        }

        public abstract void Died();

        protected virtual int CalculateBonusDamage(CharacterType enemyType, CharacterMovement enemyMovement, WeaponRange ownRange, DamageType ownDamageType)
        {
            int extraDamage = 0;

            switch (enemyType)
            {
                case CharacterType.Normal:
                    if (ownDamageType == DamageType.Magisch)
                        extraDamage += 1;
                    break;
                case CharacterType.Dickhäutig:
                    if (ownRange == WeaponRange.Fernkampf)
                        extraDamage -= 1;
                    break;
                case CharacterType.Magisch:
                    if (ownDamageType == DamageType.Normal)
                        extraDamage -= 1;
                    break;
            }

            switch (enemyMovement)
            {
                case CharacterMovement.Boden:
                    extraDamage += 0;
                    break;
                case CharacterMovement.Fliegend:
                    if (ownRange == WeaponRange.Nahkampf)
                        extraDamage -= 1;
                    break;
            }


            return extraDamage;
        }
    }
}
