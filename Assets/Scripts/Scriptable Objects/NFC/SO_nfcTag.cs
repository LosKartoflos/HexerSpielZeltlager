using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class SO_nfcTag: ScriptableObject
{
    [Serializable]
    public struct TagInfos
    {
        public string id;
        public string name;
        public string image;
    }

    public TagInfos nfcTagInfos;


}
