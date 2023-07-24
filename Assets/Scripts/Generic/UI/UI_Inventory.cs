using Hexerspiel.Character;
using Hexerspiel.Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{

    public class UI_Inventory : MonoBehaviour
    {

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


        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        #endregion

        #region Functions
        public void FillArmor()
        {

        }

        public void CreateObjectItem(SO_item newItem)
        {
            GameObject contentItem = Instantiate(uiObjectPrefab, contentContainer, false);

            //Setup
            contentItem.GetComponent<UIObjectItem>().SetupObjecItem(newItem, newItem.itemImage);

            //Highlight
           if (contentContainer.childCount == 1)
                contentItem.GetComponent<UIObjectItem>().SetHighlight(true);
        }
        #endregion
    }

}