using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerControllerNew : MonoBehaviour
{
    public static PlayerControllerNew instance;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    public POS PlayerPos = POS.Mid;
    float newPlayerPos = 0f;
    public float SwipeDistance = 2f;
    private float x;
    public float DodgeSpeed = 5f;
    public float JumpPower = 5f;
    private float verticalVelocity;
    public bool InJump;
    public bool InRoll;
    private float ColHeight;
    private float ColCenterY;

    [HideInInspector]
    public GameObject buttonUI;

    public CharacterController characterController;

    public PowerUpController powerUpController;
    public GameObject playerCamera;

    [HideInInspector]
    public Animator _animation;

    [Header("Movement")]
    public Vector3 moveDirection;
    public float runSpeed = 0.1f;

    [Header("Control Mode")]
    public ControlMode mode;

    //For Touch
    [Header("For TouchControls")]
    public bool isGrounded;
    private bool isSliding = false;

    public bool startTouch = true;

    private Vector3 velocity;
    private Vector3 move;

    public LayerMask groundLayer;
    public Transform groundCheck;

    public float moveSpeed;
    public float forwardSpeed;
    public float gravity = -12f;
    public float jumpHeight = 2;
    public float dodgeSpeed = 0.5f;
    public float trapDetectRadius = 5f;

    public float slideDuration = 1.5f;

    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 2f;//The distance between tow lanes
    //End Touch

    [Header("For InputSystem")]
    //Input system
    public bool swipeUp;
    public bool swipeRight;
    public bool swipeLeft;
    public bool swipeDown;
    public bool isDead;

    public HitXX hitx = HitXX.None;

    //  public GameObject VirtualCam;

    #region Awake,  Start and update      

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(GameManager.instance != null)
            mode = GameManager.instance.controlMode;
        //       InvokeRepeating("SetPlayerPosition", 2f, 10f);

        //touchField = GameObject.Find("Touch").GetComponent<FixedTouchField>();
        //buttonUI = GameObject.Find("PlayerCanavas").gameObject;
        //transform.position = Vector3.zero;
        ColHeight = characterController.height;
        ColCenterY = characterController.center.y;
        characterController.center = new Vector3(0, ColCenterY, 0);
        characterController.height = ColHeight;
        _animation = GetComponent<Animator>();
        powerUpController = GetComponent<PowerUpController>();
    }

    internal void Revive()
    {
        isDead = false;
        _animation.Play("Run");
        powerUpController.StartPower(1);

        UIScript uiScript = UIScript.Instance;

        Time.timeScale = 1;
        StartCoroutine(LevelManager.instance.SpeedIncrease());
        uiScript.gameOverPanel.SetActive(false);

#if UNITY_WEBGL
        uiScript.revivePanelWeb.SetActive(false);
#else
        uiScript.revivePanel.SetActive(false);
#endif

        uiScript.buff.transform.SetParent(transform);
        uiScript.buff.transform.position = transform.position;
        startTouch = true;

        Collider[] traps = Physics.OverlapSphere(transform.position, trapDetectRadius);

        foreach(Collider trap in traps)
        {
            if(trap.gameObject.tag == "Traps")
            {
                Destroy(trap.gameObject);
            }
        }
    }

    /*   void SetPlayerPosition()
       {


           this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0f);

       //    Debug.Log("mainCam=======" + VirtualCam.transform.position);

       }*/

    private void FixedUpdate()
    {
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0f);
        //Debug.Log("Player Position==============" + this.transform.position.z);
    }

    void Update()
    {



        if (mode == ControlMode.KEYBOARD && !isDead)
        {
            GameManager.instance.ShowDubug("CallKeyMove");
            PlayerMovements();
            //Vector3 moveVector = new Vector3(x - transform.position.x, verticalVelocity * Time.deltaTime, moveSpeed * Time.deltaTime);
            x = Mathf.Lerp(x, newPlayerPos, Time.deltaTime * DodgeSpeed);
            //Debug.Log("This is moveVector " + moveVector);
            //characterController.Move(new Vector3(moveVector.x, moveVector.y, moveSpeed * Time.deltaTime));
            Vector3 moveVector = new Vector3(x - transform.position.x, verticalVelocity * Time.deltaTime, moveSpeed * Time.deltaTime);

            characterController.Move(moveVector);
            Jump();
            Roll();
        }

        if (mode == ControlMode.TOUCH && !isDead)
        {
            if (startTouch == true)
            {
                _animation.SetBool("isGameStarted", true);
                move.z = moveSpeed;

                isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
                _animation.SetBool("isGrounded", isGrounded);

                //              Debug.Log("Value of isGrounded=======" + isGrounded);
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -1f;
                    //           Debug.Log("Value of isGrounded inside=======" + isGrounded);
                }
                if (isGrounded)
                {

                    if (_animation.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
                    {
                        _animation.Play("Run");
                        //  InJump = false;
                    }
                    //               Debug.Log("In Jump");
                    if (SwipeManager.swipeUp)
                    {
                        //                   Debug.Log("In Jump 1");
                        JumpTouch();
                    }

                    if (SwipeManager.swipeDown && !isSliding)
                    {

                        characterController.center = new Vector3(0, ColCenterY / 2f, 0);
                        characterController.height = ColHeight / 2f;
                        StartCoroutine(Slide());
                        /*        characterController.center = new Vector3(0, ColCenterY, 0);
                                characterController.height = ColHeight;*/
                        //GameManager.instance.ShowDubug("Coming in slide");
                    }
                }
                else
                {
                    //GameManager.instance.ShowDubug("In jump else");
                    velocity.y += gravity * Time.deltaTime;
                    _animation.SetTrigger("Fall");
                    if (SwipeManager.swipeDown && !isSliding)
                    {
                        //GameManager.instance.ShowDubug("Coming in Slide down there 1");
                        StartCoroutine(Slide());
                        velocity.y = -10;


                        //            Debug.Log("Coming in Slide down there");
                    }

                }
                characterController.Move(velocity * Time.deltaTime);

                //Gather the inputs on which lane we should be
                if (SwipeManager.swipeRight)
                {
                    //GameManager.instance.ShowDubug("Swiping right");
                    desiredLane++;
                    if (desiredLane == 3)
                        desiredLane = 2;
                    //_animation.Play("JumpRight");
                }
                if (SwipeManager.swipeLeft)
                {
                    desiredLane--;
                    if (desiredLane == -1)
                        desiredLane = 0;
                    //  _animation.Play("JumpLeft");
                }

                //Calculate where we should be in the future
                Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
                if (desiredLane == 0)
                    targetPosition += Vector3.left * laneDistance;
                else if (desiredLane == 2)
                    targetPosition += Vector3.right * laneDistance;

                //transform.position = targetPosition;
                if (transform.position != targetPosition)
                {
                    Vector3 diff = targetPosition - transform.position;
                    Vector3 moveDir = diff.normalized * 30 * dodgeSpeed * Time.deltaTime;
                    if (moveDir.sqrMagnitude < diff.magnitude)
                        characterController.Move(moveDir);
                    else
                        characterController.Move(diff);
                }

                characterController.Move(move * Time.deltaTime);
            }


            //Android 
            /*    if (Application.platform == RuntimePlatform.Android)
                {
                    if (startTouch == true)
                    {

                        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
                        {
                            Debug.Log("Inside began");
                            startTouchPosition = Input.GetTouch(0).position;
                        }

                        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended)
                        {

                            Debug.Log("Coming in");
                            endTouchPosition = Input.GetTouch(0).position;




                            if (endTouchPosition.y > startTouchPosition.y)

                            {
                                //jump
                                swipeUp = true;

                                Debug.Log("UPJUMP======" + endTouchPosition.y);
                                //         UPJump();

                            }

                            if (endTouchPosition.x > startTouchPosition.x)

                            {
                                //       Debug.Log("Coming in Left====");

                                swipeRight = true;

                            }

                            if (endTouchPosition.x < startTouchPosition.x)

                            {


                                swipeLeft = true;


                            }



                            if (endTouchPosition.y < startTouchPosition.y)
                            {
                                swipeDown = true;


                            }
                        }
                    }
                }*/
        }
    }

    #endregion

    #region Android Methods
    private void JumpTouch()
    {
        StopCoroutine(Slide());
        //     _animation.SetBool("isSliding", false);
        //   _animation.SetTrigger("Jump");
        _animation.CrossFadeInFixedTime("Jump", 0.1f);
        //      controller.center = Vector3.zero;
        //     characterController.height = 2;

        characterController.height = 1.8f;
        isSliding = false;

        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);

        //      isGrounded = true;
        //GameManager.instance.ShowDubug("In Jump 2");
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        //     _animation.SetBool("isSliding", true);

        _animation.CrossFadeInFixedTime("roll", 0.1f);



        //     characterController.height = ColHeight/2;
        SoundManager.Instance.PlaySFX("Roll");
        yield return new WaitForSeconds(0.25f / Time.timeScale);
        //    characterController.center = new Vector3(0, -0.5f, 0);
        //     characterController.height = 1;

        yield return new WaitForSeconds((slideDuration - 0.25f) / Time.timeScale);

        //    animator.SetBool("isSliding", false);

        characterController.center = new Vector3(0, ColCenterY, 0);
        characterController.height = ColHeight;

        //      controller.center = Vector3.zero;
        // controller.height = 2;

        //    characterController.height = 1.8f;



        isSliding = false;

        //   isGrounded = true;


        //GameManager.instance.ShowDubug("Coming in slide 1");
    }
    #endregion

    #region Input system

    public void OnRight(InputValue value)
    {
        RightInput(value.isPressed);
        //GameManager.instance.ShowDubug("RightA");
    }
    public void RightInput(bool newJumpState)
    {
        swipeRight = newJumpState;
        //GameManager.instance.ShowDubug("RightB");
    }

    public void OnLeft(InputValue value)
    {
        LeftInput(value.isPressed);
        //GameManager.instance.ShowDubug("LeftA");
    }
    public void LeftInput(bool newJumpState)
    {
        swipeLeft = newJumpState;
        //GameManager.instance.ShowDubug("LeftB");
    }

    public void OnRoll(InputValue value)
    {
        RollInput(value.isPressed);
        //GameManager.instance.ShowDubug("RollA");
    }
    public void RollInput(bool newJumpState)
    {
        swipeDown = newJumpState;
        //GameManager.instance.ShowDubug("RollB");
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
        //GameManager.instance.ShowDubug("JumpA");
    }
    public void JumpInput(bool newJumpState)
    {
        swipeUp = newJumpState;
        //GameManager.instance.ShowDubug("JumpB");
    }

    #endregion

    #region Player movements 

    #region Left Right 
    public void PlayerMovements()
    {
        if (swipeLeft)
        {
            InRoll = false;
            if (PlayerPos == POS.Mid)
            {
                GameManager.instance.ShowDubug("Swiped Left from mid");
                newPlayerPos = -SwipeDistance;
                PlayerPos = POS.Left;
                //animation.Play("JumpLeft");
            }
            else if (PlayerPos == POS.Right)
            {
                GameManager.instance.ShowDubug("Swiped Left from Right");
                newPlayerPos = 0;
                PlayerPos = POS.Mid;
                //animation.Play("JumpLeft");

            }
            if(SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("JumpLeftRight");
            swipeLeft = false;
        }


        if (swipeRight)
        {
            InRoll = false;
            if (PlayerPos == POS.Mid)
            {
                GameManager.instance.ShowDubug("Swiped Right from mid");
                newPlayerPos = SwipeDistance;
                PlayerPos = POS.Right;
                //animation.Play("JumpRight");

            }
            else if (PlayerPos == POS.Left)
            {
                GameManager.instance.ShowDubug("Swiped Right from Left");
                newPlayerPos = 0;
                PlayerPos = POS.Mid;
                //animation.Play("JumpRight");

            }
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("JumpLeftRight");
            swipeRight = false;
        }


    }
    #endregion

    #region Jump and Roll

    private void Jump()
    {
        GameManager.instance.ShowDubug("CallB");
        if (characterController.isGrounded)
        {
            GameManager.instance.ShowDubug("CallB2B");
            if (_animation.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            {
                _animation.Play("Run");
                InJump = false;
            }
            if (swipeUp)
            {
                swipeUp = false;
                verticalVelocity = JumpPower;
                _animation.CrossFadeInFixedTime("Jump", 0.1f);
                InJump = true;
                InRoll = false;
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlaySFX("Jump");
            }

        }
        else
        {
            GameManager.instance.ShowDubug("CallB2C");
            verticalVelocity -= JumpPower * 2 * Time.deltaTime;
            if (characterController.velocity.y < -0.1f)
                _animation.SetTrigger("Fall");
        }
    }

    internal float RollCounter;
    public void Roll()
    {
        RollCounter -= Time.deltaTime;
        if (RollCounter <= 0f)
        {
            RollCounter = 0f;
            characterController.center = new Vector3(0, ColCenterY, 0);
            characterController.height = ColHeight;
            InRoll = false;

            GameManager.instance.ShowDubug("False Roll");
        }
        if (swipeDown && !InRoll)
        {
            swipeDown = false;
            RollCounter = 0.7f;
            verticalVelocity -= 10f;
            characterController.center = new Vector3(0, ColCenterY / 2f, 0);
            characterController.height = ColHeight / 2f;

            //  characterController.height = 0.67f;
            _animation.CrossFadeInFixedTime("roll", 0.1f);
            InRoll = true;
            InJump = false;
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("Roll");

            GameManager.instance.ShowDubug("true Roll");
        }



    }

    #endregion

    #endregion

    #region Collision Detection

    #region Method 1

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Player Should Die");
    //    if (collision.gameObject.CompareTag("Death"))
    //    {
    //        Debug.Log("Player Die");
    //        animation.Play("Die");
    //    }
    //}

    #endregion

    //public void OnCharacterColliderHit(Collider col)
    //{
    //    hitx = GetHitXX(col);

    //}
    //public HitXX GetHitXX(Collider col)
    //{
    //    Bounds PlayerBounds = characterController.bounds;
    //    Bounds colliderBounds = col.bounds;
    //    float min_x = Mathf.Max(colliderBounds.min.x, PlayerBounds.min.x);
    //    float max_x = Mathf.Min(colliderBounds.max.x, PlayerBounds.max.x);
    //    float average = (min_x + max_x) / 2f - colliderBounds.min.x;
    //    HitXX hit;
    //    if (average > colliderBounds.size.x - 0.33f)
    //    {
    //        hit = HitXX.Right;
    //    }
    //    else if (average < 0.33f)
    //    {
    //        hit = HitXX.Left;
    //    }
    //    else
    //    {
    //        hit = HitXX.Mid;
    //    }
    //    return hit;
    //}

    #endregion
}
