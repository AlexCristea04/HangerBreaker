using System;
using BaseClass.GunClass;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    private Transform player;  // Reference to the player's transform
    private Transform crosshair;  // Reference to the crosshair's transform
    public float horizontalDistanceFromPlayer = 1f;  // Horizontal distance between player and gun
    public float verticalDistanceFromPlayer = 0.5f;  // Vertical distance between player and gun
    private PlayerControls playerControls;
    private float shootInput;
    public GunAi gunLogic;


    private void Awake()
    {
        playerControls = new PlayerControls();

        // Bind the shooting actions
        playerControls.Movement.Shoot.performed += _ => OnShootPressed();
        playerControls.Movement.Shoot.canceled += _ => OnShootReleased();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        crosshair = GameObject.Find("Crosshair").transform;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        gunLogic.holdenByPlayer = true;
        gunLogic.gunHolderTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        gunLogic.holdenByPlayer = true;
    }

    private void Update()
    {
        RotateGun();
        shootInput = playerControls.Movement.Shoot.ReadValue<float>();
    }

    private void RotateGun()
    {
        // Calculate the direction from the player to the crosshair
        Vector3 direction = crosshair.position - player.position;

        // Calculate the angle between the player and the crosshair
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate the new position for the gun relative to the player
        Vector3 offset = new Vector3(horizontalDistanceFromPlayer, verticalDistanceFromPlayer, 0f);
        Vector3 gunPosition = player.position + (direction.normalized * offset.magnitude);

        // Apply the calculated position
        transform.position = new Vector3(gunPosition.x, gunPosition.y, transform.position.z);

        // Rotate the gun to face the crosshair
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip the gun horizontally if pointing left
        if (direction.x < 0)
        {
            if (transform.localScale.y > 0)
            {
                Vector3 gunScale = transform.localScale;
                gunScale.y *= -1;
                transform.localScale = gunScale;
            }
        }
        else
        {
            if (transform.localScale.y < 0)
            {
                Vector3 gunScale = transform.localScale;
                gunScale.y *= -1;
                transform.localScale = gunScale;
            }
        }
    }

    private void OnShootPressed()
    {
        // Code to execute when the shoot button is pressed
        gunLogic.isShooting = true;
    }

    private void OnShootReleased()
    {
        // Code to execute when the shoot button is released
        gunLogic.isShooting = false;
    }
}
