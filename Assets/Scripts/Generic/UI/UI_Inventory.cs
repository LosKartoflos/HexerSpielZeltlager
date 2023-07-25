using Hexerspiel.Character;
using Hexerspiel.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{

    public class UI_Inventory : MonoBehaviour
    {
        public enum UiInventoryMode { overview, inDepth }


        #region Variables
        [Header("Header")]
        [SerializeField]
        TextMeshProUGUI label_Header;

        [SerializeField]
        Button bt_previous, bt_next;

        [SerializeField]
        TextMeshProUGUI label_page;

        [Header("Content")]

        [SerializeField]
        RectTransform contentContainer;

        [SerializeField]
        GameObject uiObjectPrefab;

        [Header("Footer")]
        [SerializeField]
        Button bt_look, bt_sell, bt_drop, bt_sellOrUse;

        private UiInventoryMode inventoryMode = UiInventoryMode.overview;
        private ItemType typeSelected = ItemType.none;

        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {
            FillPotion();
        }
        #endregion

        #region Functions
        public void FillArmor()
        {
            if (Player.Instance.Inventory.GearInventory.ArmorsCollected.Count == 0)
                return;

            List<SO_armor> sortedList = Player.Instance.Inventory.GearInventory.ArmorsCollected.OrderBy(o => o.itemName).ToList();
            foreach (SO_armor armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor);
            }
        }

        public void FillWeapon()
        {
            if (Player.Instance.Inventory.GearInventory.WeaponsCollected.Count == 0)
                return;

            List<SO_weapon> sortedList = Player.Instance.Inventory.GearInventory.WeaponsCollected.OrderBy(o => o.itemName).ToList();
            foreach (SO_weapon armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor);
            }
        }

        public void FillAmulet()
        {
            if (Player.Instance.Inventory.GearInventory.AmuletCollected.Count == 0)
                return;

            List<SO_amulet> sortedList = Player.Instance.Inventory.GearInventory.AmuletCollected.OrderBy(o => o.itemName).ToList();
            foreach (SO_amulet armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor);
            }
        }

        public void FillPotion()
        {
            if (Player.Instance.Inventory.PotionInventory.PotionList.Count == 0)
                return;

            List<SO_potion> sortedList = Player.Instance.Inventory.PotionInventory.PotionList.OrderBy(o => o.itemName).ToList();
            foreach (SO_potion armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor);
            }
        }

        public void FillQuestItem()
        {
            if (Player.Instance.Inventory.QuestItemInventory.QuestItemsList.Count == 0)
                return;

            List<SO_questItem> sortedList = Player.Instance.Inventory.QuestItemInventory.QuestItemsList.OrderBy(o => o.itemName).ToList();
            foreach (SO_questItem armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor);
            }
        }

        public void CreateObjectItem(SO_item newItem)
        {
            GameObject contentItem = Instantiate(uiObjectPrefab, contentContainer, false);

            //Setup
            contentItem.GetComponent<UIObjectItem>().SetupObjecItem(newItem, newItem.itemImage);

            //Highlight
            //if (contentContainer.childCount == 1)
            //    contentItem.GetComponent<UIObjectItem>().SetHighlight(true);
        }
        #endregion
    }

}