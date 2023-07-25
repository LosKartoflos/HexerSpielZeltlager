using Hexerspiel.Character;
using Hexerspiel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Items
{
    [CreateAssetMenu(fileName = "Amulet", menuName = "Hexer_ScriptableObjects/Items/Gear/Amulet")]
    public class SO_amulet : SO_gear
    {
        protected override GearType gearType => GearType.amulet;
        public Dice.Manipulation diceManipulation;

        public override string GetDescription()
        {
            return string.Format("F�r dich:\nExtra W�rfel: {0}\nW�rfelmodifikator:{1}\n\nF�r Gegner:\nW�rfel Abzug: {2}\nNegativer W�rfelmod.:{3}\n{4}\n{5}", diceManipulation.addDice, diceManipulation.addablePoints, diceManipulation.substractDiceFromEnemy, diceManipulation.subtractablePointsFromEnemy, GetAttributText(), ValueText());
        }

        public override string GetDescriptionShort()
        {
            return string.Format("P: +W: {0} +WM:{1} | E: -W: {2} -WM:{3}", diceManipulation.addDice, diceManipulation.addablePoints, diceManipulation.substractDiceFromEnemy, diceManipulation.subtractablePointsFromEnemy);
        }
    }
}
