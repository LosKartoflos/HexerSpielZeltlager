using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hexerspiel.UI
{
    public class Bar : MonoBehaviour
    {

        #region Variables
        [SerializeField]
        RectTransform bar;

        [SerializeField]
        TextMeshProUGUI textField;

        [SerializeField]
        float StartValue = 950;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Start()
        {

        }
        #endregion

        #region Functions

        public void SetBar(float noramlizedPercent = 1)
        {
            bar.sizeDelta = new Vector2(noramlizedPercent * 950, bar.sizeDelta.y); 
        }

        public void SetValues(float current, float max, string ending)
        {
            textField.text = current.ToString("0") + "/" + max.ToString("0") + ending;
            SetBar(current / max);
        }

        #endregion
    }
}
