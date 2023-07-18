using Hexerspiel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Quests
{
    public class SO_rightAnswer : ScriptableObject
    {
        public RighAnswer givenAnswer = RighAnswer.a;

        public SO_rightAnswer(RighAnswer givenAnswer)
        {
            this.givenAnswer = givenAnswer;
        }
    } 
}
