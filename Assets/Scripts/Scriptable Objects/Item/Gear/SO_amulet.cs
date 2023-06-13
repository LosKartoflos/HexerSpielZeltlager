using Hexerspiel.Character;
using Hexerspiel.gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Amulet", menuName = "Hexer_ScriptableObjects/Items/Gear/Amulet")]
    public class SO_amulet : SO_gear
    {
        public const GearType gearType = GearType.amulet;
        public DiceRoll.DiceManipulation diceManipulation;
    }
}
