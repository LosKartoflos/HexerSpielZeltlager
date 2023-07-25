using Hexerspiel.Character;
using Hexerspiel.Items;
using System;
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
        public static event Action<string> AlertLookUp = delegate { };

        [Header("Header")]
        [SerializeField]
        TextMeshProUGUI label_Header;

        [SerializeField]
        Button bt_close;


        [Header("Content")]

        [SerializeField]
        RectTransform contentContainer;

        [SerializeField]
        GameObject uiObjectPrefab;

        [SerializeField]
        ItemEquipElement armorEquip, weaponEquip, amuletEquip, potionEquip, questItemEquip;

        [SerializeField]
        GameObject overlay;

        [Header("Footer")]
        [SerializeField]
        Button bt_look, bt_sell, bt_drop, bt_sellOrUse;

        private SO_item currentitemSelected;

        private ItemType menuOpendForItem = ItemType.none;
        private GearType menuOpenedForGear = GearType.none;

        private UiInventoryMode inventoryMode = UiInventoryMode.overview;

        bool highlightFirstEquipped = false;

        #endregion

        #region Accessors
        #endregion

        #region LifeCycle


        private void OnEnable()
        {
            UIObjectItem.RecieveSelectedItem += ItemSelected;

        }

        private void OnDisable()
        {
            UIObjectItem.RecieveSelectedItem -= ItemSelected;
        }
        private void Start()
        {
            closeOverlay();

            //assign Equip button
            bt_close.onClick.AddListener(closeOverlay);
            armorEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.gear, GearType.armor); });
            weaponEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.gear, GearType.weapon); });
            amuletEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.gear, GearType.amulet); });
            potionEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.potion); });
            questItemEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.quest); });

            //style equip buttons
            if (Player.Instance.Inventory.GearInventory.ArmorEquiped != null)
                armorEquip.ChangeAppreance(Player.Instance.Inventory.GearInventory.ArmorEquiped.itemName, Player.Instance.Inventory.GearInventory.ArmorEquiped.GetDescriptionShort(), Player.Instance.Inventory.GearInventory.ArmorEquiped.itemImage);
            else
                armorEquip.ChangeAppreance(null, null);

            if (Player.Instance.Inventory.GearInventory.WeaponEquipped != null)
                weaponEquip.ChangeAppreance(Player.Instance.Inventory.GearInventory.WeaponEquipped.itemName, Player.Instance.Inventory.GearInventory.WeaponEquipped.GetDescriptionShort(), Player.Instance.Inventory.GearInventory.WeaponEquipped.itemImage);
            else
                weaponEquip.ChangeAppreance(null, null);

            if (Player.Instance.Inventory.GearInventory.AmuletEquipped != null)
                amuletEquip.ChangeAppreance(Player.Instance.Inventory.GearInventory.AmuletEquipped.itemName, Player.Instance.Inventory.GearInventory.AmuletEquipped.GetDescriptionShort(), Player.Instance.Inventory.GearInventory.AmuletEquipped.itemImage);
            else
                amuletEquip.ChangeAppreance(null, null);

            potionEquip.ChangeAppreance(null, null);
            questItemEquip.ChangeAppreance(null, null);

            //add listener to normal buttons

            bt_look.onClick.AddListener(delegate { lookAtItem(currentitemSelected); });

        }
        #endregion

        #region Functions
        public void ItemSelected(SO_item newItem)
        {
            currentitemSelected = newItem;
        }

        //bt effects


        public void lookAtItem(SO_item itemToLook)
        {
            if (currentitemSelected != null)
                AlertLookUp(itemToLook.itemName + "\n\n" + itemToLook.GetDescription());
            else
                AlertLookUp("kein Item ausgewählt");
        }

        //Fill and open ui
        public void OpenOverlay(ItemType itemType, GearType gearType = GearType.none)
        {
            menuOpendForItem = itemType;
            menuOpenedForGear = gearType;

            highlightFirstEquipped = false;

            overlay.SetActive(true);

            switch (itemType)
            {
                case ItemType.potion:
                    FillPotion();
                    break;
                case ItemType.gear:
                    switch (gearType)
                    {
                        case GearType.armor:
                            FillArmor();
                            break;
                        case GearType.weapon:
                            FillWeapon();
                            break;
                        case GearType.amulet:
                            FillAmulet();
                            break;
                        case GearType.none:
                            Debug.LogError("No menue for none");
                            break;
                    }
                    break;
                case ItemType.quest:
                    FillQuestItem();
                    break;
                case ItemType.misc:
                    Debug.LogError("No menue for misc");
                    break;
                case ItemType.none:
                    Debug.LogError("No menue for none");
                    break;
            }
        }
        public void FillArmor()
        {
            if (Player.Instance.Inventory.GearInventory.ArmorsCollected.Count == 0)
                return;


            List<SO_armor> sortedList = Player.Instance.Inventory.GearInventory.ArmorsCollected.OrderBy(o => o.itemName).ToList();
            foreach (SO_armor armor in sortedList)
            {
                if (armor != null)
                    CreateObjectItem(armor, (armor == Player.Instance.Inventory.GearInventory.ArmorEquiped));
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
                    CreateObjectItem(armor, (armor == Player.Instance.Inventory.GearInventory.WeaponEquipped));


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
                    CreateObjectItem(armor, (armor == Player.Instance.Inventory.GearInventory.AmuletEquipped));
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

        public void CreateObjectItem(SO_item newItem, bool select = false)
        {
            GameObject contentItem = Instantiate(uiObjectPrefab, contentContainer, false);

            //Setup
            contentItem.GetComponent<UIObjectItem>().SetupObjecItem(newItem, newItem.itemImage);

            if (select && highlightFirstEquipped == false)
            {
                highlightFirstEquipped = true;
                contentItem.GetComponent<UIObjectItem>().SelectElement();
            }

            //Highlight
            //if (contentContainer.childCount == 1)
            //    contentItem.GetComponent<UIObjectItem>().SetHighlight(true);
        }

        public void closeOverlay()
        {
            overlay.SetActive(false);

            while (contentContainer.childCount > 0)
            {
                DestroyImmediate(contentContainer.GetChild(0).gameObject);
            }
        }
        #endregion
    }

}
