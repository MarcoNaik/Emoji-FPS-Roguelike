using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemGrabbedUI : MonoBehaviour
    {
        private TextMeshProUGUI text;
    
        public static ItemGrabbedUI Instance { get; protected set; }
 
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
    
            text = GetComponent<TextMeshProUGUI>();
        }

        public void StartGrabCoroutine(String text)
        {
            StartCoroutine(GrabCoroutine(text));
        }

        private IEnumerator GrabCoroutine(String itemText)
        {
            float t = 0;
            Color32 startColor = new Color32(255, 255, 255, 255);
            Color32 endColor = new Color32(255, 255, 255, 0);

            text.color = startColor;
            text.text = itemText;

            while (t < 1)
            {
                text.color = Color32.Lerp(startColor, endColor, t);
            
                t += Time.deltaTime / 2f;
                yield return null;
            }
        
            text.color = endColor;
        }
    }
}
