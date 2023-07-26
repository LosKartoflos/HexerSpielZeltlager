using Hexerspiel.Character;
using Hexerspiel.Character.monster;
using Hexerspiel.Items;
using Hexerspiel.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Hexerspiel.Dice;

namespace Hexerspiel.Fight
{
    public class FightManager : MonoBehaviour
    {
        public enum FIGHT_ENDING { none, playerWon, enemyWon }

        #region Variables

        public static event Action<int> RoundFought = delegate { };
        public static event Action<MonsterCharacter> PlayerWonEvent = delegate { };
        public static event Action PlayerLostEvent = delegate { };

        int[] damageFromPlayer;
        int[] damageFromEnemy;

        int extraDiceForPlayer;
        int extraPointsForPlayer;

        int malusDiceForEnemy;
        int malusPointsForEnemy;

        private string playerInfo = "";
        private string enemyInfo = "";
        private RollInfos rollInfosPlayer;
        private RollInfos rollInfosEnemy;


        int round = 1;

        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {
            // check why random of dice is null;

            round = 1;
        }

        private void OnEnable()
        {
            round = 1;
            PotionInventory.PotionUsed += DrinkPotion;
        }
        private void OnDisable()
        {
            PotionInventory.PotionUsed -= DrinkPotion;
        }
        #endregion

        #region Functions

