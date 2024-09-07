using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControllerWeb : MonoBehaviour
{
    public static PlayerControllerWeb instance;


    public POS PlayerPos = POS.Mid ;
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

    private CharacterController characterController;

    [HideInInspector]
    public Animator animation;

    //Input system
    public bool swipeUp;
    public bool swipeRight;
    public bool swipeLeft;
    public bool swipeDown;

    public HitXX hitx = HitXX.None;

    #region Awake,  Start and update      

    private void Awake()
    {
        instance = this;
    }
   
    void Start()
    {       
        transform.position = Vector3.zero;
        characterController = GetComponent<CharacterController>();
        ColHeight = characterController.height;
        ColCenterY = characterController.center.y;
        characterController.center = new Vector3(0, ColCenterY, 0);
        characterController.height = ColHeight;        
        animation = GetComponent<Animator>();
    }

    
    void Update()
    {
       
        PlayerMovements();
        Vector3 moveVector = new Vector3(x - transform.position.x, verticalVelocity * Time.deltaTime, 0);
        x = Mathf.Lerp(x, newPlayerPos, Time.deltaTime * DodgeSpeed);
        characterController.Move(moveVector);
        Jump();
        Roll();

    }

    #endregion

    #region Input system

    public void OnRight(InputValue value)
    {
        RightInput(value.isPressed);
    }
    public void RightInput(bool newJumpState)
    {
        swipeRight = newJumpState;
    }

    public void OnLeft(InputValue value)
    {
        LeftInput(value.isPressed);
    }
    public void LeftInput(bool newJumpState)
    {
        swipeLeft = newJumpState;
    }

    public void OnRoll(InputValue value)
    {
        RollInput(value.isPressed);
    }
    public void RollInput(bool newJumpState)
    {
        swipeDown = newJumpState;
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void JumpInput(bool newJumpState)
    {
        swipeUp = newJumpState;
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
                Debug.Log("Swiped Left from mid");
                newPlayerPos = -SwipeDistance;
                PlayerPos = POS.Left;
                //animation.Play("JumpLeft");
            }
            else if (PlayerPos == POS.Right)
            {
                Debug.Log("Swiped Left from Right");
                newPlayerPos = 0;
                PlayerPos = POS.Mid;
                //animation.Play("JumpLeft");
                
            }
            SoundManager.Instance.PlaySFX("JumpLeftRight");
            swipeLeft = false;
        }


        if (swipeRight)
        {
            InRoll= false;
            if (PlayerPos == POS.Mid)
            {
                Debug.Log("Swiped Right from mid");
                newPlayerPos = SwipeDistance;
                PlayerPos = POS.Right;
                //animation.Play("JumpRight");
                
            }
            else if (PlayerPos == POS.Left)
            {
                Debug.Log("Swiped Right from Left");
                newPlayerPos = 0;
                PlayerPos = POS.Mid;
                //animation.Play("JumpRight");
                
            }
            SoundManager.Instance.PlaySFX("JumpLeftRight");
            swipeRight = false;
        }

        
    }
    #endregion

    #region Jump and Roll

    private void Jump()
    {
        if(characterController.isGrounded)
        {
            if(animation.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            {                               
                animation.Play("Run");
                InJump = false;
            }
            if(swipeUp)
            {
                verticalVelocity = JumpPower;
                animation.CrossFadeInFixedTime("Jump", 0.1f);
                InJump = true;
                InRoll= false;
                SoundManager.Instance.PlaySFX("Jump");
            }
            swipeUp = false;

        }
        else
        {
            verticalVelocity -= JumpPower * 2 * Time.deltaTime;
            if(characterController.velocity.y < -0.1f)
            animation.SetTrigger("Fall");
        }
    }

    internal float RollCounter;
    public void Roll()
    {
        RollCounter -= Time.deltaTime;
        if(RollCounter <= 0f)
        {
            RollCounter = 0f;
            characterController.center = new Vector3(0, ColCenterY, 0);
            characterController.height = ColHeight;
            InRoll = false;
        }
        if(swipeDown && !InRoll)
        {
            RollCounter = 0.7f;
            verticalVelocity -= 10f;
            characterController.center = new Vector3(0, ColCenterY/2f, 0);
            characterController.height = ColHeight/2f;
            animation.CrossFadeInFixedTime("roll", 0.1f);
            InRoll= true;
            InJump= false;
            SoundManager.Instance.PlaySFX("Roll");
        }

        swipeDown = false;
        

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
