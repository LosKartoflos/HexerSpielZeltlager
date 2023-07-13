using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexerspiel.Fight
{
    public class FightManager : MonoBehaviour
    {
        #region Variables
        int[] damageFromPlayer;
        int[] damageFromEnemy;

        int extraDiceForPlayer;
        int extraPointsForPlayer;

        int malusDiceForEnemy;
        int malusPointsForEnemy;

        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {
            // check why random of dice is null;
           

        }

        private void OnEnable()
        {
            PotionInventory.PotionUsed += DrinkPotion;
        }
        private void OnDisable()
        {
            PotionInventory.PotionUsed -= DrinkPotion;
        }
        #endregion

        #region Functions

        public void FightTest()
        {
            FightARound();
        }
        /// <summary>
        /// A Round in a fight
        /// </summary>
        /// <returns>if the fight is over because one contester is dead, returns false</returns>
        public bool FightARound()
        {
            //to do: give back winner or if still fighting
            bool fighIsOver = false;
            //player Attacks

            //apply amulet dice modifiers
            UseAmulet(Player.Instance.Inventory.GearInventory.AmuletEquipped);

            Debug.Log(string.Format("Player attacks now. Enemy has {1}hp.", Fight.enemy.MonsterName, Fight.enemy.BasicStatsValue.health));
            int[] damageDealtByPlayer = Fight.player.Attack(0, extraPointsForPlayer, extraDiceForPlayer, Fight.enemy.BasicStatsValue.characterType, Fight.enemy.BasicStatsValue.characterMovement);
            Fight.enemy.Defend(damageDealtByPlayer[0]);
            Debug.Log(string.Format("Player fought. Damge dealt {0}. Damaged modified {2}, Enemy has {1}hp.", damageDealtByPlayer[0], Fight.enemy.BasicStatsValue.health, damageDealtByPlayer[1]));


            //falls monster schon besiegt ist
            if (Fight.enemy.BasicStatsValue.health <= 0)
            {
                Fight.player.SetLife(0);
                Player.Instance.PlayerValues.Died();
                fighIsOver = true;

                ClearExtraDiceAndPoints();
                return fighIsOver;

                
            }

            //Enemy Attacks
            Debug.Log(string.Format("{0} attacks now. Player has {1}hp.", Fight.enemy.MonsterName, Fight.player.BasicStatsValue.health));
            int[] damageDealtByEnemy = Fight.enemy.Attack(0, malusPointsForEnemy, malusDiceForEnemy, Fight.player.BasicStatsValue.characterType, Fight.player.BasicStatsValue.characterMovement);
            Fight.player.Defend(damageDealtByEnemy[0]);
            Debug.Log(string.Format("Round fought with. Damge dealt {0}. Damaged modified {2}, Player has {1}hp.", damageDealtByEnemy[0], Fight.player.BasicStatsValue.health, damageDealtByEnemy[1]));

            //Round fought check for results and reset;

            //falls spieler besiegt ist
            if(Fight.player.BasicStatsValue.health <= 0)
            {
                Fight.enemy.SetLife(0);
                Fight.enemy.Died();
                Player.Instance.PlayerValues.SetLife(Fight.player.GetLife());
                fighIsOver = true;
            }

            
            ClearExtraDiceAndPoints();

            return fighIsOver;
        }

        /// <summary>
        /// use a potion in fight
        /// </summary>
        /// <param name="potionStats">potion to use</param>
        public void DrinkPotion(PotionStats potionStats)
        {
            Fight.player.AddLife(potionStats.addLife);
            Fight.player.AddMana(potionStats.addMana);

            extraDiceForPlayer = potionStats.diceManipulation.addDice;
            extraPointsForPlayer = potionStats.diceManipulation.addablePoints;

            malusDiceForEnemy = potionStats.diceManipulation.substractDiceFromEnemy;
            malusPointsForEnemy = potionStats.diceManipulation.subtractablePointsFromEnemy;
        }

        public void UseAmulet(SO_amulet amulet)
        {
            if (amulet == null)
                return;

            extraDiceForPlayer = amulet.diceManipulation.addDice;
            extraPointsForPlayer = amulet.diceManipulation.addablePoints;

            malusDiceForEnemy = amulet.diceManipulation.substractDiceFromEnemy;
            malusPointsForEnemy = amulet.diceManipulation.subtractablePointsFromEnemy;
        }

        public void ClearExtraDiceAndPoints()
        {
            extraDiceForPlayer = 0;
            extraPointsForPlayer = 0;

            malusDiceForEnemy = 0;
            malusPointsForEnemy = 0;
        }

        #endregion
    }
}
