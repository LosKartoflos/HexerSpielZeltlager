using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Character
{
    public class InventoryCollection : MonoBehaviour
    {
        [SerializeField]
        private BasicInventory basicInventory = new BasicInventory();

        public BasicInventory BasicInventory { get => basicInventory; set => basicInventory = value; }

      
    }
}