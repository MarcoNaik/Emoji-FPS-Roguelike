using UnityEngine;

namespace Guns
{
    public class GunSwitch : MonoBehaviour
    {
        public int currentGun= 0;

        private GunsHUDController hudController;

        private void Awake()
        {
            hudController = FindObjectOfType<GunsHUDController>();
        }

        private void SelectWeapon()
        {
            int i = 0;
            foreach (Transform gun in transform)
            {
                if(i==currentGun)
                    gun.gameObject.SetActive(true);
                else
                    gun.gameObject.SetActive(false);
                i++;
            }
            hudController.GunSelected(currentGun);
        }

        public void SetCurrentGun(int childIndex)
        {
            currentGun = childIndex;
            SelectWeapon();
        }


        void Start()
        {
            SelectWeapon();
        }

        void Update()
        {
            if (transform.childCount == 0) return;
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            int previousGun = currentGun;

            if (scrollWheel>0f)
            {
                currentGun = Mathf.Abs(currentGun+1)%transform.childCount;
            }
            else if (scrollWheel<0f)
            {
                currentGun = Mathf.Abs(currentGun-1)%transform.childCount;
            }
        
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentGun = 0;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            {
                currentGun = 1;
            }
            if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            {
                currentGun = 2;
            }
            if(Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            {
                currentGun = 3;
            }

            if (previousGun != currentGun)
            {
                SelectWeapon();
            }
        
        }
    }
}
