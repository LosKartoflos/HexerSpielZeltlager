using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexerspiel.Quests
{
    public class SO_freeEntry : ScriptableObject
    {
        public string entry;

        public SO_freeEntry(string newEntry)
        {
            entry = newEntry;
        }
    }
}
