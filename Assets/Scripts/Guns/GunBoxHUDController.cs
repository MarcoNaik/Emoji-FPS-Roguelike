using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Guns
{
    public class GunBoxHUDController : MonoBehaviour
    {
        public Color selected;
        public Color unSelected;
        public Color selectedNumber;
        public Color unSelectedNumber;
        private Image gun;
        private Image box;
        private TextMeshProUGUI number;

        private void Awake()
        {
            gun = transform.GetChild(0).GetComponent<Image>();
            box = GetComponent<Image>();
            number = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        public void SetGun(Sprite gunSprite, int gunNumber)
        {
            gun.sprite = gunSprite;
            number.text = gunNumber.ToString();
        }

        public void OnSelectedGun()
        {
            gun.color = selected;
            box.color = selected;
            number.color = selectedNumber;
        }
    
        public void OnDeselectedGun()
        {
            gun.color = unSelected;
            box.color = unSelected;
            number.color = unSelectedNumber;
        }
    }
}
