using Hexerspiel.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hexerspiel.Character
{
    public class Inventory : MonoBehaviour
    {


        #region Variables
        private static Inventory instance;

        [SerializeField]
        private BasicInventory basicInventory = new BasicInventory();

        [SerializeField]
        private GearInventory gearInventory = new GearInventory();

        [SerializeField]
        private PotionInventory potionInventory = new PotionInventory();

        [SerializeField]
        private QuestItemInventory questItemInventory = new QuestItemInventory();


        #endregion

        #region Accessors
        public static Inventory Instance { get => instance;}
        public BasicInventory BasicInventory { get => basicInventory; set => basicInventory = value; }
        public GearInventory GearInventory { get => gearInventory; set => gearInventory = value; }
        public PotionInventory PotionInventory { get => potionInventory; set => potionInventory = value; }
        public QuestItemInventory QuestItemInventory { get => questItemInventory; set => questItemInventory = value; }
        #endregion

        #region LifeCycle
        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // In first scene, make us the singleton.
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
                Destroy(gameObject);
        }
        #endregion

        #region Functiosn
        #endregion






    }
}