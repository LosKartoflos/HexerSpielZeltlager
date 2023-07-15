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
    public class Fight : MonoBehaviour
    {

        #region Variables
        [SerializeField]
        public static PlayerCharacterValues player = new PlayerCharacterValues();
        [SerializeField]
        public static MonsterCharacter enemy = new MonsterCharacter();
       // public static int round = 1;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        #endregion

        #region Functions
        public static void StartFightingScene(SO_Monster so_monster)
        {
            enemy = new MonsterCharacter(so_monster);
            player = Player.Instance.PlayerValues;
            SceneManager.LoadScene("FightScene");

        }

        //public void FightARound()
        //{
        //    if (enemy == null || player == null)
        //    {
        //        Debug.LogError("No enemay or player");
        //        return;
        //    }


                
        //}


        


        #endregion


    }

}
