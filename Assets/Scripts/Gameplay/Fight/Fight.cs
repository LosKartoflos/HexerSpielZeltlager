using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.Fight
{
    /// <summary>
    /// manages the fight procedure
    /// </summary>
    public  class Fight
    {

        #region Variables
        [SerializeField]
        public static BasicCharacter player;
        [SerializeField]
        public static MonsterCharacter enemy;
        public static int round = 1;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        #endregion

        #region Functions
        public static void StartFightingScene(SO_Monster so_monster)
        {
            SceneManager.LoadScene("FightScene");
            enemy = new MonsterCharacter(so_monster);
            player = (BasicCharacter) PlayerCharacter.Instance;

        }

        #endregion
        //private void Attack(out BasicCharacterValues attacker, out BasicCharacterValues basicCharacterValues)
        //{

        //}

    }

}
