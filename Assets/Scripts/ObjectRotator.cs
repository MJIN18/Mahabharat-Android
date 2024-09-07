using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;

    public bool canScale;

    public float scaleSpeed = 0.5f;  // Speed at which the object scales
    public float minScale = 0.5f;    // Minimum scale factor
    public float maxScale = 2f;      // Maximum scale factor

    private bool isScalingUp = true; // Flag to indicate whether the object is currently scaling up or down

    void Update()
    {
        // Rotate the object around its up-axis at the desired speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if(canScale )
        {
            float newScale = transform.localScale.x + (isScalingUp ? scaleSpeed : -scaleSpeed) * Time.deltaTime;

            // Ensure the new scale factor stays within the defined min and max scale limits
            if (newScale < minScale)
            {
                newScale = minScale;
                isScalingUp = true;
            }
            else if (newScale > maxScale)
            {
                newScale = maxScale;
                isScalingUp = false;
            }

            // Set the new scale for all axes
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}