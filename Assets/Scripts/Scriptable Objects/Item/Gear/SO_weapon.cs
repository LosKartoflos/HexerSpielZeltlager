using Hexerspiel.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Hexer_ScriptableObjects/Items/Gear/Weapon")]
    public class SO_weapon : SO_gear
    {
        protected override GearType gearType => GearType.weapon;
        public PlayerCharacterValues.OffensivStats weaponStats;

        public int ExtraAttack()
        {
            if(weaponStats.damageType == DamageType.Magisch)
            {
                return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributes1.mind / Player.Instance.PlayerValues.PlayerAttributes1.attributAddThreshold);
            }
            
            else if (weaponStats.damageType == DamageType.Normal)
            {
                return Mathf.FloorToInt(Player.Instance.PlayerValues.PlayerAttributes1.body / Player.Instance.PlayerValues.PlayerAttributes1.attributAddThreshold);
            }

            return 0;
        }

        public override string GetDescription()
        {
            return string.Format("Schadenstype: {0}\nReichweite: {1}\nAngriffswürfel: {2}\nTrefferwert{3}", weaponStats.damageType, weaponStats.weaponRange,weaponStats.attackDice,weaponStats.succesThreshold);
        }

        public override string GetDescriptionShort()
        {
            return string.Format("S: {0}, R: {1}, AW: {2}, T {3}", weaponStats.damageType, weaponStats.weaponRange, weaponStats.attackDice, weaponStats.succesThreshold);
        }
    }
}