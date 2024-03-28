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
        [Header("Misc")]
        [SerializeField]
        TextMeshProUGUI label_gold;
        [SerializeField]
        TextMeshProUGUI label_herbs, label_meat, label_magicEssence;

        [SerializeField]
        Button bt_eatHerb, bt_eatMeat, bt_useMagicEssence;




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
        ItemEquipElement armorEquip, weaponEquip, amuletEquip, potionEquip, questItemEquip, shopItem;

        [SerializeField]
        GameObject overlay;

        [Header("Footer")]
        [SerializeField]
        Button bt_look, bt_sell, bt_drop, bt_buy, bt_equip, bt_use;

        [SerializeField]
        Button bt_sellHerb, bt_sellMeat, bt_sellEssence;

        private UIObjectItem currentitemSelected;

        private ItemType menuOpendForItem = ItemType.none;
        private GearType menuOpenedForGear = GearType.none;

        private ItemType itemTypeselectedByObjectItem = ItemType.none;
        private GearType gearTypeSelectedByObjectItem = GearType.none;

        private UiInventoryMode inventoryMode = UiInventoryMode.overview;

        bool highlightFirstEquipped = false;
        public static bool atShop = false;
        public static List<SO_item> shopItemList = new List<SO_item>();
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle


        private void OnEnable()
        {
            UIObjectItem.RecieveSelectedUIObjectItem += UIObjectItemSelected;

        }

        private void OnDisable()
        {
            UIObjectItem.RecieveSelectedUIObjectItem -= UIObjectItemSelected;
        }
        private void Start()
        {
            closeOverlay();


            shopItem.gameObject.SetActive(atShop);

            //assign Equip button
            bt_close.onClick.AddListener(closeOverlay);
            armorEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.gear, GearType.armor); });
            weaponEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.gear, GearType.weapon); });
            amuletEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.gear, GearType.amulet); });
            potionEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.potion); });
            questItemEquip.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.quest); });
            shopItem.Button.onClick.AddListener(delegate { OpenOverlay(ItemType.none); });

            //style equip buttons
            UpdateAppreance();
            UpdateMisc();

            //add listener to normal buttons

            bt_look.onClick.AddListener(delegate { lookAtItem(currentitemSelected.ItemAttached); });
            bt_drop.onClick.AddListener(delegate { DropItem(currentitemSelected.ItemAttached); });
            bt_sell.onClick.AddListener(delegate { SellItem(currentitemSelected.ItemAttached); });
            bt_buy.onClick.AddListener(delegate { BuyItem(currentitemSelected.ItemAttached); });
            bt_equip.onClick.AddListener(delegate { EquipItem(currentitemSelected.ItemAttached); });
            bt_use.onClick.AddListener(delegate { UseItem(currentitemSelected.ItemAttached); });

            bt_eatHerb.onClick.AddListener(EatHerbs);
            bt_eatMeat.onClick.AddListener(EatMeat);
            bt_useMagicEssence.onClick.AddListener(EatMagicEssence);
        }
        #endregion

        #region Functions
        //UI setup
        public void GearSetup()
        {
            bt_look.gameObject.SetActive(true);
            bt_sell.gameObject.SetActive(atShop);
            bt_drop.gameObject.SetActive(true);
            bt_buy.gameObject.SetActive(false);
            bt_equip.gameObject.SetActive(true);
            bt_use.gameObject.SetActive(false);
        }

        public void PotionSetup()
        {
            bt_look.gameObject.SetActive(true);
            bt_sell.gameObject.SetActive(atShop);
            bt_drop.gameObject.SetActive(true);
            bt_buy.gameObject.SetActive(false);
            bt_equip.gameObject.SetActive(false);
            bt_use.gameObject.SetActive(true);
        }

        public void QuestItemSetup()
        {
            bt_look.gameObject.SetActive(true);
            bt_sell.gameObject.SetActive(false);
            bt_drop.gameObject.SetActive(false);
            bt_buy.gameObject.SetActive(false);
            bt_equip.gameObject.SetActive(false);
            bt_use.gameObject.SetActive(false);
        }

        public void ShopSetup()
        {
            bt_look.gameObject.SetActive(true);
            bt_sell.gameObject.SetActive(false);
            bt_drop.gameObject.SetActive(false);
            bt_buy.gameObject.SetActive(true);
            bt_equip.gameObject.SetActive(false);
            bt_use.gameObject.SetActive(false);
        }

        void UpdateAppreance()
        {
            if (Player.Instance.Inventory.GearInventory.AmuletEquipped != null)
                amuletEquip.ChangeAppreance(Player.Instance.Inventory.GearInventory.AmuletEquipped.itemName, Player.Instance.Inventory.GearInventory.AmuletEquipped.GetDescriptionShort(), Player.Instance.Inventory.GearInventory.AmuletEquipped.itemImage);
            else
                amuletEquip.ChangeAppreance(null, "Lege ein Amulett an!");

            if (Player.Instance.Inventory.GearInventory.ArmorEquiped != null)
                armorEquip.ChangeAppreance(Player.Instance.Inventory.GearInventory.ArmorEquiped.itemName, Player.Instance.Inventory.GearInventory.ArmorEquiped.GetDescriptionShort(), Player.Instance.Inventory.GearInventory.ArmorEquiped.itemImage);
            else
                armorEquip.ChangeAppreance(null, "Rüste dich!");

            if (Player.Instance.Inventory.GearInventory.WeaponEquipped != null)
                weaponEquip.ChangeAppreance(Player.Instance.Inventory.GearInventory.WeaponEquipped.itemName, Player.Instance.Inventory.GearInventory.WeaponEquipped.GetDescriptionShort(), Player.Instance.Inventory.GearInventory.WeaponEquipped.itemImage);
            else
                weaponEquip.ChangeAppreance(null, "Schnapp dir eine Waffe!");

           

            potionEquip.ChangeAppreance(null, null);
            questItemEquip.ChangeAppreance(null, null);
            shopItem.ChangeAppreance(null, null);
        }

       public void UpdateOverlayHeader()
        {
            string header = "Dein Geld: " + Player.Instance.Inventory.BasicInventory.Amount.gold.ToString() + " Gold";
            
            if(currentitemSelected != null)
            {
                header += "\nKauf: " + currentitemSelected.ItemAttached.valueBuy + "Gold | Verkauf: " + currentitemSelected.ItemAttached.valueSell + " Gold";
            }
            label_Header.text = header;
        }

        public void UpdateMisc()
        {
            label_gold.text = Player.Instance.Inventory.BasicInventory.Amount.gold.ToString();
            label_herbs.text = Player.Instance.Inventory.BasicInventory.Amount.miscItems.herbs.ToString();
            label_meat.text = Player.Instance.Inventory.BasicInventory.Amount.miscItems.meat.ToString();
            label_magicEssence.text = Player.Instance.Inventory.BasicInventory.Amount.miscItems.magicEssence.ToString();
        }
        //Actions
        public void UIObjectItemSelected(UIObjectItem newItem)
        {
            currentitemSelected = newItem;
            itemTypeselectedByObjectItem = newItem.ItemAttached.ItemType;

            if (newItem.ItemAttached.ItemType == ItemType.gear)
            {
                gearTypeSelectedByObjectItem = ((SO_gear)newItem.ItemAttached).GearType;
            }
            else
            {
                gearTypeSelectedByObjectItem = GearType.none;
            }

            UpdateOverlayHeader();
        }



        public void DropItem(SO_item itemToProcess)
        {
            switch (menuOpendForItem)
            {
                case ItemType.potion:
                    Player.Instance.Inventory.PotionInventory.DropPotion((SO_potion)itemToProcess);
                    break;
                case ItemType.gear:
                    switch (menuOpenedForGear)
                    {
                        case GearType.armor:
                            Player.Instance.Inventory.GearInventory.DropGear((SO_armor)itemToProcess);
                            break;
                        case GearType.weapon:
                            Player.Instance.Inventory.GearInventory.DropGear((SO_weapon)itemToProcess);
                            break;
                        case GearType.amulet:
                            Player.Instance.Inventory.GearInventory.DropGear((SO_amulet)itemToProcess);
                            break;
                    }
                    break;
                case ItemType.quest:
                    Player.Instance.Inventory.QuestItemInventory.DropQuestItem((SO_questItem)itemToProcess);
                    break;

            }

            currentitemSelected.DeleteItem();
        }

        public void BuyItem(SO_item itemToProcess)
        {

            switch (itemTypeselectedByObjectItem)
            {
                case ItemType.potion:
                    Player.Instance.Inventory.PotionInventory.BuyPotion((SO_potion)itemToProcess);
                    break;
                case ItemType.gear:
                    switch (gearTypeSelectedByObjectItem)
                    {
                        case GearType.armor:
                            Player.Instance.Inventory.GearInventory.BuyGear((SO_armor)itemToProcess);
                            break;
                        case GearType.weapon:
                            Player.Instance.Inventory.GearInventory.BuyGear((SO_weapon)itemToProcess);
                            break;
                        case GearType.amulet:
                            Player.Instance.Inventory.GearInventory.BuyGear((SO_amulet)itemToProcess);
                            break;
                    }
                    break;
                case ItemType.quest:
                    Player.Instance.Inventory.QuestItemInventory.BuyQuestItem((SO_questItem)itemToProcess);

                    break;

            }
            //AlertLookUp("Du hast für " + itemToProcess.valueBuy + " Gold " + itemToProcess.itemName + "gekauft.");
            shopItemList.Remove(itemToProcess);
            UpdateOverlayHeader();
            currentitemSelected.DeleteItem();
        }

        public void SellItem(SO_item itemToProcess)
        {
            switch (menuOpendForItem)
            {
                case ItemType.potion:
                    Player.Instance.Inventory.PotionInventory.SellPotion((SO_potion)itemToProcess);
                    currentitemSelected.DeleteItem();
                    break;
                case ItemType.gear:
                    switch (menuOpenedForGear)
                    {
                        case GearType.armor:
                            Player.Instance.Inventory.GearInventory.SellGear((SO_armor)itemToProcess);
                            break;
                        case GearType.weapon:
                            Player.Instance.Inventory.GearInventory.SellGear((SO_weapon)itemToProcess);
                            break;
                        case GearType.amulet:
                            Player.Instance.Inventory.GearInventory.SellGear((SO_amulet)itemToProcess);
                            break;

                    }
                    currentitemSelected.DeleteItem();
                    break;

            }
            UpdateOverlayHeader();
        }

        public void EquipItem(SO_item itemToProcess)
        {
            switch (menuOpendForItem)
            {

                case ItemType.gear:
                    switch (menuOpenedForGear)
                    {
                        case GearType.armor:
                            Player.Instance.Inventory.GearInventory.EquipGear((SO_armor)itemToProcess);
                            break;
                        case GearType.weapon:
                            Player.Instance.Inventory.GearInventory.EquipGear((SO_weapon)itemToProcess);
                            break;
                        case GearType.amulet:
                            Player.Instance.Inventory.GearInventory.EquipGear((SO_amulet)itemToProcess);
                            break;
                    }
                    break;

            }
        }
        public void UseItem(SO_item itemToProcess)
        {
            switch (menuOpendForItem)
            {
                case ItemType.potion:
                    Player.Instance.Inventory.PotionInventory.UsePotion((SO_potion)itemToProcess);
                    currentitemSelected.DeleteItem();
                    break;


            }


        }

        //bt effects


        public void lookAtItem(SO_item itemToLook)
        {
            if (currentitemSelected != null)
            {
                if (itemToLook.Type != ItemType.potion)
                    AlertLookUp(itemToLook.itemName + "\n\n" + itemToLook.GetDescription());
                else
                    AlertLookUp(itemToLook.itemName + "\n\n" + ((SO_potion)itemToLook).GetDescription());
            }
               
            else
                AlertLookUp("kein Item ausgewählt");
        }

        //Fill and open ui
        public void OpenOverlay(ItemType itemType, GearType gearType = GearType.none)
        {
            UpdateOverlayHeader();

            menuOpendForItem = itemType;
            menuOpenedForGear = gearType;

            highlightFirstEquipped = false;

            overlay.SetActive(true);

            switch (itemType)
            {
                case ItemType.potion:
                    FillPotion();
                    PotionSetup();
                    break;
                case ItemType.gear:
                    GearSetup();
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
                    QuestItemSetup();
                    FillQuestItem();
                    break;
                case ItemType.misc:
                    Debug.LogError("No menue for misc");
                    break;
                case ItemType.none:
                    ShopSetup();
                    FillShoptItem();
                    break;
            }
        }

        private void FillShoptItem()
        {
            if (shopItemList.Count == 0)
            {
                return;
            }
            List<SO_item> sortedList = shopItemList.OrderBy(o => (int)o.ItemType).ToList();

            foreach (SO_item item in sortedList)
            {
                if (item != null)
                    CreateObjectItem(item);
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
            UpdateAppreance();
            UpdateMisc();
            overlay.SetActive(false);

            while (contentContainer.childCount > 0)
            {
                DestroyImmediate(contentContainer.GetChild(0).gameObject);
            }


        }

        public void EatHerbs()
        {
            if (Player.Instance.Inventory.BasicInventory.Amount.miscItems.herbs > 0 && Player.Instance.PlayerValues.GetLife() != Player.Instance.PlayerValues.basicStatsValue.healthMax)
            {
                UpdateMisc();
                Player.Instance.Inventory.BasicInventory.ChangeHerbs(-1);
                Player.Instance.PlayerValues.AddLife(1);
                Player.Instance.SetLifeForPlayerOutsideFight(Player.Instance.PlayerValues.GetLife() + 1);
            }
        }

        public void EatMeat()
        {
            if (Player.Instance.Inventory.BasicInventory.Amount.miscItems.meat > 0 && Player.Instance.PlayerValues.GetLife() != Player.Instance.PlayerValues.basicStatsValue.healthMax)
            {
                
                Player.Instance.Inventory.BasicInventory.ChangeMeat(-1);
                UpdateMisc();
                Player.Instance.SetLifeForPlayerOutsideFight(Player.Instance.PlayerValues.GetLife() + 1);
            }
        }

        public void EatMagicEssence()
        {
            if (Player.Instance.Inventory.BasicInventory.Amount.miscItems.meat > 0 && Player.Instance.PlayerValues.GetMana() != Player.Instance.PlayerValues.playerStats.manaMax)
            {
                
                Player.Instance.Inventory.BasicInventory.ChangeMagicessence(-1);
                UpdateMisc();
                Player.Instance.SetManaForPlayerOutsideFight(Player.Instance.PlayerValues.GetMana() + 1);
            }
        }
        #endregion
    }

}
