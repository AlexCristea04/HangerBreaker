using UnityEngine;

namespace DefaultNamespace
{
    public class GunDirectionManagerEnemy : MonoBehaviour
    {
        private Camera mainCam;
        private Vector3 aimPos;
        public GameObject bullet;
        public Transform bulletTransform;
        private GameObject enemyGameObject;
        private GameObject player;
        public bool ready; // state wether a bullet can be shot or not
        private float timer;
        public float attackSpeed = 0.3f; // cooldown between bullet shots
        // Start is called before the first frame update
        void Start()
        {
            enemyGameObject= gameObject.transform.parent.gameObject;
            player = GameObject.Find("Player");
            aimPos = player.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // aiming code
            Vector3 rotation = aimPos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,rotZ);
        }
    }
}