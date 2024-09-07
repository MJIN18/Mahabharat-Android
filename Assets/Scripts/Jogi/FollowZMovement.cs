using UnityEngine;

public class FollowZMovement : MonoBehaviour
{
    public Transform target; // Assign the target object in the Unity Inspector.

    void Update()
    {
        if (target != null)
        {
            // Get the current position of the object to be followed.
            Vector3 currentPosition = transform.position;

            // Update only the Z position to match the target's Z position.
            currentPosition.z = target.position.z;

            // Assign the updated position back to the object.
            transform.position = currentPosition;
        }
    }
}
