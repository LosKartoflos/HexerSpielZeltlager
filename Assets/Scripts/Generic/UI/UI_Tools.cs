using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI_Tools 
{
   public static void SetCanvasGroup(CanvasGroup cg,bool state)
    {
        if (state)
        {
            cg.interactable = true;
            cg.alpha = 1;
            cg.blocksRaycasts = true;
        }
        else if (!state)
        {
            cg.interactable = false;
            cg.alpha = 0;
            cg.blocksRaycasts = false;
        }
    }
}
