using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.Fight
{
    public class FightManager : MonoBehaviour
    {
        #region Variables
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {
            Debug.Log(string.Format("Fight start with {0}. Player has {1}hp.", Fight.enemy.MonsterName, Fight.player.BasicStatsValue.health));

            Fight.enemy.Attack(0,0,Fight.player.BasicStatsValue.characterType,Fight.player.BasicStatsValue.characterMovement);

            Debug.Log(string.Format("Fight start with {0}. Player has {1}hp.", Fight.enemy.MonsterName, Fight.player.BasicStatsValue.health)); 


        }
        #endregion

        #region Functions


        #endregion
    }
}
