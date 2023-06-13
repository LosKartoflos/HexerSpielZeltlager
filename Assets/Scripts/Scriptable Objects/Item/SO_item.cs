
using UnityEngine;
using UnityEngine.UI;


namespace Hexerspiel.Items
{
    public abstract class SO_item : ScriptableObject
    {
        public string itemName;
        public Image itemImage;
        public int valueBuy;
        public int valueSell;
    }
}