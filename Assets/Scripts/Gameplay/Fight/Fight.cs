using Hexerspiel.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Fight
{
    /// <summary>
    /// manages the fight procedure
    /// </summary>
    public class Fight : MonoBehaviour
    {
        [SerializeField]
        PlayerCharacter player;
        [SerializeField]
        MonsterCharacter enemy;

        private int round = 1;

        public static Action fightStarted = delegate { };

        public int Round { get => round;  }

        public Fight(PlayerCharacter player, MonsterCharacter enemy)
        {
            this.player = player;
            this.enemy = enemy;

            round = 1;

            fightStarted();
        }


        //private void Attack(out BasicCharacterValues attacker, out BasicCharacterValues basicCharacterValues)
        //{

        //}

    }

}
