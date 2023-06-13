using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.Character.monster
{
    [CreateAssetMenu(fileName = "Monster", menuName = "Hexer_ScriptableObjects/Mobs/Monster")]
    public class SO_Monster : ScriptableObject
    {
        public string monsterName;
        public Image monsterImage;

        public MonsterCharacter.BasicStats basicStats;
        public MonsterCharacter.MonsterStats monsterStats;
        public MonsterCharacter.OffensivStats offensivStats;
        public MonsterCharacter.DefensiveStats defensiveStats;
    }

}
