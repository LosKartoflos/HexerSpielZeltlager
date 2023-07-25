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
        public  PlayerCharacterValues.DefensiveStats armorStats;
        public CharacterType armorTypeMod;
        public CharacterMovement armorMovementMod;

        public override string GetDescription()
        {
            return string.Format("Rüstungswert: {0}\nRüstungstyp: {1}\nBewegungsstil: {2}", armorStats.armor, armorTypeMod.ToString(), armorMovementMod.ToString());
        }

        public override string GetDescriptionShort()
        {
            return string.Format("R: {0}, T: {1}, B: {2}", armorStats.armor, armorTypeMod.ToString(), armorMovementMod.ToString());
        }

    }
}