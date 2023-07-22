using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Hexerspiel.Quests.SO_step_mulitpleChoiceAttribute;

namespace Hexerspiel.UI
{
    public class MultipleChoicePopUp : MonoBehaviour
    {
        #region Variables
        public static event Action<RighAnswer, string> ANSWERGIVEN = delegate { };


        private string revieverId;

        [SerializeField]
        TextMeshProUGUI label;

        [SerializeField]
        Button bt_a, bt_b, bt_c, bt_d;
        [SerializeField]
        TextMeshProUGUI label_a, label_b, label_c, label_d;
        #endregion

        #region Accessors
        #endregion

        #region LifeCycle
        private void Awake()
        {
            bt_a.onClick.AddListener(delegate { ANSWERGIVEN(RighAnswer.a, revieverId); Destroy(gameObject); });
            bt_b.onClick.AddListener(delegate { ANSWERGIVEN(RighAnswer.b, revieverId); Destroy(gameObject); });
            bt_c.onClick.AddListener(delegate { ANSWERGIVEN(RighAnswer.c, revieverId); Destroy(gameObject); });
            bt_d.onClick.AddListener(delegate { ANSWERGIVEN(RighAnswer.d, revieverId); Destroy(gameObject); });
        }
        #endregion

        #region Functions
        public void FillLabel(string text)
        {
            label.text = text;
        }

        public void IntiatePopUp(string text, string _recieverId, List<AttributAnswer> answers)
        {
            List<string> answersString = new List<string>();

            foreach (AttributAnswer aa in answers)
            {
                answersString.Add(aa.Text + "\n(" + aa.attribut.ToString() + ": " + aa.level.ToString() + ")" );
            }

            IntiatePopUp(text, _recieverId, answersString);
        }

        public void IntiatePopUp(string text, string _recieverId, List<string> answers)
        {
            bt_a.interactable = false;
            bt_b.interactable = false;
            bt_c.interactable = false;
            bt_d.interactable = false;

            if (answers.Count > 0)
            {
                bt_a.interactable = true;
                label_a.text = answers[0];
            }
            else
            {
                label_a.text = "~";
            }
            if (answers.Count > 1)
            {
                bt_b.interactable = true;
                label_b.text = answers[1];
            }
            else
            {
                label_b.text = "~";
            }

            if (answers.Count > 2)
            {
                bt_c.interactable = true;
                label_c.text = answers[2];
            }
            else
            {
                label_c.text = "~";
            }

            if (answers.Count > 3)
            {
                bt_d.interactable = true;
                label_d.text = answers[3];
            }
            else
            {
                label_d.text = "~";
            }

            FillLabel(text);
            this.revieverId = _recieverId;
        }
        #endregion
    }
}
