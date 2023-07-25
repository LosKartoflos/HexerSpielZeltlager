using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Items
{

    [CreateAssetMenu(fileName = "QuestItem_", menuName = "Hexer_ScriptableObjects/Items/Questitem")]
    public class SO_questItem : SO_item
    {
        protected override ItemType itemType => ItemType.quest;

        public override ItemType Type { get { return ItemType.quest; } }

        public override string GetDescription()
        {
            return "Du kannst dammit eine QuestLösen.";
        }
        public override string GetDescriptionShort()
        {
            return "Questlösung";
        }
    }
}