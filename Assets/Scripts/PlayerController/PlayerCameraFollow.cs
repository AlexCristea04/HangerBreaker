namespace BaseClass
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class PlayerCameraFollow : MonoBehaviour
    {
        public Transform player;
        public Transform cameraTrans;
        private Vector3 newCameraPosition;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            newCameraPosition  = new Vector3(player.transform.position.x, player.transform.position.y, cameraTrans.position.z);
            cameraTrans.position = newCameraPosition;
        }
    }
}