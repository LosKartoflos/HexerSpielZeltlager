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
        #endregion
    }

}