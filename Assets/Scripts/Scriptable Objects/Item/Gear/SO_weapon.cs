using Hexerspiel.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Hexer_ScriptableObjects/Items/Gear/Weapon")]
    public class SO_weapon : SO_gear
    {
        public const GearType gearType = GearType.weapon;
        public PlayerCharacter.OffensivStats weaponStats;
    }
}