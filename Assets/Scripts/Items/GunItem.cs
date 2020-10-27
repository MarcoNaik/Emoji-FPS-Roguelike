using System;
using Guns;
using UnityEngine;

namespace Items
{
    public class GunItem: ItemController
    {
        public GameObject prefab;
        public Sprite gunSprite;

        private GunSwitch gunHolder;

        private void Start()
        {
            gunHolder = FindObjectOfType<GunSwitch>();
        }

        protected override void Effect()
        {
            Debug.Log("Gun Picked Effect");
            Instantiate(prefab, gunHolder.transform);
            int childCount = gunHolder.transform.childCount;
            
            gunHolder.SetCurrentGun(childCount - 1);
            
            FindObjectOfType<GunsHUDController>().GunAdded(gunSprite);
        }
    }
}