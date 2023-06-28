using Hexerspiel.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    [Serializable]
    public class BasicInventoryCounters
    {
        public int gold;
        public MiscItems miscItems;

        public BasicInventoryCounters()
        {
            this.gold = 0;
            this.miscItems.herbs = 0;
            this.miscItems.magicEssence = 0;
            this.miscItems.meat = 0;
        }

        public BasicInventoryCounters(int gold, MiscItems miscItems)
        {
            this.gold = gold;
            this.miscItems = miscItems;
        }

        public BasicInventoryCounters(int gold, int herbs, int magicEssence, int meat)
        {
            this.gold = gold;
            this.miscItems.herbs = herbs;
            this.miscItems.magicEssence = magicEssence;
            this.miscItems.meat = meat;
        }

    }
}