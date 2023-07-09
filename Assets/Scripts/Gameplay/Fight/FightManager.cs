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
            //Debug.Log(string.Format("Fight start with {0}. Player has {1}hp.", Fight.enemy.name, Fight.player.BasicStatsValue.health));
            Debug.Log(Fight.enemy.name);
        }
        #endregion

        #region Functions


        #endregion
    }
}
