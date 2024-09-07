using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour
{
    public FixedTouchField touchField;
    public int threshold;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (touchField.Pressed)
        {
            touchField.TouchDist = touchField.TouchDist / threshold;

            if(touchField.TouchDist.x > 0.2f || touchField.TouchDist.x < -0.2f)
            {
                Debug.Log(touchField.TouchDist.x + " X");
            }
            if(touchField.TouchDist.y > 0.2f || touchField.TouchDist.y < -0.2f)
            {
                Debug.Log(touchField.TouchDist.y + " Y");
            }
        }
    }
}
