using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float rotationSmoothTime = 0.05f;
    [SerializeField] float turnSpeed = 4;
    [SerializeField] float jumpTime = 1;
    [SerializeField, Range(0, 1)] float airControll = 1f;

    float m_speed;
    float m_timer = 0;
    float m_forceTime = 1;
    bool m_isGrounded;
    bool m_getForce;
    bool m_canJump;
    Vector3 m_force;
    float m_angle;
    float m_h, m_v;
    float m_turnSmoothVelocity;
    float m_sideways;
    float m_forward;
    float m_forceTimer = 0;
    

    public float JumpPercentage { get { return m_timer / jumpTime; } }
    public bool IsGrounded { get { return m_isGrounded; } }
    public bool CanJump { get { return m_canJump; } }
    public bool GetForce { get { return m_getForce; } }
    float m_velocityY;
    bool m_canMove = true;
    CharacterController m_CharacterController;
    


    //Get the CharacterController Component
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();

    }

    //Check for PlayerInput
    //Calculate Movement, Jump and Force
    void Update()
    {
            Mechanics();
    }

    private void Mechanics()
    {
        if (m_canMove)
        {
            PlayerInput();                  //Handle Player Input
            Jump();                         //Jump the Player
            Movement();                     //Calculate Movement and send to the CharacterController
            Jump();                         //Jump the Player
            ExecuteForce();                 //Execute Force
        }
    }

    private void ExecuteForce()
    {
        if (m_getForce)                                                 //while true
        {   
            float m_perc = m_forceTimer / m_forceTime;                  //calc percentage 0 -1
            Vector3 m_acc = Vector3.Lerp(m_force, Vector3.zero, m_perc);//Lerp from force-Vector to Vector3.Zero
            m_CharacterController.Move(m_acc * Time.deltaTime);         //add to characterController
            m_forceTimer += Time.deltaTime;                             //increase Timer,percantag can calculate
            if (m_forceTimer >= m_forceTime)                            //if timer is >= time
            {                                                           //stop force input and leave this method
                m_getForce = false;
            }
        }
    }

    private void PlayerInput()
    {
        m_h = Input.GetAxisRaw("Horizontal");                           
        m_v = Input.GetAxisRaw("Vertical");
    }

    void Movement()
    {
        Rotation();

        bool m_isRunning = Input.GetKey(KeyCode.LeftShift);             //while Key down run

        

        m_speed = m_isRunning ? runSpeed : walkSpeed;                   //if Key is Down speed = runSpeed else speed = walkSpeed
        m_sideways = m_h != 0 ? m_h * m_speed : 0;                      //Walk Left /right
        m_forward = m_v != 0 ? m_v * m_speed : 0;                       //walkd Forward/Backward
        m_velocityY += Time.deltaTime * gravity;                        //adds continuose Gravity
        var m_move = new Vector3(m_sideways, m_velocityY, m_forward);   //Create a Vector3 out of all Inputs
        m_move = !m_isGrounded ? new Vector3(m_move.x * airControll, m_move.y, m_move.z * airControll) : m_move;
        m_CharacterController.Move(transform.TransformDirection(m_move) * Time.deltaTime);  //add Vector to CharacterController
        


        if (m_isGrounded)                                               //is CharacterController is grounded
        {                                                               //dont add Gravity
            m_velocityY = 0;
        }
        
    }
    /// <summary>
    /// Reduce Speed
    /// </summary>
    /// <param name="value"> 0.5 half Speed</param>
    public void Modifier(float value)
    {
        m_speed *= value;
    }
    void Jump()
    {
        //If CharacterController is ground and Space is pressed
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_isGrounded = m_CharacterController.isGrounded;                //get if CharacterController is grounded or not
            if (!m_isGrounded) return;
            m_canJump = true;
            StartCoroutine(JumpCoroutine());
        }
    }
    IEnumerator JumpCoroutine()
    {
        m_timer = 0;
        while(m_timer < jumpTime)
        {
            float m_perc = m_timer / jumpTime;
            float m_jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            m_velocityY = Mathf.Lerp(m_jumpVelocity,gravity, m_perc);
            m_timer += Time.deltaTime;
            yield return null;
        }
        m_canJump = false;
    }
    void Rotation()
    {
        m_angle += Input.GetAxisRaw("Mouse X") * turnSpeed;
        float m_targetRotation = m_angle;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_turnSmoothVelocity, rotationSmoothTime);
    }
    public Vector3 Velocity()
    {
        return m_CharacterController.velocity;
    }
    public void AddForce(Vector3 direction,float force,float time)
    {
        m_forceTime = time;
        m_forceTimer = 0;
        m_getForce = true;
        var m_direction = direction.normalized;
        this.m_force = m_direction * force;
    }
}
