using UnityEngine;

namespace Guns
{
    public class GunController : MonoBehaviour
    {
        public GameObject bullet;
        public float bulletSpeed;
        public float range;
        public int damage;
        public float timeBetweenShots;
    
        private float shotCounter;
        private bool isFiring;
        public Transform firePoint;
        void Awake()
        {
            isFiring = false;
            shotCounter = 0f;
        }

    
        void Update()
        {
            isFiring = Input.GetKey(KeyCode.Mouse0);
            shotCounter -= Time.deltaTime;
            if(isFiring &&shotCounter <= 0f)
            {
                float criticMultiplier = 1;
                if (Random.value > PlayerController.Instance.critic) criticMultiplier = 2;
                shotCounter = timeBetweenShots;
                GameObject newBullet = Instantiate(bullet);
                newBullet.transform.position = firePoint.position;
                newBullet.GetComponent<BulletController>().SetBullet(range, bulletSpeed*PlayerController.Instance.shootSpd, (int)(damage* PlayerController.Instance.damage* criticMultiplier));
            
            }
        
        }
    }
}