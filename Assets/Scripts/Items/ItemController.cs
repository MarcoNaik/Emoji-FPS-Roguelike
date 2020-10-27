using System;
using System.Security.Cryptography;
using UI;
using UnityEngine;

namespace Items
{
    public abstract class ItemController : Interactable
    {
        protected String GrabbedText;

        

        public override void OnInteract()
        {
            PickedUp();
        }

        private void PickedUp()
        {
            Debug.Log("an item has been picked up");
            Effect(); 
            Destroy(gameObject);
            ItemGrabbedUI.Instance.StartGrabCoroutine(GrabbedText);
        }

        protected abstract void Effect();
    }

    
}