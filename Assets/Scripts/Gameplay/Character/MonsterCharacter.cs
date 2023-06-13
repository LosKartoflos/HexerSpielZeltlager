using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    public class MonsterCharacter : BasicCharacterValues
    {
        [Serializable]
        public struct MonsterStats
        {
            //Basic
            [Range(1, 10)]
            public int level;
            public int gold;
            public int xp;
            //Drops
            public int herbs, meat, magicEssence;
            //these items are only droped the first time they beat the monster
            public List<SO_gear> dropedGear;
            public List<SO_questItem> dropedQuestItems;

        }
    }
}