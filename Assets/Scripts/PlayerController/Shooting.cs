using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool ready; // state wether a bullet can be shot or not
    private float timer;
    public float attackSpeed = 0.3f; // cooldown between bullet shots
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // aiming code

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);

        if (!ready)
        {
            timer += Time.deltaTime;
            if (timer > attackSpeed)
            {
                ready = true; // allow the gun to fire
                timer = 0; // reset the time for the delay of the next bullet
            }
        }

        // bullet shoot code
        if (ready && Input.GetMouseButton(0))
        {
            ready = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        }
    }
}
