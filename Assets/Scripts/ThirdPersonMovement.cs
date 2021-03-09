using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header(" Constant values ")]
    [Range(-10,-50)] public float gravity = -9.81f;
    [Range (0.5f,3)] public float jumpHight=3f;
    [Range(6,15)] public float speed = 6f;


    public CharacterController controller;
    public Transform cam;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;


    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 currentVelocity;

    void Update()
    {
        isGrounded = Physics.CheckSphere ( groundCheck.position , groundDistance , groundMask );

        float horizontal = Input.GetAxisRaw ( "Horizontal" );    
        float vertical = Input.GetAxisRaw ( "Vertical" );

        Vector3 direction = new Vector3 ( horizontal , 0f , vertical ).normalized;
    
        if ( direction.magnitude >= 0.1f )
        {
            float targetAngle = Mathf.Atan2 ( direction.x , direction.z )* Mathf.Rad2Deg+ cam.eulerAngles.y;
            print ( direction );
            float angle = Mathf.SmoothDampAngle ( transform.eulerAngles.y , targetAngle , ref turnSmoothVelocity , turnSmoothTime );
            transform.rotation = Quaternion.Euler ( 0f , angle , 0f );


            Vector3  moveDir = Quaternion.Euler ( 0f , targetAngle , 0f )*Vector3.forward;
            controller.Move ( moveDir.normalized * speed * Time.deltaTime );
        }

        if ( isGrounded && currentVelocity.y<0 )
        {
            currentVelocity.y = -2f;

        }
        currentVelocity.y += gravity * Time.deltaTime;

        controller.Move ( currentVelocity * Time.deltaTime );

        if ( Input.GetButtonDown ( "Jump" ) && isGrounded )
        {
            currentVelocity.y = Mathf.Sqrt ( jumpHight * gravity * -2 );
        }
    }
}