        public void ProgressFight()
        {
            FIGHT_ENDING end = FIGHT_ENDING.none;
            end = FightARound();

            //player won
            if (end == FIGHT_ENDING.playerWon)
            {
                PlayerWon();
            }

            //enemy won
            else if (end == FIGHT_ENDING.enemyWon)
            {
                PlayerLost();
            }
        }
        /// <summary>
        /// A Round in a fight
        /// </summary>
        /// <returns>if the fight is over because one contester is dead, returns false</returns>
        public FIGHT_ENDING FightARound()
        {
            FIGHT_ENDING fighIsOver = FIGHT_ENDING.none;
            //player Attacks

            //apply amulet dice modifiers
            UseAmulet(Player.Instance.Inventory.GearInventory.AmuletEquipped);

            Debug.Log(string.Format("Player attacks now. Enemy has {1}hp.", Fight.enemy.MonsterName, Fight.enemy.BasicStatsValue.health));
            int[] damageDealtByPlayer = Fight.player.Attack(0, extraPointsForPlayer, extraDiceForPlayer, Fight.enemy.BasicStatsValue.characterType, Fight.enemy.BasicStatsValue.characterMovement, out rollInfosPlayer);
            //add attribute Damage 


            int finalDamagetToMonster = Fight.enemy.Defend(damageDealtByPlayer[0]);
            Debug.Log(string.Format("Player fought. Damge dealt {0}. Damaged modified {2}, Enemy has {1}hp.", damageDealtByPlayer[0], Fight.enemy.BasicStatsValue.health, damageDealtByPlayer[1]));


            //falls monster schon besiegt ist
            if (Fight.enemy.BasicStatsValue.health <= 0)
            {
                Fight.enemy.SetLife(0);
                Fight.enemy.Died();

                fighIsOver = FIGHT_ENDING.playerWon;

                ClearExtraDiceAndPoints();
                Player.Instance.PlayerValues.SetLife(Fight.player.GetLife());

                playerInfo = string.Format(UI_Fight.PLAYER_INFO_FORMAT,
                Fight.player.BasicStatsValue.health,//0
                Fight.player.GetMana().ToString(),//1
                "0",//2
                Fight.player.DeffensiveStatsValue.armor.ToString(),//3
                "0",//4
                rollInfosPlayer.rolledValues,//5
                rollInfosPlayer.threshold.ToString(),//6
                rollInfosPlayer.mods.ToString(),//7
                rollInfosPlayer.modedValues,//8
                rollInfosPlayer.succssess,//9
                damageDealtByPlayer[1],//10
                damageDealtByPlayer[0],
                Fight.player.BasicStatsValue.characterType.ToString(),
                Fight.player.BasicStatsValue.characterMovement.ToString(),
                Fight.player.OffensivStatsValue.weaponRange.ToString(),
                Fight.player.OffensivStatsValue.damageType.ToString()
                );

                UI_Fight.Instance.UpdateAfterFightRound(round, playerInfo, "<b>Gegner:</b>\nIst besiegt");
                RoundFought(round);
                return fighIsOver;


            }

            //Enemy Attacks
            Debug.Log(string.Format("{0} attacks now. Player has {1}hp.", Fight.enemy.MonsterName, Fight.player.BasicStatsValue.health));
            int[] damageDealtByEnemy = Fight.enemy.Attack(0, malusPointsForEnemy, malusDiceForEnemy, Fight.player.BasicStatsValue.characterType, Fight.player.BasicStatsValue.characterMovement, out rollInfosEnemy);
            int finalDamagetToPlayer = Fight.player.Defend(damageDealtByEnemy[0]);
            Debug.Log(string.Format("Round fought with. Damge dealt {0}. Damaged modified {2}, Player has {1}hp.", damageDealtByEnemy[0], Fight.player.BasicStatsValue.health, damageDealtByEnemy[1]));

            //Round fought check for results and reset;

            //falls spieler besiegt ist
            if (Fight.player.BasicStatsValue.health <= 0)
            {
                Fight.player.SetLife(0);
                Player.Instance.PlayerValues.Died();

                ClearExtraDiceAndPoints();

                enemyInfo = string.Format(UI_Fight.Enemy_INFO_FORMAT,
                Fight.enemy.BasicStatsValue.health,//0
                damageDealtByPlayer[0].ToString(),//1
                Fight.enemy.DeffensiveStatsValue.armor.ToString(),//2
                finalDamagetToMonster,//3
                rollInfosEnemy.rolledValues,//4
                rollInfosEnemy.threshold.ToString(),//5
                rollInfosEnemy.mods.ToString(),//6
                rollInfosEnemy.modedValues,//7
                rollInfosEnemy.succssess,//8
                damageDealtByEnemy[1],//9
                damageDealtByEnemy[0],
                Fight.enemy.BasicStatsValue.characterType.ToString(),
                Fight.enemy.BasicStatsValue.characterMovement.ToString(),
                Fight.enemy.OffensivStatsValue.weaponRange.ToString(),
                Fight.enemy.OffensivStatsValue.damageType.ToString()
            );

                UI_Fight.Instance.UpdateAfterFightRound(round, "<b>Spieler:</b>\nDu wurdest besiegt", enemyInfo);
                RoundFought(round);
                return fighIsOver = FIGHT_ENDING.enemyWon;
            }


            // "Spieler:\nHP: {0}\nMana{1}\nSchaden erhalten: {2} - {3} = {4}\nWürfel gerolt: {5}\nThreshhol{6}, Modifikator:{7}\nWürfel modifiziert:{8}\nErfolge:{9}\n\nWaffenart und Gegnertypbonus: {10}";

            playerInfo = string.Format(UI_Fight.PLAYER_INFO_FORMAT,
                Fight.player.BasicStatsValue.health,//0
                Fight.player.GetMana().ToString(),//1
                damageDealtByEnemy[0].ToString(),//2
                Fight.player.DeffensiveStatsValue.armor.ToString(),//3
                finalDamagetToPlayer,//4
                rollInfosPlayer.rolledValues,//5
                rollInfosPlayer.threshold.ToString(),//6
                rollInfosPlayer.mods.ToString(),//7
                rollInfosPlayer.modedValues,//8
                rollInfosPlayer.succssess,//9
                damageDealtByPlayer[1],//10
                damageDealtByPlayer[0],
                Fight.player.BasicStatsValue.characterType.ToString(),
                Fight.player.BasicStatsValue.characterMovement.ToString(),
                Fight.player.OffensivStatsValue.weaponRange.ToString(),
                Fight.player.OffensivStatsValue.damageType.ToString()
                );
            // "Spieler:\nHP: {0}\n\nSchaden erhalten: {1} - {2} = {3}\nWürfel gerolt: {4}\nThreshhol{5}, Modifikator:{6}\nWürfel modifiziert:{7}\nErfolge:{8}\n\nWaffenart und Gegnertypbonus: {9}";
            enemyInfo = string.Format(UI_Fight.Enemy_INFO_FORMAT,
                Fight.enemy.BasicStatsValue.health,//0
                damageDealtByPlayer[0].ToString(),//1
                Fight.enemy.DeffensiveStatsValue.armor.ToString(),//2
                finalDamagetToMonster,//3
                rollInfosEnemy.rolledValues,//4
                rollInfosEnemy.threshold.ToString(),//5
                rollInfosEnemy.mods.ToString(),//6
                rollInfosEnemy.modedValues,//7
                rollInfosEnemy.succssess,//8
                damageDealtByEnemy[1],//9
                damageDealtByEnemy[0],
                Fight.enemy.BasicStatsValue.characterType.ToString(),
                Fight.enemy.BasicStatsValue.characterMovement.ToString(),
                Fight.enemy.OffensivStatsValue.weaponRange.ToString(),
                Fight.enemy.OffensivStatsValue.damageType.ToString()
            );

            UI_Fight.Instance.UpdateAfterFightRound(round, playerInfo, enemyInfo);

            //invoke event
            RoundFought(round);
            round += 1;
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

        private void PlayerWon()
        {
            
            Player.Instance.RecieveLootMonster(Fight.enemy.MonsterStat);
            PlayerWonEvent(Fight.enemy);
        }

        private void PlayerLost()
        {
            PlayerLostEvent();
        }
        #endregion
    }
}
