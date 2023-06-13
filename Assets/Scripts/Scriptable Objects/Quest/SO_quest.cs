using Hexerspiel.Items;
using Hexerspiel.nfcTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_quest : SO_nfcTag
{
    //only needs a NFCTag if its a start quest;
    public string questText;
    public string resolvedText;

    //reward
    public int gold;
    public int xp;
    //Drops
    public int herbs, meat, magicEssence;
    public List<SO_gear> dropedGear;
    public List<SO_questItem> dropedQuestItems;



    bool lastQuest = false;
    public SO_quest nextQuestItem;

    //Condtions (if not needed leave empty
    [Header("Conditions")]
    public SO_spots spotToGO;
    public SO_npc personToInteract;
}
