using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.Fight
{
    public class Spells : MonoBehaviour
    {
        [SerializeField]
        private Button bt_heal, bt_damage, bt_close;

        public static Action<String> AlertSpell = delegate { };

        [SerializeField]
        private TextMeshProUGUI label_heal, label_damage;
        public void Start()
        {
            bt_close.onClick.AddListener(Close);
            bt_damage.onClick.AddListener(Damage);
            bt_heal.onClick.AddListener(Heal);

            int manaCost = 11 - Fight.player.playerAttributesBasic.charisma;
            if (manaCost < 1)
                manaCost = 1;

            label_heal.text = string.Format("Für {0} Mana {1} heilen", manaCost, Fight.player.playerAttributesBasic.mind * 2);
            label_damage.text = string.Format("Für {0} Mana {1} schaden Machen", manaCost, Fight.player.playerAttributesBasic.mind * 2);
        }

        public void Heal()
        {
            int manaCost = 11 - Fight.player.playerAttributesBasic.charisma;
            if (manaCost < 1)
                manaCost = 1;

            if (manaCost > FightManager.Instance.manaPlayer)
            {
                AlertSpell("Du hast nicht genug mana: " + manaCost.ToString());
                return;
            }

            FightManager.Instance.manaPlayer -= manaCost;

            FightManager.Instance.lifePlayer += Fight.player.playerAttributesBasic.mind * 2;
            if (FightManager.Instance.lifePlayer > FightManager.Instance.lifePlayeMax)
                FightManager.Instance.lifePlayer = FightManager.Instance.lifePlayeMax;

            FightManager.spellAvailable = false;

            FightManager.Instance.FillPlayerInfo();

            //manaPlayer += potionStats.addMana;
            //if (manaPlayer > manaPlayerMax)
            //    manaPlayer = manaPlayerMax;

            gameObject.SetActive(false);
        }



        public void Damage()
        {


            int manaCost = 11 - Fight.player.playerAttributesBasic.charisma;
            if (manaCost < 1)
                manaCost = 1;

            if (manaCost > FightManager.Instance.manaPlayer)
            {
                AlertSpell("Du hast nicht genug mana: " + manaCost.ToString());
                return;
            }

            FightManager.Instance.manaPlayer -= manaCost;

            FightManager.Instance.lifeEnemy -= Fight.player.playerAttributesBasic.mind * 2;
            FightManager.Instance.FillPlayerInfo();
            FightManager.Instance.FillEnemyInfo();


            FightManager.spellAvailable = false;

            gameObject.SetActive(false);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }

}