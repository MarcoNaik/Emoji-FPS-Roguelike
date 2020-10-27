using UnityEngine;

namespace Guns
{
    public class BulletController : MonoBehaviour
    {
        private float lifeTime;
        private float speed;
        private int damage;
        private Camera playerCamera;

        public void SetBullet(float range, float bulletSpeed, int bulletDamage)
        {
            playerCamera = FindObjectOfType<Camera>();
            lifeTime = range;
            speed = bulletSpeed;
            damage = bulletDamage;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(playerCamera.transform.forward * speed *Time.deltaTime);
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
        
            if (other.gameObject.CompareTag($"Enemy"))
            {
                EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
                enemy.SetAggro();
                Destroy(gameObject);
            }
        }
    }
}