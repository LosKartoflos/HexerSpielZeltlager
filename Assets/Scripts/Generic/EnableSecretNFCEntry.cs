using UnityEngine;
using UnityEngine.UI;

namespace Hexerspiel.Generic
{
    public class EnableSecretNFCEntry : MonoBehaviour
    {
        public Button button1; // Reference to the first UI button
        public Button button2; // Reference to the second UI button
        public float requiredPressTime = 5f; // The time required to hold both buttons (in seconds)

        private bool button1Pressed;
        private bool button2Pressed;
        private float buttonsPressedTime;

        private void Start()
        {
            // Add event listeners to the buttons
            button1.onClick.AddListener(Button1Pressed);
            button2.onClick.AddListener(Button2Pressed);
        }

        private void Update()
        {
            // If both buttons were pressed within the required time
            if (button1Pressed && button2Pressed && Time.time - buttonsPressedTime >= requiredPressTime)
            {
                Debug.Log("Both buttons were pressed for 5 seconds at the same time.");
                UI_NFCScanScene.Instance.inputFieldDebug.gameObject.SetActive(!UI_NFCScanScene.Instance.inputFieldDebug.IsActive());
                // Reset the state
                button1Pressed = false;
                button2Pressed = false;
            }
        }

        private void Button1Pressed()
        {
            button1Pressed = true;
            CheckIfBothButtonsPressed();
        }

        private void Button2Pressed()
        {
            button2Pressed = true;
            CheckIfBothButtonsPressed();
        }

        private void CheckIfBothButtonsPressed()
        {
            // If both buttons were pressed, record the time
            if (button1Pressed && button2Pressed)
            {
                buttonsPressedTime = Time.time;
            }
        }

        

       
    }

}