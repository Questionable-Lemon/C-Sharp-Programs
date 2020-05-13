using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

   	public CharacterController controller;

   	public Transform IsOnGround;
   	public Transform Camera;

   	public LayerMask GroundMask;

   	private bool IsGrounded;
   	private bool IsBigRotating = false;

   	public float SlowRotationAngle = 90.0f;
   	public float VerticalVelocityCancel = -2.0f;
   	public float GroundCheckRadius = 0.4f;
   	public float gravity = -9.81f;
    public float MovementSpeed = 10.0f;
    public float RotationSpeed = 4.0f;
    public float ACCELERATION = 6.0f;
    public float DE_ACCELERATION = 7.0f;
    private float accel;

    private Vector3 VerticalVelocity;
    private Vector3 HorizontalVelocity;
    private Vector3 Direction;
    private Vector3 NewPos;

    private Quaternion RotationTargetQ;

    // Start is called before the first frame update
    private void Start(){
        VerticalVelocity = new Vector3(0f, 0f, 0f);
        Direction = new Vector3(0f, 0f, 0f);
        HorizontalVelocity = new Vector3(0f, 0f, 0f);
        RotationTargetQ = transform.rotation;
    }

    // Update is called once per frame
    private void Update(){


    	//Movements and rotations
    	IsGrounded = Physics.CheckSphere(IsOnGround.position, GroundCheckRadius, GroundMask);

    	if(IsGrounded)
    		VerticalVelocity[1] = VerticalVelocityCancel;

    	Direction = Vector3.zero;

    	Direction += Input.GetAxis("Horizontal") * Camera.right;
    	Direction += Input.GetAxis("Vertical") * Camera.forward;
    	
        if(Input.GetKey(KeyCode.W)){
        	Direction += Camera.forward;
        }
        if(Input.GetKey(KeyCode.S)){
        	Direction -= Camera.forward;
        }
        if(Input.GetKey(KeyCode.D)){
        	Direction += Camera.right;
        }
        if(Input.GetKey(KeyCode.A)){
        	Direction -= Camera.right;
        }

        Direction[1] = 0;
        Direction.Normalize();

        //If moving, start rotation towards movement
        if(Direction != Vector3.zero){
        	RotationTargetQ = Quaternion.LookRotation(Direction);
        }
        if(Quaternion.Angle(transform.rotation, RotationTargetQ) > SlowRotationAngle){
        	IsBigRotating = true;
        }

        VerticalVelocity[1] += Time.deltaTime * gravity;

        HorizontalVelocity[1] = 0f;

        NewPos = Direction * MovementSpeed;
        accel = DE_ACCELERATION;

        if(Vector3.Dot(Direction, HorizontalVelocity) > 0)
        	accel = ACCELERATION;

        //Moving vertically and horizontally
       	if(!IsBigRotating){
        HorizontalVelocity = Vector3.Lerp(HorizontalVelocity, NewPos, accel * Time.deltaTime);
        controller.Move(VerticalVelocity*Time.deltaTime);
        controller.Move(HorizontalVelocity*Time.deltaTime);
    	}
    	else{
    		IsBigRotating = false;
    	}

        //Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, RotationTargetQ, Time.deltaTime * RotationSpeed);
    }
}
