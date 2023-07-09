using System;
using UnityEngine;

namespace DigitsNFCToolkit.Samples
{
    public class NFCReadControl : MonoBehaviour
    {
        //[SerializeField]
        //private ReadScreenView view;

        //      [SerializeField]
        //private MessageScreenView messageScreenView;

        public static event Action tagReadEvent = delegate { };

        public void Start()
        {
#if (!UNITY_EDITOR)
			NativeNFCManager.AddNFCTagDetectedListener(OnNFCTagDetected);
			NativeNFCManager.AddNDEFReadFinishedListener(OnNDEFReadFinished);
			//NativeNFCManager.AddNDEFMakeReadonlyFinishedListener(OnNDEFMakeReadonlyFinished);
			Debug.Log("NFC Tag Info Read Supported: " + NativeNFCManager.IsNFCTagInfoReadSupported());
			Debug.Log("NDEF Read Supported: " + NativeNFCManager.IsNDEFReadSupported());
			Debug.Log("NDEF Write Supported: " + NativeNFCManager.IsNDEFWriteSupported());
			Debug.Log("NDEF Push Supported: " + NativeNFCManager.IsNDEFPushSupported());
			Debug.Log("NFC Enabled: " + NativeNFCManager.IsNFCEnabled());
			Debug.Log("NDEF Push Enabled: " + NativeNFCManager.IsNDEFPushEnabled());
#endif
        }

        private void OnEnable()
        {
#if (!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Enable();
#endif
            //view.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
#if (!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Disable();
#endif
            //if(view != null)
            //{
            //	view.gameObject.SetActive(false);
            //}
        }

        public void OnStartNFCReadClick()
        {
#if (!UNITY_EDITOR)
			NativeNFCManager.ResetOnTimeout = true;
			StartReading();
#endif
        }

        public void OnNFCTagDetected(NFCTag tag)
        {
            NFCMessenger.Instance.UpdateTagInfo(tag);
        }

        public void OnNDEFReadFinished(NDEFReadResult result)
        {
            string readResultString = string.Empty;
            if (result.Success)
            {
                readResultString = string.Format("NDEF Message was read successfully from tag {0}", result.TagID);
                NFCMessenger.Instance.UpdateNDEFMessage(result.Message);

            }
            else
            {
                readResultString = string.Format("Failed to read NDEF Message from tag {0}\nError: {1}", result.TagID, result.Error);
            }
            Debug.Log(readResultString);
        }

        public void StartReading()
        {
            NativeNFCManager.Enable();
        }

        public void StopReading()
        {

            this.enabled = false;
            gameObject.SetActive(false);
        }


    }
}
