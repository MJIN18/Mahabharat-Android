using UnityEngine;

public class TrapsBehaviour : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float moveSpeed = 5.0f;

    public Vector3 rotationDirection;
    public Vector3 moveDirection;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local Y-axis
        transform.Rotate(rotationDirection, rotationSpeed * Time.deltaTime);

        // Move the object forward along its local Z-axis
        transform.parent.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
