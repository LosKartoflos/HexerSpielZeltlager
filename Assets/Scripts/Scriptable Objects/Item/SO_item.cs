
using UnityEngine;
using UnityEngine.UI;


namespace Hexerspiel.Items
{
    public abstract class SO_item : ScriptableObject
    {
        public string itemName;
        public string itemImage;
        public int valueBuy;
        public int valueSell;

        protected virtual ItemType itemType => ItemType.none;
        public ItemType ItemType { get => itemType; }

        public virtual ItemType Type { get { return ItemType.none; } }

        public virtual string GetDescription()
        {
            return "It's a item";
        }
        public virtual string GetDescriptionShort()
        {
            return "It's a item";
        }

        public string ValueText()
        {
            return "Kaufwert: " + valueBuy + " | Verkaufswert: " + valueSell;
        }
    }
}