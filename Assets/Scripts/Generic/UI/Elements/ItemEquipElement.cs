using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class ItemEquipElement : MonoBehaviour
    {

        #region Variables
        [SerializeField]
        Image itemImage;
        [SerializeField]
        TextMeshProUGUI label_itemName, label_description;

        [SerializeField]
        Button button;

        [SerializeField]
        string defaultName = "Item";
        [SerializeField]
        string defaultdescription = "Beschreibung";
        [SerializeField]
        string defaultImage = "Images/Items/defaultIcons/defaultItem";


        #endregion

        #region Accessors
        public Button Button { get => button; }
        #endregion

        #region LifeCycle

        #endregion

        #region Functions
        public void ChangeAppreance(string itemName, string itemDescription, string imagePath = null)
        {
            if (itemName == null || itemName == "")
            {
                label_itemName.text = defaultName;
            }
            else
            {
                label_itemName.text = itemName;
            }

            if (itemDescription == null || itemDescription == "")
            {
                label_description.text = defaultdescription;
            }
            else
            {
                label_description.text = itemDescription;
            }

            if (imagePath == null || imagePath == "")
            {
                itemImage.sprite = Resources.Load<Sprite>(defaultImage);
            }
            else
            {
                itemImage.sprite = Resources.Load<Sprite>("Images/Items/"+imagePath);
            }


        }

        #endregion
    }

}