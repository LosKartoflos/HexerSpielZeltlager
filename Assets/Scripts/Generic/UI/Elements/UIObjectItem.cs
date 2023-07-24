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
        public static event Action<int> ItemSelected = delegate { };
        public static event Action<SO_item> RecieveSelectedItem = delegate { };

        [SerializeField]
        SO_item itemAttached;

        [SerializeField]
        TextMeshProUGUI label_item;

        [SerializeField]
        Image image;

        [SerializeField]
        Button button;

        [SerializeField]
        GameObject highlight;

        bool selected = false;

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
            this.image.sprite = image.sprite = Resources.Load<Sprite>("Images/Items/" + imageName );
        }


        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
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
            RecieveSelectedItem(itemAttached);

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
                SetHighlight(false);
            }
        }

        #endregion
    }

}