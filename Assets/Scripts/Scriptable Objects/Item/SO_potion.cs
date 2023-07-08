using Hexerspiel.Character;
using Hexerspiel.gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Hexer_ScriptableObjects/Items/Potion")]
    public class SO_potion : SO_item
    {
        protected override ItemType itemType => ItemType.potion;
        public PotionStats potionStats;
    }
}