using Hexerspiel;
using Hexerspiel.Character;
using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Character
{
    [RequireComponent(typeof(Inventory))]
    public class GearInventory : MonoBehaviour
    {
        public static event Action EquipGearChanged = delegate { };

        public static event Action<string> AlertGearChanged = delegate { };

        [SerializeField]
        private List<SO_armor> armorsCollected = new List<SO_armor>();

        [SerializeField]
        private List<SO_amulet> amuletCollected = new List<SO_amulet>();

        [SerializeField]
        private List<SO_weapon> weaponsCollected = new List<SO_weapon>();

        [SerializeField]
        private SO_armor armorEquiped;

        [SerializeField]
        private SO_amulet amuletEquipped;

        [SerializeField]
        private SO_weapon weaponEquipped;

        public List<SO_armor> ArmorsCollected { get => armorsCollected; }
        public List<SO_amulet> AmuletCollected { get => amuletCollected; }
        public List<SO_weapon> WeaponsCollected { get => weaponsCollected; }
        public SO_armor ArmorEquiped { get => armorEquiped; }
        public SO_amulet AmuletEquipped { get => amuletEquipped; }
        public SO_weapon WeaponEquipped { get => weaponEquipped; }

        public void GetGear(SO_gear newGear)
        {
            if (newGear == null)
            {
                Debug.LogError("Das newGear ist leer");
            }
              

            switch (newGear.GearType)
            {
                case GearType.armor:
                    armorsCollected.Add((SO_armor)newGear);
                    break;
                case GearType.weapon:
                    weaponsCollected.Add((SO_weapon)newGear);
                    break;
                case GearType.amulet:
                    amuletCollected.Add((SO_amulet)newGear);
                    break;
                case GearType.none:
                    Debug.LogError("No gear type for " + newGear.name);
                    AlertGearChanged(newGear.name + " ist keine Gültige Ausrüstung!");
                    return;
                    break;
            }

            AlertGearChanged("Du hast " + newGear.itemName + " erhalten");
        }

        public void EquipGear(SO_gear newGear)
        {
            switch (newGear.GearType)
            {
                case GearType.armor:
                    if (armorsCollected.Contains((SO_armor)newGear))
                    {
                        armorEquiped = (SO_armor)newGear;
                        AlertGearChanged("Du hast " + newGear.itemName + " ausgerüstet!");
                    }
                    else
                        Debug.Log("You do not own " + newGear.itemName);
                    break;
                case GearType.weapon:
                    if (weaponsCollected.Contains((SO_weapon)newGear))
                    {
                        weaponEquipped = (SO_weapon)newGear;
                        AlertGearChanged("Du hast " + newGear.itemName + " ausgerüstet!");
                    }
                    else
                        Debug.Log("You do not own " + newGear.itemName);
                    break;
                case GearType.amulet:
                    if (amuletCollected.Contains((SO_amulet)newGear))
                    {
                        amuletEquipped = (SO_amulet)newGear;
                        AlertGearChanged("Du hast " + newGear.itemName + " ausgerüstet!");
                    }
                    else
                        Debug.Log("You do not own " + newGear.itemName);
                    break;
                case GearType.none:
                    Debug.LogError("No gear type for " + newGear.itemName);
                    break;
            }


            EquipGearChanged();
        }

        public void UnequipGear(GearType gearTypeToUnqequip)
        {
            switch (gearTypeToUnqequip)
            {
                case GearType.armor:
                    armorEquiped = null;
                    break;
                case GearType.weapon:
                    weaponEquipped = null;
                    break;
                case GearType.amulet:
                    amuletEquipped = null;
                    break;
                case GearType.none:
                    Debug.LogError("Nothing cannot be unequipped and you cannot divide by zero");
                    break;
            }

            EquipGearChanged();
        }

        public bool DropGear(SO_gear gearToDrop)
        {
            bool dropedSuccessfull = false;

            switch (gearToDrop.GearType)
            {
                case GearType.armor:
                    if (armorsCollected.Contains((SO_armor)gearToDrop))
                    {
                        armorsCollected.Remove((SO_armor)gearToDrop);
                        dropedSuccessfull = true;
                    }

                    else
                        Debug.Log("You do not own " + gearToDrop.itemName);

                    if ((SO_armor)gearToDrop == armorEquiped)
                        UnequipGear(GearType.armor);

                    break;
                case GearType.weapon:
                    if (weaponsCollected.Contains((SO_weapon)gearToDrop))
                    {
                        weaponsCollected.Remove((SO_weapon)gearToDrop);
                        dropedSuccessfull = true;
                    }
                    else
                        Debug.Log("You do not own " + gearToDrop.itemName);

                    if ((SO_weapon)gearToDrop == weaponEquipped)
                        UnequipGear(GearType.armor);

                    break;
                case GearType.amulet:
                    if (amuletCollected.Contains((SO_amulet)gearToDrop))
                    {
                        amuletCollected.Remove((SO_amulet)gearToDrop);
                        dropedSuccessfull = true;
                    }
                    else
                        Debug.Log("You do not own " + gearToDrop.itemName);

                    if ((SO_amulet)gearToDrop == amuletEquipped)
                        UnequipGear(GearType.armor);

                    break;
                case GearType.none:
                    Debug.LogError("No gear type for " + gearToDrop.itemName);
                    break;
            }

            if (dropedSuccessfull)
                EquipGearChanged();

            return dropedSuccessfull;
        }

        public bool SellGear(SO_gear gearToSell)
        {

            if (DropGear(gearToSell))
            {
                Player.Instance.Inventory.BasicInventory.ChangeGold(gearToSell.valueSell);
                Debug.Log("Sell " + gearToSell.name + " for " + gearToSell.valueSell.ToString());
                AlertGearChanged("Du hast " + gearToSell.itemName + " für " + gearToSell.valueSell + " Gold verkauft!");
                return true;
            }

            return false;
        }

        public bool BuyGear(SO_gear gearTobuy)
        {

            if (Player.Instance.Inventory.BasicInventory.ChangeGold(-gearTobuy.valueBuy))
            {
                GetGear(gearTobuy);
                Debug.Log("Buy " + gearTobuy.name + " for " + gearTobuy.valueBuy.ToString());
                AlertGearChanged("Du hast " + gearTobuy.itemName + " für " + gearTobuy.valueBuy + " Gold gekauft!");
                return true;
            }
            return false;
        }
    }
}