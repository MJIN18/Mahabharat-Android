using UnityEngine;
using System.Collections;

public class ImageScalingAnimation : MonoBehaviour
{
    public RectTransform imageTransform;
    public float animationDuration = 1f;

    void Start()
    {
        imageTransform = GetComponent<RectTransform>();
        // Start the scaling animation
        StartScalingAnimation();
    }

    void StartScalingAnimation()
    {
        // Set the initial scale
        imageTransform.localScale = new Vector3(0.2f, 0.2f, 1f);

        // Use LeanTween to smoothly scale the image to 1f
        LeanTween.scale(imageTransform, new Vector3(1f, 1f, 1f), animationDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(OnAnimationComplete); // Optional: Callback when the animation is complete
    }

    void OnAnimationComplete()
    {
        // Animation complete, you can add any additional logic here
        StartScalingAnimation();
    }
}
