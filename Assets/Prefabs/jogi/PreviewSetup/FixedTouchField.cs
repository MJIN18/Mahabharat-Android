using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    [HideInInspector]
    public bool isDragging = false;

    public bool isTouchField;

    public bool isPrevTouchField = false;

    public bool isCursorLocked = false;

    public StarterAssetsInputs starterInput;

    [SerializeField]
    private GameObject TouchField;
  /*  [SerializeField]
    private GameObject NewPositionofTouchField;
    [SerializeField]
    Canvas touchCanvas;*/

    Vector3 TouchFieldPosition;

    //  RectTransform rt;

    // Use this for initialization
    void Start()
    {
        //    rt = TouchField.GetComponent<RectTransform>();

        TouchFieldPosition = TouchField.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || isPrevTouchField)
        {
            if (Pressed)
            {
                if (Input.touches.Length >= 2 && isTouchField)
                {

                    Debug.Log("dubara call kyu kra");
                    int currentTouch = Input.touches.Length - 1;
                    int previousTouch = currentTouch - 1;
                    Touch touch = Input.GetTouch(currentTouch);

                    //TouchDist = Input.touches[currentTouch].position - PointerOld;
                    //PointerOld = Input.touches[currentTouch].position;

                    if (touch.position.x > Screen.width / 2)  // for Portrait (Double touch only on Screen without Controls)
                                                              //        if (touch.position.x > Screen.height || touch.position.x < Screen.height)  // for LandScape (Double touch only on Screen without Controls)
                    {
                        TouchDist = Input.touches[currentTouch].position - PointerOld;
                        PointerOld = Input.touches[currentTouch].position;
                    }
                    else
                    {
                        TouchDist = Input.touches[previousTouch].position - PointerOld;
                        PointerOld = Input.touches[previousTouch].position;
                    }
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }
            else
            {
                TouchDist = new Vector2();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1)) // 1 represents the right mouse button
            {
                // Toggle the cursor state
                isCursorLocked = !isCursorLocked;
                starterInput.SetCursorState(isCursorLocked);
            }
        }
    }

    public void TouchPress()
    {
        Pressed = true;
    }

    public void TouchReleased()
    {
        Pressed = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("call kyu kra");
        //if (Input.touches.Length == 2)
        //{
        //    isDragging = true;
        //}
        //Pressed = true;
        //PointerId = eventData.pointerId;
        //PointerOld = eventData.position;

        if (isTouchField)
        {
            Debug.Log("call kyu kra");
            if (Input.touches.Length == 2)
            {
                isDragging = true;
            }
            Pressed = true;
            PointerId = eventData.pointerId;
            PointerOld = eventData.position;
            //Debug.Log(eventData.pointerId);
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        isDragging = false;
    }

  /*  public void EnableDisableTouchField(bool b)
    {
        TouchField.SetActive(b);
    }*/

  /*  public void ResizeTouchField(int c)
    {
        if (c == 0)
        {

            //TouchField.transform.position = new Vector3(TouchField.transform.position.x, NewPositionofTouchField.transform.position.y, TouchField.transform.position.z);
            touchCanvas.sortingOrder = -2;
        }
        else if (c == 1)
        {
            //TouchField.transform.position = TouchFieldPosition;
            touchCanvas.sortingOrder = 0;
        }
    }*/
}
