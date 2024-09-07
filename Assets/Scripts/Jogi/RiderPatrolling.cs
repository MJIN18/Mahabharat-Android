using UnityEngine;
using DG.Tweening;

public class RiderPatrolling : MonoBehaviour
{
    public Transform[] patrolPoints;   // An array to hold the patrol points.
    public float patrolDuration = 3f;  // Time taken to move from one point to another.
    public float dis = 1;
    public float rotateDuration = 0.5f; // Time taken to rotate towards the patrolling point.
    public bool loopPatrol = true;     // Set to true to keep patrolling indefinitely.
    public Ease easeType = Ease.Linear; // Ease type for movement. You can experiment with other types.

    public bool isTurning;

    public int currentPatrolIndex = 0; // Index of the current patrol point.

    private void Start()
    {
        //StartPatrol();
    }

    private void StartPatrol()
    {
        if (patrolPoints.Length < 2)
        {
            Debug.LogError("Please assign at least two patrol points to the character.");
            return;
        }

        // Move to the next patrol point.
        MoveToNextPoint();

        if (loopPatrol)
        {
            // If looping is enabled, call this function again after the patrol duration.
            Invoke("StartPatrol", patrolDuration);
        }
    }

    private void Update()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        // Get the next patrol point's position.
        //Vector3 targetPosition = patrolPoints[currentPatrolIndex].position;

        // Calculate the rotation towards the patrol point.
        //Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        // Use DOTween to move and rotate the character smoothly.
        if (!isTurning)
        {
            transform.DOMove(patrolPoints[currentPatrolIndex].position, patrolDuration).SetEase(easeType);

            float distance = Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position);

            if (distance < dis)
            {
                isTurning = true;
            }
        }

        if (isTurning)
        {
            Quaternion targetRotation = Quaternion.LookRotation(patrolPoints[currentPatrolIndex].position - transform.position);
            transform.DORotateQuaternion(targetRotation, rotateDuration).OnComplete (() =>
            {
                OnRotationComplete();

                // Increment the current patrol index and loop back if needed.
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            });
        }
    }
    private void OnRotationComplete()
    {
        isTurning = false;
    }
}
