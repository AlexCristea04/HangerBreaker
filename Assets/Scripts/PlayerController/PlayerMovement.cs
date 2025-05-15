using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    private PlayerControls playerControls;
    private BoxCollider2D boxCollider2d;
    private Rigidbody2D rigidBody2d;
    private Animator animator;

    //inputs
    private Vector2 moveInput;
    private float slideInput;

    //character controller properties
    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;

    [Header("Slide")]
    [SerializeField] private float slideMaxSpeed;
    [SerializeField] private float slideDeceleration;
    [SerializeField] private float slideCooldown;
    [SerializeField] private int maxSlideChains;
    [SerializeField] private float chainedSlideDeceleration;
    [SerializeField] private float slideTime = 0.2f;


    //global variables and such
    public bool isSliding = false;
    private bool canSlide = true;  
    private float slideCooldownTimer = 0f;
    public float slideChainedTimer = 0f;
    private float initalSlideMaxSpeed;
    private int slideChains = 0;
    private bool facingRight = true;
    private bool wasSlideInputPressed = false;

    //initializing needed components
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
        rigidBody2d = gameObject.GetComponent<Rigidbody2D>();
        boxCollider2d = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        initalSlideMaxSpeed = slideMaxSpeed;
    }

    //called once every frame. used to take player input. physics calculations are done in FixedUpdate. 
    void Update()
    {
        //basic movement
        moveInput = playerControls.Movement.Move.ReadValue<Vector2>();
        slideInput = playerControls.Movement.Slide.ReadValue<float>();


    }

    private void FixedUpdate()
    {

        if (isSliding == false && slideInput == 0)
            Movement();

        if(!isSliding)
            animator.SetFloat("xVelocity", Math.Abs(rigidBody2d.velocity.x));

        if (canSlide && slideInput == 1 &&!wasSlideInputPressed)
            StartCoroutine(Slide());

        wasSlideInputPressed = (slideInput == 1);
    }



    public void Movement()
    {

        //calculate the direction we want to move in and our desired speed/velocity
        float x = moveInput.x;
        float xTargetSpeed = x * maxSpeed;
        float y = moveInput.y;
        float yTargetSpeed = y * maxSpeed;

        //calculate difference between current velocity and desired velocity
        float xSpeedDif = xTargetSpeed - rigidBody2d.velocity.x;
        float ySpeedDif = yTargetSpeed - rigidBody2d.velocity.y;

        //change acceleration/decceleration depending on situation 
        float accelRate;

        if (Mathf.Abs(xTargetSpeed) > 0f)
            accelRate = acceleration;
        else
            accelRate = deceleration;

        //applies acceleration rate to speed difference
        //raised to a set power so acceleration can increase with higher speeds
        //since we use the absolute num of speed difference, we reapply direction with Mathf.sign
        float xMovement = Mathf.Pow(Mathf.Abs(xSpeedDif) * accelRate, velPower) * Mathf.Sign(xSpeedDif);
        float yMovement = Mathf.Pow(Mathf.Abs(ySpeedDif) * accelRate, velPower) * Mathf.Sign(ySpeedDif);

        //normalizes the movement when moving diagonally, prevents moving faster
        Vector2 normalizedMovement = new Vector2(xMovement, yMovement).normalized;
        normalizedMovement = normalizedMovement.normalized * Time.deltaTime;

        if (moveInput.x == 1 && moveInput.y == 1)
            rigidBody2d.AddForce(normalizedMovement);

        else
        {
            rigidBody2d.AddForce(xMovement * Vector2.right);
            rigidBody2d.AddForce(yMovement * Vector2.up);
        }

        HandleCharacterFlip(x);
    }

    private void HandleCharacterFlip(float moveDirection)
    {
        if (moveDirection > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveDirection < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public IEnumerator Slide()
    {
        isSliding = true;
        canSlide = false;

        // Calculate the slide direction based on movement input
        Vector2 slideDirection = moveInput.normalized;

        // Apply the slide velocity
        rigidBody2d.velocity = slideDirection * slideMaxSpeed;
        animator.SetBool("isSliding", isSliding);
        yield return new WaitForSeconds(slideTime);

        isSliding = false;
        animator.SetBool("isSliding", isSliding);
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;

    }

    /*
    // check if player can slide
    if (!isSliding && rigidBody2d.velocity.magnitude > 1f && slideInput == 1)
    {
        //initial slide
        if (slideCooldownTimer <= 0f)
        {

            if (slideChainedTimer > 0.4f && slideChainedTimer < 0.9f && slideChains < maxSlideChains)
            {
                Debug.Log("slide chain!");
                slideMaxSpeed += 2f;
                slideChains++;
            }
            else {
                slideMaxSpeed = initalSlideMaxSpeed;
                slideChains = 0;
            }

            isSliding = true;
            animator.SetBool("isSliding", isSliding);


            Debug.Log("slide chain: " + slideChains);
            rigidBody2d.velocity = rigidBody2d.velocity.normalized * slideMaxSpeed;
            slideCooldownTimer = slideCooldown;

        }


        else if(slideCooldownTimer > 0f)
        {
            slideChains = 0;
            slideMaxSpeed = initalSlideMaxSpeed;
            Debug.Log("Inputs during slide cooldown! Slide chain canceled");
        }

    } 

    if (isSliding)
    {
        // Apply slide deceleration
        Vector2 slideDirection = rigidBody2d.velocity.normalized;
        Vector2 slideForce = -slideDirection * slideDeceleration;
        rigidBody2d.AddForce(slideForce);

        // Check if slide is to be stopped
        if (slideInput == 0 || rigidBody2d.velocity.magnitude < 0.1f)
        {
            slideInput = 0;
            isSliding = false;
            animator.SetBool("isSliding", isSliding);
            //slideChainedTimer = 1f;

            // Transition to normal movement
            // If slide velocity is higher than maxSpeed, transition with the current velocity
            float transitionSpeed = Mathf.Min(maxSpeed, rigidBody2d.velocity.magnitude);
            rigidBody2d.velocity = rigidBody2d.velocity.normalized * transitionSpeed;
        }
    }
    */


    private void SlideCooldownUpdate()
    {
        if (slideCooldownTimer > 0f)
            slideCooldownTimer -= Time.deltaTime;

        if (slideChainedTimer > 0f)
            slideChainedTimer -= Time.deltaTime;
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        /*
        // Check if the collided object has the tag "Bullet"
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Get the Bullet script component attached to the collided object
            BulletsAi bulletScript = other.gameObject.GetComponent<BulletsAi>();

            // Call the method inside the Bullet script
            if (bulletScript != null)
            {
                bulletScript.OnHitEnemy();
                hp -= bulletScript.damage;
            }
            Destroy(other.gameObject);
        */
    }
}
