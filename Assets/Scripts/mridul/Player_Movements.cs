using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum SIDE { Left = -2, Mid = 0 , Right = 2}
public enum HitX { Left, Mid, Right, None}
public enum HitY { Up, Mid, Down, Low, None}
public enum HitZ { Forward, Mid, Backward, None}
public class Player_Movements : MonoBehaviour
{
    #region Singleton
    public static Player_Movements instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    public SIDE m_Side = SIDE.Mid;    
    [HideInInspector] public bool SwipeRight, SwipeLeft, SwipeUp, SwipeDown;        
    private CharacterController m_char;
    private Animator m_Animator;
    private float x;
    public float speedDodge;  
    public float jumpPower = 7f;
    private float y;
    public bool InJump;
    public bool InRoll;
    private float ColliderHeight;
    private float ColliderCenterY;

    public HitX hitx = HitX.None;
    public HitY hity = HitY.None;
    public HitZ hitz = HitZ.None;
    private SIDE LastSide;

    // Input System
    public bool jump;
    public bool right;
    public bool left;
    public bool roll;

    #region Start And Update
    void Start()
    {
        transform.position = Vector3.zero;        
        m_char = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        ColliderHeight = m_char.height;
        ColliderCenterY = m_char.center.y;
    }
    
    void Update()
    {
      
        if (left && !InRoll)
        {

            if (m_Side == SIDE.Mid)
            {
                Debug.Log("ON Swipe Left 01");
                LastSide = m_Side;
                m_Side = SIDE.Left;
                m_Animator.Play("Standing Dodge Left");
            }
            else if(m_Side == SIDE.Right)
            {
                Debug.Log("ON Swipe Left 02");
                LastSide = m_Side;
                m_Side = SIDE.Mid;                
                m_Animator.Play("Standing Dodge Left");
            }
            else
            {
                LastSide = m_Side;
                m_Animator.Play("Stumble_Left");
            }
            left = false;
        }
         
        if (right && !InRoll)
        {

            if (m_Side == SIDE.Mid)
            {
                Debug.Log("ON Swipe Right 01");
                LastSide = m_Side;
                m_Side = SIDE.Right;
                m_Animator.Play("Standing Dodge Right");
            }
            else if (m_Side == SIDE.Left)
            {
                Debug.Log("ON Swipe Right 02");
                LastSide = m_Side;
                m_Side = SIDE.Mid;
                m_Animator.Play("Standing Dodge Right");
            }
            else
            {
                LastSide = m_Side;
                m_Animator.Play("Stumble_Right");
            }
            right = false;
        }
      
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, 0);
        x = Mathf.Lerp(x, (int)m_Side, Time.deltaTime * speedDodge);
        m_char.Move(moveVector);
        
