using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public class PlayerMovement : MonoBehaviour
    {
        public float verticalVelocity = 0f;
        public float dodgeSpeed = 5f;
        public float maxDodgeDistance = 2f;

        private Rigidbody playerRigidbody;
        private float originalX;
        private bool isDodging = false;
        private float dodgeStartTime;

        void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            originalX = transform.position.x;
        }

        void Update()
        {
            // Check for input to initiate dodge
            if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
            {
                // Start the dodge movement
                isDodging = true;
                dodgeStartTime = Time.time;
            }
        }

        void FixedUpdate()
        {
            // If the character is currently dodging
            if (isDodging)
            {
                // Calculate the new position for the dodge
                float dodgeDistance = Mathf.Lerp(0, maxDodgeDistance, (Time.time - dodgeStartTime) * dodgeSpeed);
                Vector3 c = new Vector3(originalX + dodgeDistance, transform.position.y, transform.position.z);

                // Move the player towards the target position using Rigidbody's MovePosition method
                //playerRigidbody.MovePosition(targetPosition);

                // Check if the dodge distance has reached the maximum distance
                if (dodgeDistance >= maxDodgeDistance)
                {
                    // End the dodge movement
                    isDodging = false;
                    originalX = transform.position.x;
                }
            }
        }
    }



}
