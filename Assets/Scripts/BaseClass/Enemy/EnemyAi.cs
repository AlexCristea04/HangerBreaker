using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using BaseClass;
using BaseClass.GunClass;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public abstract class EnemyAi : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Enemy Info")]
    public String name;
    public int enemyId;
    public int enemyDifficulty;
    public int hp;
    public bool isDead = false;
    public int monsterCoins = 0;
    [Tooltip("This is the refresh update method, the value indicated here will define at which rate the Ai will work")]
    public float AiIntervale;
    [Tooltip("Pass in the animationController")]
    public Animator creatureAnimator;
    [Header("Enemy Agent")] 
    public NavMeshAgent agent;
    public float speed;
    public float acceleration;
    public bool canMove = true;
    
    public List<BehaviourStateData> behaviourState = new List<BehaviourStateData>();
    public int startBehaviourStateIndex;
    [Header("Enemy Audio")] 
    public AudioSource enemySfx;
    public float volumeSfx = 1f;
    public AudioSource enemyVoice;
    public float volumeVoice = 1f;
    public AudioClip deathSfx;
    public AudioClip hitSfx;
    public AudioClip gotShotSfx;
    public bool roamsWhenPcAway = true;

    [Header("Enemy Summoning")] 
    [Tooltip("Whether the ai can spawn enemy or not")]
    public bool canSummonEnemy;
    [Tooltip("Put a prefab of the enemy")]
    public GameObject enemyCanSummon;
    [Tooltip("1 enemy will spawn every summon Intervale")]
    public int summonIntervale;

    [FormerlySerializedAs("aimAt")] [Header("Weapon")]
    //TODO create a weapon script
    public Transform monsterGunTransform;
    public GunAi monsterGun;
    public float distanceGunPlayer;

    [Header("Weapon")]
    //TODO create a weapon script
    public float shootSpeed;

    public GameObject gunPrefab;


    [Header("MovementPattern")] 
    public float distanceCheck;
    [Tooltip("Time Before next patrol")]
    public float nodeWaitTime;
    
    private float timer;
    [NonSerialized]
    public int currentBehaviourStateIndex;
    private BehaviourStateData currentBehaviourState;
    private bool justSwitchedBehaviour;
    [NonSerialized]
    public GameObject player;
    private PlayerHandler playerHandler;

    public GameObject eyePoint;
    [Header("Death")] 
    public GameObject monsterDeadBody;

    public bool canDropGun = true;
    [SerializeField] 
    private int percentageChangeDropWeapon = 30;
    
    void  Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        timer = 0;
        currentBehaviourStateIndex = startBehaviourStateIndex;
        currentBehaviourState = behaviourState[currentBehaviourStateIndex];

        player = GameObject.FindGameObjectWithTag("Player");
        playerHandler = player.GetComponent<PlayerHandler>();

        if (monsterGun != null)
        {
            monsterGun.holdenByPlayer = false; 
        }
        
        InvokeRepeating("AiIntervaleMethod", 0f, AiIntervale);
        StartMonster();
        
        
    }

    private void Update()
    {
        AdjustTransformAimAt();
    }

    public virtual void StartMonster()
    {
        
    }

    public virtual void AiIntervaleMethod()
    {
        
        justSwitchedBehaviour = false;

    }

    private void AdjustTransformAimAt()
    {
        if (monsterGunTransform != null)
        {
            try
            {
                Vector3 direction = player.transform.position - transform.position;

                // Normalize the direction vector to get a unit vector
                Vector3 normalizedDirection = direction.normalized;

                // Calculate the desired position at the specified distance along this direction
                Vector3 desiredPosition = transform.position + normalizedDirection * distanceGunPlayer;
            
                monsterGunTransform.position = desiredPosition;
            
            
                direction.Normalize(); // Ensure the direction vector is normalized
                // Calculate the angle between the object's forward direction and the target direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Rotate the object to face the target
                monsterGunTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                //monsterGunTransform.LookAt(player.transform); Does't work since we are in 2d
            }
            catch (NullReferenceException e)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    protected bool HasDirectLineOfSight(Vector2 endPosition)
    {
        RaycastHit2D[] hits = new RaycastHit2D[10]; // Define an array to store hits, adjust size as needed
        int hitCount = Physics2D.LinecastNonAlloc(eyePoint.transform.position, endPosition, hits);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hits[i];

            if (hit.collider.CompareTag("Obstacles"))
            {
                // If an object with the tag "Obstacles" is hit, return false
                return false;
            }
        }

// If no objects with the tag "Obstacles" were hit, return true
        return true;

    }


    public void PlayStateAnimation(int index)
    {
        //Set trigger to true
        if (behaviourState[index].isTrigger)
        {
            creatureAnimator.SetTrigger(behaviourState[index].nameAnimation);
        }
        else
        {
            // Sett all boolean animation to false
            foreach (var parameters in creatureAnimator.parameters)
            {
                if (parameters.type == AnimatorControllerParameterType.Bool)
                {
                    creatureAnimator.SetBool(parameters.name, false);
                }
            }
            creatureAnimator.SetBool(behaviourState[index].nameAnimation, true);
        }
        
    }
    //SetDestinationToPosition
    public bool SetDestinationToPosition(Vector2 aimedPosition)
    {
        agent.SetDestination(aimedPosition);
        return true;
        /*try
        {
            //We found Destination
            if (IsDestinationReachable(aimedPosition))
            {
                Debug.Log("Found possible destination!");
                //We succeeded in choosing destination

            }
            else
            {
                //Couldn't find a way to point b
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }*/
    }
    /// <summary>
    /// Change the behaviour state of the enemy
    /// </summary>
    /// <param name="index"> The behaviour state Index!</param>
    /// <returns>Did it change behaviour? True = yes! False = No</returns>
    public bool SetBehaviourState(int index)
    {
        try
        {
            currentBehaviourStateIndex = index;
            currentBehaviourState = behaviourState[index];
        }
        catch (Exception e)
        {
            Console.WriteLine(e + " No such value, wtf are you writing whoever is behind the script, YES i am talking to you");
            return false;
        }
        if (!string.IsNullOrEmpty(currentBehaviourState.nameAnimation))
        {
            PlayStateAnimation(index);
        }
        if (behaviourState[index].stateSound)
        {
            enemySfx.Stop();
            enemySfx.clip = behaviourState[index].stateSound;
            enemySfx.Play();
        }

        justSwitchedBehaviour = true;
        return true;
    }
    
    public virtual void OnCollisionEnter(Collision other)
    {
        // Check if the collided object has the tag "Bullet"
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Get the Bullet script component attached to the collided object
            BulletsAi bulletScript = other.gameObject.GetComponent<BulletsAi>();

            // Call the method inside the Bullet script
            if (bulletScript != null)
            {
                DamageEnemy(bulletScript.damage);
            }
            Destroy(other.gameObject);
        }
    }
    
    
    
    /*public void FindAndMoveToRandomPosition(Transform monsterTrans, float radius, float minDistance)
    {
        Vector3 randomPosition = Vector3.zero;
        int tries = 0;

        // Repeat until a reachable position is found or 10 tries have been made
        while (tries < 10)
        {
            // Generate a random point within the specified radius around the middle transform
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            randomPosition = monsterTrans.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // Ensure the random position is at least minDistance away from the monster's current position
            if (Vector3.Distance(randomPosition, monsterTrans.position) < minDistance)
            {
                // Adjust the random position to meet the minimum distance requirement
                randomPosition += (randomPosition - monsterTrans.position).normalized * (minDistance - Vector3.Distance(randomPosition, monsterTrans.position));
            }

            // Check if the random point is within the NavMesh boundaries and reachable
            if (IsPositionWithinNavMesh(randomPosition) && IsPositionReachable(randomPosition))
            {
                Debug.Log("Found Position!");
                // Move to the random reachable position
                SetDestinationToPosition(randomPosition);
                return; // Exit the function after finding a suitable position
            }

            tries++; // Increment the try counter
        }

        Debug.Log("Could not find a suitable position after 10 tries.");
    }*/
    /// <summary>
    /// Finds a Random position and navigates to it if it can!
    /// </summary>
    /// <param name="monsterTrans">The current transformation of the monster</param>
    /// <param name="radius">the Radius in which he position can be found</param>
    /// <param name="minDistance">The minimum radius in which the position can be found</param>
    public void FindAndMoveToRandomPosition(Transform monsterTrans, float radius, float minDistance)
    {
        Vector2 randomPosition = Vector2.zero;
        int tries = 0;

        // Repeat until a reachable position is found or 10 tries have been made
        while (tries < 10)
        {
            // Generate a random point within the specified radius around the middle transform
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            randomPosition = (Vector2)monsterTrans.position + randomOffset;

            // Ensure the random position is at least minDistance away from the monster's current position
            if (Vector2.Distance(randomPosition, monsterTrans.position) < minDistance)
            {
                // Adjust the random position to meet the minimum distance requirement
                randomPosition += (randomPosition - (Vector2)monsterTrans.position).normalized *
                                  (minDistance - Vector2.Distance(randomPosition, (Vector2)monsterTrans.position));
            }

            // Check if the random point is within the NavMesh boundaries and reachable
            if (IsPositionWithinNavMesh(randomPosition) && IsPositionReachable(randomPosition))
            {
                // Move to the random reachable position
                SetDestinationToPosition(randomPosition);
                return; // Exit the function after finding a suitable position
            }

            tries++; // Increment the try counter
        }

        Debug.Log("Could not find a suitable position after 10 tries.");
    }

    
    /// <summary>
    /// Check if the given position is reachable on the Nav Mesh
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    bool IsPositionWithinNavMesh(Vector3 position)
    {
        // Check if the position is within the NavMesh bounds
        //TODO THOSE THIS WORK
        return agent.CalculatePath(transform.position,new NavMeshPath());
    }
    bool IsPositionReachable(Vector3 position)
    {
        NavMeshHit hit;
        
        // Check if the position is reachable
        return NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas);
    }

    IEnumerator WaitAndChangeBehaviour(float time, int behaviourIndex)
    {
        yield return new WaitForSeconds(time);
        SetBehaviourState(behaviourIndex);
        Logger($"Finished Coroutine : [WaitAndChangeBehaviour]: time : [{time}]");
    }
    
    

    /// <summary>
    /// Called By bullets when Monster gets hits by bullet
    /// </summary>
    /// <param name="x">Damage received!</param>
    public void DamageEnemy(int x)
    {
        //int? damageBuff = GameObject.Find("Canvas").GetComponent<BuffManager>().GetDamageBuff();
        
        hp -= x;
        
        if (hp <= 0)
        {
            OnEnemyDeath();
        }
        
    }
    /// <summary>
    /// Called when enemy Hp reaches lower than 0 hp!
    /// </summary>
    public virtual void OnEnemyDeath()
    {

        playerHandler.OnEnemyKilled();

        //If we do implement the pickup mechanic
        if (canDropGun && percentageChangeDropWeapon >= RandomNumberGenerator.GetInt32(100))
        {
            Instantiate(monsterGun, transform);
        }

        //TODO add dead body
        if (monsterDeadBody !=null)
        {
            Instantiate(monsterDeadBody, transform);
        }
        GameObject
            .Find("WaveManagementSystem")
            .GetComponent<WaveManagementSystem>()
            .CheckNumberOfEnemy(); ;
        Destroy(gameObject);
    }
    public void Logger(String x)
    {
        Debug.Log($"[Error Enemy][{name}][{DateTime.Now.ToString("hh:mm:ss tt")}] - {x}");
    }

}