        Jump();
        Roll();
    }
    #endregion

    #region Input System 
    public void OnRight(InputValue value)
    {
        RightInput(value.isPressed);
    }
    public void RightInput(bool newJumpState)
    {
        right = newJumpState;
    }

    public void OnLeft(InputValue value)
    {
        LeftInput(value.isPressed);
    }
    public void LeftInput(bool newJumpState)
    {
        left = newJumpState;
    }

    public void OnRoll(InputValue value)
    {
        RollInput(value.isPressed);
    }
    public void RollInput(bool newJumpState)
    {
        roll = newJumpState;
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    #endregion

    #region Jump And Roll
    public void Jump()
    {
        if(m_char.isGrounded)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("InAir"))
            {
                m_Animator.Play("JumpLand");
            }
            if(jump)
            {
                y = jumpPower;
                m_Animator.CrossFadeInFixedTime("JumpStart", 0.1f);
                InJump = true;
            }            
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            if (m_char.velocity.y < -0.1f)
            //m_Animator.Play("InAir");
            m_Animator.SetTrigger("InAir");
        }
        jump = false;
    }

    internal float RollCounter;
    public void Roll()
    {
        RollCounter -= Time.deltaTime;
        if(RollCounter <= 0f)
        {
            RollCounter = 0f;
            m_char.center = new Vector3(0, ColliderCenterY, 0);
            m_char.height = ColliderHeight;
            InRoll = false;
        }
        if (roll && !InRoll)
        {
            RollCounter = 0.8f;
            y -= 10f;
            if (!InJump)
            {
                m_char.center = new Vector3(0, ColliderCenterY / 2f, 0);
                m_char.height = ColliderHeight / 2f;
            }
            m_Animator.CrossFadeInFixedTime("roll", 0.1f);
            //m_Animator.SetBool("rolling", true);
            InRoll = true;           
            InJump = false;

        }
        roll= false;
    }

    #endregion

    #region collision Detection
    public void OnCharacterColliderHit(Collider col)
    {
        hitx = GetHitX(col);
        hity = GetHitY(col);
        hitz = GetHitZ(col);

        if(hitz == HitZ.Forward && hitx == HitX.Mid)  // Death 
        {
            if(hity == HitY.Low)
            {
                m_Animator.Play("Stumble_straight1");
                ResetCollision();
            }
            else if (hity == HitY.Down)
            {
                m_Animator.Play("Dam_StandDie");
                ResetCollision();
            }
            else if (hity == HitY.Mid)
            {
                if (col.tag == "MovingTrain")
                {
                    m_Animator.Play("Dam_FlyDie_Right");
                    ResetCollision();
                }
                else if (col.tag != "ramp")
                {
                    m_Animator.Play("Stumble_straight1");
                    ResetCollision();
                }
            }
            else if (hity == HitY.Up && !InRoll)
            {
                m_Animator.Play("Stumble_straight2");
                ResetCollision();
            }
        }
        else if(hitz == HitZ.Mid)
        {
            if(hitx == HitX.Right)
            {
                m_Side = LastSide;
                m_Animator.Play("stumbleRight");
                ResetCollision();
            }
            else if( hitx == HitX.Left)
            {
                m_Side = LastSide;
                m_Animator.Play("stumbleLeft");
                ResetCollision();
            }
        }
        else
        {
            if (hitx == HitX.Right)
            {
                m_Animator.Play("Stand_Stumble");
                ResetCollision();
            }
            else if (hitx == HitX.Left)
            {
                m_Animator.Play("Stand_Stumble");
                ResetCollision();
            }
        }
    }

    private void ResetCollision()
    {
        print(hitx.ToString() + hity.ToString() + hitz.ToString());
        hitx = HitX.None;
        hity = HitY.None;
        hitz = HitZ.None;

    }

    public HitX GetHitX(Collider col)
    {
        Bounds char_bounds = m_char.bounds;
        Bounds col_bounds = col.bounds;

        float min_x = Mathf.Max(col_bounds.min.x, char_bounds.min.x);
        float max_x = Mathf.Min(col_bounds.max.x, char_bounds.max.x);
        float average = (min_x + max_x) / 2f - col_bounds.min.x;
        HitX hit;
        if(average> col_bounds.size.x - 0.33f)
        {
            hit = HitX.Right;
        }
        else if( average < 0.33f)
        {
            hit = HitX.Left;
        }
        else
        {
            hit = HitX.Mid;
        }
        return hit;

    }

    public HitY GetHitY(Collider col)
    {
        Bounds char_bounds = m_char.bounds;
        Bounds col_bounds = col.bounds;

        float min_y = Mathf.Max(col_bounds.min.y, char_bounds.min.y);
        float max_y = Mathf.Min(col_bounds.max.y, char_bounds.max.y);
        float average = ((min_y + max_y) / 2f - col_bounds.min.y) / char_bounds.size.y;
        HitY hit;
        if (average < 0.17f)
        {
            hit = HitY.Low;
        }
        else if (average <  0.33f)
        {
            hit = HitY.Down;
        }
        else if (average < 0.66f)
        {
            hit = HitY.Mid;
        }
        else
        {
            hit = HitY.Up;
        }
        return hit;

    }

    public HitZ GetHitZ(Collider col)
    {
        Bounds char_bounds = m_char.bounds;
        Bounds col_bounds = col.bounds;

        float min_z = Mathf.Max(col_bounds.min.z, char_bounds.min.z);
        float max_z = Mathf.Min(col_bounds.max.z, char_bounds.max.z);
        float average = ((min_z + max_z) / 2f - col_bounds.min.z) / char_bounds.size.z;
        HitZ hit;
        if (average < 0.33f)
        {
            hit = HitZ.Backward;
        }
        else if (average < 0.66f)
        {
            hit = HitZ.Mid;
        }
        else
        {
            hit = HitZ.Backward;
        }
        return hit;

    }
    #endregion
}

