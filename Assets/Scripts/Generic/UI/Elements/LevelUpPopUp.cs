using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.UI
{
    public class LevelUpPopUp : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        TextMeshProUGUI label, lb_body, lb_mind, lb_charisma;

        [SerializeField]
        Button bt_body, bt_mind, bt_charisma;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Awake()
        {
            bt_body.onClick.AddListener(delegate { Player.Instance.AddBody(); Destroy(gameObject); });
            bt_mind.onClick.AddListener(delegate { Player.Instance.AddMind(); Destroy(gameObject); });
            bt_charisma.onClick.AddListener(delegate { Player.Instance.AddCharisma(); Destroy(gameObject); });
        }
        #endregion

        #region Functions
        public void FillLabel()
        {
            label.text = "Du bist jetzt Level: " + Player.Instance.PlayerValues.playerStats.level + "!\n\n Steigere ein Attribut:";

            lb_body.text = "Körper (" + Player.Instance.PlayerValues.playerAttributesBasic.body.ToString()+")";
            lb_mind.text ="Geist (" + Player.Instance.PlayerValues.playerAttributesBasic.mind.ToString() + ")";
            lb_charisma.text = "Charisma ("+ Player.Instance.PlayerValues.playerAttributesBasic.charisma.ToString() + ")";
        }
        #endregion
    }
}