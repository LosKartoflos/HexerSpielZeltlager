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
        public PlayerCharacterValues.OffensivStats weaponStats;

        public int ExtraAttack()
        {
            if(weaponStats.damageType == DamageType.magical)
            {
                return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributes1.mind / Player.Instance.PlayerValues.PlayerAttributes1.attributAddThreshold);
            }
            
            else if (weaponStats.damageType == DamageType.normal)
            {
                return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributes1.body / Player.Instance.PlayerValues.PlayerAttributes1.attributAddThreshold);
            }

            return 0;
        }
    }
}