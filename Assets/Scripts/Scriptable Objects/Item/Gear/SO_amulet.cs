using Hexerspiel.Character;
using Hexerspiel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Amulet", menuName = "Hexer_ScriptableObjects/Items/Gear/Amulet")]
    public class SO_amulet : SO_gear
    {
        protected override GearType gearType => GearType.amulet;
        public Dice.Manipulation diceManipulation;
    }
}
