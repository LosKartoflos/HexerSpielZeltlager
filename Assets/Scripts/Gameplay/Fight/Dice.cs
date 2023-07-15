using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel
{
    /// <summary>
    /// manages all dice roll beahviour
    /// </summary>
    public class Dice : MonoBehaviour
    {
        private static Dice instance;

        private System.Random random;

        public static Dice Instance { get => instance; }

        [Serializable]
        public struct RollInfos
        {
            public int diceRolled;
            public int threshold;
            public string rolledValues;
            public int mods;
            public string modedValues;
            public int succssess;
        }


        [Serializable]
        public struct Manipulation
        {
            public int addDice;
            public int substractDiceFromEnemy;
            public int addablePoints;
            public int subtractablePointsFromEnemy;

            public Manipulation(int addDice, int substractDiceFromEnemy, int addablePoints, int subtractablePointsFromEnemy)
            {
                this.addDice = addDice;
                this.substractDiceFromEnemy = substractDiceFromEnemy;
                this.addablePoints = addablePoints;
                this.subtractablePointsFromEnemy = subtractablePointsFromEnemy;
            }
        }

        private void Awake()
        {
            instance = this;
            random = new System.Random();
        }

        /// <summary>
        /// rolls a d6
        /// </summary>
        /// <returns>returns a random number between 1 and 6</returns>
        public int Roll()
        {
            if (random == null)
                random = new System.Random();
            return random.Next(1, 7);
        }

        /// <summary>
        /// Returns how many succesfulls rolls have been done.
        /// </summary>
        /// <param name="successThreshhold">the value after which a roll is successfull</param>
        /// <param name="availableDices">the dices to be rolled</param>
        /// <param name="passiveModifierPool">Permanent modifiers for every roll. Like gear modifiers</param>
        /// <param name="activeModifierPool">Points that get distributed to modify dice rolls</param>
        /// <returns></returns>
        public int RollForSuccess(int availableDices, int successThreshhold, int passiveModifierPool, int activeModifierPool, out RollInfos rollInfos)
        {

            int[] rollResults = new int[availableDices];
            int successfullRolls = 0;

            //roll available dices;
            for (int i = 0; i < availableDices; i++)
            {
                //roll should never be below zero
                rollResults[i] = Roll() + passiveModifierPool;// (Roll() + passiveModifierPool)>=0? (Roll() + passiveModifierPool):0;
            }

            //Sort from high to low and check for successa and modify if needed
            Array.Sort(rollResults);
            Array.Reverse(rollResults);


            string rolledValues = "";
            foreach (int value in rollResults)
            {
                rolledValues += (value.ToString() + "; ");
            }



            //modifiy add
            if (activeModifierPool > 0)
                rollResults = AddModifier(availableDices, successThreshhold, activeModifierPool, rollResults);
            //modify substract
            else if (activeModifierPool < 0)
                rollResults = SubtractModifier(availableDices, successThreshhold, activeModifierPool, rollResults);



            //checkssuccesses
            for (int i = 0; i < availableDices; i++)
            {
                //roll is success
                if (rollResults[i] >= successThreshhold)
                {
                    successfullRolls++;
                }
                //no successfull left;
                else
                {
                    break;
                }
            }

            string modifiedRolledValues = "";
            foreach (int value in rollResults)
            {
                modifiedRolledValues += (value.ToString() + "; ");
            }

            rollInfos.succssess = successfullRolls;
            rollInfos.threshold = successThreshhold;
            rollInfos.rolledValues = rolledValues;
            rollInfos.modedValues = modifiedRolledValues;
            rollInfos.diceRolled = availableDices;
            rollInfos.mods = activeModifierPool;
            //Debug.Log("Successes: " + successfullRolls + " Threshold: " + successThreshhold + "; Rolled Values: " + rolledValues + " Modified Values: " + modifiedRolledValues);


            return successfullRolls;
        }

        /// <summary>
        /// subtracts the amount of active modifying points from the results
        /// </summary>
        /// <param name="availableDices">the dices to be rolled</param>
        /// <param name="successThreshhold">the value after which a roll is successfull</param>
        /// <param name="activeModifierPool">Points that get distributed to modify dice rolls</param>
        /// <param name="rollResults">The earlier rolled and allready passive modified results</param>
        /// <returns> the new modified results/returns>
        private int[] SubtractModifier(int availableDices, int successThreshhold, int activeModifierPool, int[] rollResults)
        {
            for (int i = availableDices - 1; i >= 0; i--)
            {
                //roll is not success
                if (rollResults[i] >= successThreshhold)
                {
                    //check needed amount
                    int amountNeeded = rollResults[i] - (successThreshhold - 1);
                    //apply activeModifier if available
                    if (amountNeeded <= Mathf.Abs(activeModifierPool))
                    {

                        rollResults[i] -= amountNeeded;
                        activeModifierPool += amountNeeded;
                    }
                    //not enough modifier left
                    else
                    {
                        activeModifierPool = 0;
                        break;
                    }
                }
            }

            return rollResults;
        }

        /// <summary>
        /// adds the amount of active modifying points from the results
        /// </summary>
        /// <param name="availableDices">the dices to be rolled</param>
        /// <param name="successThreshhold">the value after which a roll is successfull</param>
        /// <param name="activeModifierPool">Points that get distributed to modify dice rolls</param>
        /// <param name="rollResults">The earlier rolled and allready passive modified results</param>
        /// <returns> the new modified results/returns>
        private int[] AddModifier(int availableDices, int successThreshhold, int activeModifierPool, int[] rollResults)
        {
            for (int i = 0; i < availableDices; i++)
            {
                //roll is not success
                if (rollResults[i] < successThreshhold)
                {
                    //check needed amount
                    int amountNeeded = successThreshhold - rollResults[i];
                    //apply activeModifier if available
                    if (amountNeeded <= activeModifierPool)
                    {

                        rollResults[i] += amountNeeded;
                        activeModifierPool -= amountNeeded;
                    }
                    //not enough modifier left
                    else
                    {
                        activeModifierPool = 0;
                        break;
                    }
                }
            }

            return rollResults;
        }

    }

}

