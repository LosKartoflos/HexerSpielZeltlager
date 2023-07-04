using Hexerspiel.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Armor", menuName = "Hexer_ScriptableObjects/Items/Gear/Armor")]
    public class SO_armor : SO_gear
    {
        protected override GearType gearType => GearType.armor;
        public  PlayerCharacter.DefensiveStats armorStats;
    }
}