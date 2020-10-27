using System.Collections.Generic;
using UnityEngine;

namespace Guns
{
    public class GunsHUDController : MonoBehaviour
    {

        private int gunsQuantity;


        private void Awake()
        {

            foreach (Transform boxObject in transform)
            {
                
                boxObject.gameObject.SetActive(false);
            }
        }
        

        public void GunAdded(Sprite gun)
        {
            transform.GetChild(gunsQuantity).gameObject.SetActive(true);
            transform.GetChild(gunsQuantity).GetComponent<GunBoxHUDController>().SetGun(gun, gunsQuantity+1);
            gunsQuantity++;
        
        }

        public void GunSelected(int gunIndex)
        {
            for (int i = 0; i < gunsQuantity ; i++)
            {
                if(i == gunIndex)
                    transform.GetChild(i).GetComponent<GunBoxHUDController>().OnSelectedGun();
                else
                {
                    transform.GetChild(i).GetComponent<GunBoxHUDController>().OnDeselectedGun();
                }
            }
        }
    }
}
