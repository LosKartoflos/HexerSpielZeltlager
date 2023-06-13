using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.gameplay {
    public class DiceRoll : MonoBehaviour
    {
        [Serializable]
        public struct DiceManipulation
        {
            public int addDice;
            public int substractDiceFromEnemy;
            public int addablePoints;
            public int subtractablePointsFromEnemy;

        }
    }

}
