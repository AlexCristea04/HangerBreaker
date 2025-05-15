using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;


namespace BaseClass
{
    public abstract class BulletsAi : MonoBehaviour
    {
        //Gun must have collider,Rigidbody2d
        [Header("Bullets Info")] 
        public String bulletName;
        public bool shotByPlayer = false;
        public float AiIntervale;
        public int damage;
        public Transform gunDirection;
        [Header("Components")] 
        public Rigidbody2D rb;
        public Collider2D col;
        [Header("Properties")] 
        public float spread;

        public float speed;
        public bool bounce= false;
        public bool accelerates;
        public float accelerateMultiplier;
        public bool isLaser = false;
        public bool canHitEnemy = false;
        
        public bool homing;
        [Tooltip("This only works if homing is on")]
        private Transform destination;
        public SpringJoint2D HomingJoint2D;
        [Header("Particles")] 
        public ParticleSystemTrailMode trail;

        public ParticleSystem death;

        public float scale;
        
        [Header("Summons Bullets")] 
        public bool summonsBullets;
        public float summonIntervale;
        public GameObject prefabBullet;

        [Header("Audio")] 
        public AudioSource deathSFX;
        public AudioSource hitSFX;
        public AudioSource shotSFX;
        public AudioClip shotSFXAUDIO;
        
        //public StatusEffectInterface applyEffect;
        private Transform player;


        private void Start()
        {
            _isshotSfxNotNull = shotSFX != null;
        }

        /// <summary>
        /// Will be called by the Gun Class on instantiation, to define the true start method.
        /// This will make sure the gun and the bullet are on the same page
        /// </summary>
        /// <param name="shotDirection"></param>
        public void TrueStart(Vector2 shotDirection, bool shotByPlayerGun)
        {
            this.shotByPlayer= shotByPlayerGun;
            
            float spreadAngle = UnityEngine.Random.Range(-spread / 2f, spread / 2f);

            Vector2 rotatedDirection = Quaternion.Euler(0f, 0f, spreadAngle) * shotDirection;

            rb.velocity = rotatedDirection.normalized * speed;

            ChangeBulletDirection();
            //InvokeRepeating("AiIntervaleMethod", 0.1f, AiIntervale);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (summonsBullets)
            {
                InvokeRepeating("SummonBullet", 1f, summonIntervale);
            }
            if (_isshotSfxNotNull)
            {
                
                shotSFX.PlayOneShot(shotSFXAUDIO);
            }
        }
        private float timer;
        private float bulletLifetime;
        private bool _isshotSfxNotNull;

        private void Update()
        {

            bulletLifetime += Time.deltaTime;
        }

        /// <summary>
        /// Small Update Method
        /// </summary>

        public virtual void AiIntervaleMethod()
        {
            
            if (!shotByPlayer)
            {
                destination = player;
            }
            if (accelerates)
            {
                rb.velocity *= accelerateMultiplier;
            }
            if (homing)
            {
                HomingHandle();
            }
            
        }

        
        /// <summary>
        /// Change the angle of the bullet for it to spread!
        /// </summary>
        public virtual void ChangeBulletDirection()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
            if (rb != null)
            {
                Vector2 currentVelocity = rb.velocity.normalized; // Get the current velocity direction

                // Calculate a random angle within the range of [-maxAngleChange, maxAngleChange]
                float randomAngle = UnityEngine.Random.Range(5f,5f);
                UnityEngine.Random.Range(-spread, spread);

                // Rotate the current velocity direction by the random angle
                Vector2 newDirection = Quaternion.Euler(0f, 0f, randomAngle) * currentVelocity;

                // Apply the new direction to the Rigidbody2D
                rb.velocity = newDirection * rb.velocity.magnitude;
            }
            else
            {
                Debug.LogWarning("Rigidbody2D component not found!"); // Warn if the Rigidbody2D component is not found
            }
        }

        public virtual void SummonBullet()
        {

            GameObject bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            BulletsAi bulletAi = bullet.GetComponent<BulletsAi>();
            
            // Calculate the direction towards the target position
            Vector2 directionTowardsTarget = -((Vector2)player.position - (Vector2)transform.position).normalized;

            // Calculate the opposite direction
            Vector2 oppositeDirection = -directionTowardsTarget;
        
        
            bulletAi.TrueStart(oppositeDirection, shotByPlayer);

        }
        public virtual void HomingHandle()
        {
            HomingJoint2D.connectedAnchor = player.transform.position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Obstacles"))
            {
                
                OnCollideWithWalls(other);
            }
            if (other.gameObject.CompareTag("Player"))
            {
                OnCollideWithPlayer(other);
            }
            if (other.gameObject.CompareTag("Enemy"))
            {
                OnCollideWithEnemy(other);
            }
            if (other.gameObject.CompareTag("Bullets"))
            {
                OnCollideWithBullet(other);
            }
        }

        /// <summary>
        /// When the bullet Hits a wall, it will act!
        /// </summary>
        /// <param name="other"></param>
        public virtual void OnCollideWithWalls(Collider2D other)
        {

            Destroy(gameObject);
            
        }

        public virtual void OnCollideWithPlayer(Collider2D other)
        {
            if (!shotByPlayer && !other.GetComponent<PlayerMovement>().isSliding)
            {
                other.GetComponent<PlayerHandler>().TakeDamage(damage);
                if (hitSFX != null)
                {
                    if (!hitSFX.isPlaying)
                    {
                        hitSFX.Play();
                    }
                }
                Destroy(gameObject);
            }
        }

        public virtual void OnCollideWithEnemy(Collider2D other)
        {
            if (shotByPlayer)
            {
                other.GetComponent<EnemyAi>().DamageEnemy(damage);
                if (hitSFX != null)
                {
                    if (!hitSFX.isPlaying)
                    {
                        hitSFX.Play();
                    }
                }
                Destroy(gameObject);
            }
            
        }
        public virtual void OnCollideWithBullet(Collider2D other)
        {
            if (hitSFX != null)
            {
                if (!hitSFX.isPlaying)
                {
                    hitSFX.Play();
                }
            }
        }

        public void OnDestroy()
        {
            
            if (deathSFX != null)
            {
                if (!deathSFX.isPlaying)
                {
                    deathSFX.Play();
                }
            }
            if (death != null)
            {
                if (!death.isPlaying)
                {
                    death.Play();
                }
            }
        }
    }
}