using Hexerspiel.Character;
using Hexerspiel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class UIObjectItem : MonoBehaviour
    {

        #region Variables
        //disable other items
        public static event Action<int> ItemSelected = delegate { };
        //for inventory to check
        public static event Action<UIObjectItem> RecieveSelectedUIObjectItem = delegate { };
        public static event Action<ItemType> TypeSelected = delegate { };

        [SerializeField]
        SO_item itemAttached;

        [SerializeField]
        TextMeshProUGUI label_item;

        [SerializeField]
        Image image;

        [SerializeField]
        Button button, close;

        [SerializeField]
        GameObject highlight;

        bool selected = false;

        public SO_item ItemAttached { get => itemAttached;  }

        public UIObjectItem(SO_item itemAttached, Image image)
        {
            this.itemAttached = itemAttached;
            this.image = image;
        }

        public UIObjectItem(SO_item itemAttached, Sprite image)
        {
            this.itemAttached = itemAttached;
            this.image.sprite = image;
        }

        public UIObjectItem(SO_item itemAttached, string imageName)
        {
            this.itemAttached = itemAttached;
            this.image.sprite = image.sprite = Resources.Load<Sprite>("Images/Items/" + imageName);
        }


        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {
            button.onClick.AddListener(SelectElement);
        }

        private void OnEnable()
        {
            UIObjectItem.ItemSelected += RecieveItemSelected;
        }

        private void OnDisable()
        {
            UIObjectItem.ItemSelected -= RecieveItemSelected;
        }
        #endregion

        #region Functions
        public void SelectElement()
        {
            if (selected)
                return;

            ItemSelected(gameObject.GetInstanceID());
            RecieveSelectedUIObjectItem(this);

            InventorySceneManager.currentObjectItem = this;

            TypeSelected(itemAttached.ItemType);

            SetHighlight(true);

            selected = true;
        }

        public void SetHighlight(bool set)
        {
            highlight.SetActive(set);
        }

        public void RecieveItemSelected(int otherId)
        {
            if (otherId != gameObject.GetInstanceID() && selected)
            {
                selected = false;
                SetHighlight(false);
            }
        }

        public void SetupObjecItem(SO_item itemAttached, string imageName)
        {
            SetHighlight(false);
            this.itemAttached = itemAttached;
            if (imageName != "")
                this.image.sprite = image.sprite = Resources.Load<Sprite>("Images/Items/" + imageName);
            else
                this.image.sprite = image.sprite = Resources.Load<Sprite>("Images/Items/defaultIcons/defaultItem");

            label_item.text = itemAttached.itemName;

        }

        public void DeleteItem()
        {
            Destroy(gameObject);
        }



        #endregion
    }

}