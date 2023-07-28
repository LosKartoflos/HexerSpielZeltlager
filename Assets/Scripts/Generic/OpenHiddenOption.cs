using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.Generic
{
    public class OpenHiddenOption : MonoBehaviour
    {
        public GameObject objectToActivate;
        public float timeFrame = 10f;
        public int requiredPressCount = 5;

        private int pressCount = 0;
        private float startTime = 0f;

        private void Start()
        {
           // objectToActivate.SetActive(false);
        }

        public void OnButtonClick()
        {
            if (Time.time - startTime <= timeFrame)
            {
                pressCount++;
                if (pressCount >= requiredPressCount)
                {
                    objectToActivate.SetActive(true);
                }
            }
            else
            {
                pressCount = 1;
                startTime = Time.time;
            }
        }
    }

}