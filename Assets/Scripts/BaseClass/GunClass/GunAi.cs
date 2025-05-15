using System;
using JetBrains.Annotations;
using UnityEngine;
using Unity;
using Unity.VisualScripting;

namespace BaseClass.GunClass
{
    public abstract class GunAi : MonoBehaviour
    {

        public GameObject bulletPrefab;
        public GameObject bulletPrefabPlayer;

        [Header("Shooting Information")] 
        public float shootIntervale = 1f;
        //Define Whether the gun is shooting or not
        public bool isShooting = false;
        private float timer = 0f;
        public Transform gunHolderTransform;
        public bool holdenByPlayer;
        [CanBeNull] public Transform gunBarrelEnd;
        public int amountOfBullets =1;
        
        private Transform monsterTransform;
        void Start()
        {
            monsterTransform = GetComponentInParent<Transform>();
            if (gunBarrelEnd == null)
            {
                gunBarrelEnd = transform;
            }
        }
        // Update is called once per frame
        void Update()
        {
            // Example: Fire when the player presses the spacebar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBullet();
            }
            
            
            timer -= Time.deltaTime;
            if (timer <= 0 && isShooting)
            {
                timer = shootIntervale;
                for (int i = 0; i < amountOfBullets; i++)
                {
                    FireBullet();
                }
                // Reset the timer
                
            }
        }

        public void MakePlayerGun()
        {
            
        }

        public void FireBullet()
        {
            if (holdenByPlayer)
            {
                GameObject bullet = Instantiate(bulletPrefabPlayer, gunBarrelEnd.position, Quaternion.identity);
                BulletsAi bulletAi = bullet.GetComponent<BulletsAi>();

                
                // Calculate the direction towards the target position
                Vector2 directionTowardsTarget = ((Vector2)gunHolderTransform.transform.position - (Vector2)transform.position).normalized;

                // Calculate the opposite direction
                Vector2 oppositeDirection = -directionTowardsTarget;
            
            
                bulletAi.TrueStart(oppositeDirection,holdenByPlayer); 
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, gunBarrelEnd.position, Quaternion.identity);
                BulletsAi bulletAi = bullet.GetComponent<BulletsAi>();

            
            
            
                // Calculate the direction towards the target position
                Vector2 directionTowardsTarget = ((Vector2)gunHolderTransform.transform.position - (Vector2)transform.position).normalized;

                // Calculate the opposite direction
                Vector2 oppositeDirection = -directionTowardsTarget;
            
            
                bulletAi.TrueStart(oppositeDirection,holdenByPlayer);
            }
        }
    }
}