using Hexerspiel.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Hexer_ScriptableObjects/Items/Gear/Weapon")]
    public class SO_weapon : SO_gear
    {
        protected override GearType gearType => GearType.armor;
        public PlayerCharacter.OffensivStats weaponStats;
    }
}