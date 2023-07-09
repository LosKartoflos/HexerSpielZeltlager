using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexerspiel.UI
{
    public class ScaleCanvasToFitNotch : MonoBehaviour
    {
        //[SerializeField] private RectTransform canvasRect;
        //[SerializeField] private RectTransform emptyRect;
        //[SerializeField] private float notchOffset = 100f;

        //private void Start()
        //{
        //    // Calculate the safe area of the screen
        //    Rect safeArea = Screen.safeArea;

        //    // Calculate the offset for the notch area
        //    Vector2 notchOffsetVector = new Vector2(0f, notchOffset);

        //    // Calculate the new position and size for the empty GameObject
        //    Vector2 newPosition = new Vector2(safeArea.xMin + notchOffsetVector.x, safeArea.yMin + notchOffsetVector.y);
        //    Vector2 newSize = new Vector2(safeArea.width - notchOffsetVector.x * 2f, safeArea.height - notchOffsetVector.y * 2f);

        //    // Apply the new position and size to the empty GameObject
        //    emptyRect.anchoredPosition = newPosition;
        //    emptyRect.sizeDelta = newSize;

        //    // Scale the canvas to fit the new size
        //    float scale = Mathf.Min(newSize.x / canvasRect.sizeDelta.x, newSize.y / canvasRect.sizeDelta.y);
        //    canvasRect.localScale = new Vector3(scale, scale, 1f);
        //}

        private RectTransform rectTrans = new RectTransform();
        private Rect safeArea;
        Vector2 anchorMin = Vector2.zero;
        Vector2 anchorMax = Vector2.zero;

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
            safeArea = Screen.safeArea;
            anchorMin = safeArea.position;
            anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTrans.anchorMin = anchorMin;
            rectTrans.anchorMax = anchorMax;

        }


    }
}