using Hexerspiel.Character;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Hexer_ScriptableObjects/Items/Potion")]
    public class SO_potion : SO_item
    {
        protected override ItemType itemType => ItemType.potion;
        public PotionStats potionStats;

        public override ItemType Type { get { return ItemType.potion; } }

        public virtual string GetDescription()
        {
            return string.Format("F�r dich:\nExtra W�rfel: {0}\nW�rfelmodifikator:{1}\n\nF�r Gegner:\nW�rfel Abzug: {2}\nNegativer W�rfelmod.:{3}\n\nLebenregen: {4} | Manaregen: {5}\n\n{6}", potionStats.diceManipulation.addDice, potionStats.diceManipulation.addablePoints, potionStats.diceManipulation.substractDiceFromEnemy, potionStats.diceManipulation.subtractablePointsFromEnemy, potionStats.addLife, potionStats.addMana, ValueText());
        }
        public virtual string GetDescriptionShort()
        {
            return string.Format("P: +W: {0} +WM:{1} | E: -W: {2} -WM:{3} | L: {4} M:{5}", potionStats.diceManipulation.addDice, potionStats.diceManipulation.addablePoints, potionStats.diceManipulation.substractDiceFromEnemy, potionStats.diceManipulation.subtractablePointsFromEnemy, potionStats.addLife, potionStats.addMana);
        }
    }
}