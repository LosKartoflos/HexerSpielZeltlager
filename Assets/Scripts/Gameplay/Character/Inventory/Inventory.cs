using Hexerspiel.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Character
{
    public class Inventory : MonoBehaviour
    {

        [SerializeField]
        private List<SO_item> itemsList = new List<SO_item>();

        [SerializeField]
        private List<SO_questItem> questItemsList = new List<SO_questItem>();

        [SerializeField]
        private List<SO_potion> potionList = new List<SO_potion>();

        public void GetItem(SO_item newItem)
        {
            switch (newItem.ItemType)
            {
                case ItemType.potion:
                    potionList.Add((SO_potion)newItem);
                    break;              
                case ItemType.misc:
                    Debug.LogError("access it over basicinventory");
                    break;
                case ItemType.none:
                    break;
                case ItemType.gear:
                    break;
                case ItemType.quest:
                    break;
            }
        }

        //public void EquipGear(SO_gear newGear)
        //{
        //    switch (newGear.GearType)
        //    {
        //        case GearType.armor:
        //            if (armorsCollected.Contains((SO_armor)newGear))
        //                armorEquiped = (SO_armor)newGear;
        //            else
        //                Debug.Log("You do not own " + newGear.itemName);
        //            break;
        //        case GearType.weapon:
        //            if (weaponsCollected.Contains((SO_weapon)newGear))
        //                weaponEquipped = (SO_weapon)newGear;
        //            else
        //                Debug.Log("You do not own " + newGear.itemName);
        //            break;
        //        case GearType.amulet:
        //            if (amuletCollected.Contains((SO_amulet)newGear))
        //                amuletEquipped = (SO_amulet)newGear;
        //            else
        //                Debug.Log("You do not own " + newGear.itemName);
        //            break;
        //        case GearType.none:
        //            Debug.LogError("No gear type for " + newGear.itemName);
        //            break;
        //    }

        //    EquipGearChanged();
        //}

        //public void UnequipGear(GearType gearTypeToUnqequip)
        //{
        //    switch (gearTypeToUnqequip)
        //    {
        //        case GearType.armor:
        //            armorEquiped = null;
        //            break;
        //        case GearType.weapon:
        //            weaponEquipped = null;
        //            break;
        //        case GearType.amulet:
        //            amuletEquipped = null;
        //            break;
        //        case GearType.none:
        //            Debug.LogError("Nothing cannot be unequipped and you cannot divide by zero");
        //            break;
        //    }

        //    EquipGearChanged();
        //}

        //public bool DropGear(SO_gear gearToDrop)
        //{
        //    bool soldSuccesfull = false;

        //    switch (gearToDrop.GearType)
        //    {
        //        case GearType.armor:
        //            if (armorsCollected.Contains((SO_armor)gearToDrop))
        //            {
        //                armorsCollected.Remove((SO_armor)gearToDrop);
        //                soldSuccesfull = true;
        //            }

        //            else
        //                Debug.Log("You do not own " + gearToDrop.itemName);

        //            if ((SO_armor)gearToDrop == armorEquiped)
        //                UnequipGear(GearType.armor);

        //            break;
        //        case GearType.weapon:
        //            if (weaponsCollected.Contains((SO_weapon)gearToDrop))
        //            {
        //                weaponsCollected.Remove((SO_weapon)gearToDrop);
        //                soldSuccesfull = true;
        //            }
        //            else
        //                Debug.Log("You do not own " + gearToDrop.itemName);

        //            if ((SO_weapon)gearToDrop == weaponEquipped)
        //                UnequipGear(GearType.armor);

        //            break;
        //        case GearType.amulet:
        //            if (amuletCollected.Contains((SO_amulet)gearToDrop))
        //            {
        //                amuletCollected.Remove((SO_amulet)gearToDrop);
        //                soldSuccesfull = true;
        //            }
        //            else
        //                Debug.Log("You do not own " + gearToDrop.itemName);

        //            if ((SO_amulet)gearToDrop == amuletEquipped)
        //                UnequipGear(GearType.armor);

        //            break;
        //        case GearType.none:
        //            Debug.LogError("No gear type for " + gearToDrop.itemName);
        //            break;
        //    }

        //    if (soldSuccesfull)
        //        EquipGearChanged();

        //    return soldSuccesfull;
        //}

        //public bool SellGear(SO_gear gearToSell)
        //{

        //    if (DropGear(gearToSell))
        //    {
        //        PlayerCharacter.Instance.Inventory.BasicInventory.ChangeGold(gearToSell.valueSell);
        //        Debug.Log("Sell " + gearToSell.name + " for " + gearToSell.valueSell.ToString());
        //        return true;
        //    }

        //    return false;
        //}

        //public bool BuyGear(SO_gear gearTobuy)
        //{

        //    if (PlayerCharacter.Instance.Inventory.BasicInventory.ChangeGold(-gearTobuy.valueBuy))
        //    {
        //        GetGear(gearTobuy);
        //        Debug.Log("Buy " + gearTobuy.name + " for " + gearTobuy.valueBuy.ToString());
        //        return true;
        //    }
        //    return false;
        //}

        [SerializeField]
        private BasicInventory basicInventory = new BasicInventory();

        public BasicInventory BasicInventory { get => basicInventory; set => basicInventory = value; }


    }
}