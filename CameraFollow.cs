using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject CameraFollowObject;

    public float CameraMoveSpeed = 120.0f;
    public float ClampAngleUp = 80.0f;
    public float ClampAngleDown = 60.0f;
    public float InputSensitivity = 150.0f;
    public float MouseX;
    public float MouseY;
    public float FinalInputX;
    public float FinalInputZ;

    private float RotationY = 0.0f;
    private float RotationX = 0.0f;

    // Start is called before the first frame update
    void Start(){
        Vector3 rot = transform.localRotation.eulerAngles;
        RotationY = rot.y;
        RotationX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update(){
        float InputX = Input.GetAxis("Joystick X");
        float InputZ = Input.GetAxis("Joystick Z");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        FinalInputX = InputX + MouseX;
        FinalInputZ = InputZ + MouseY;

        RotationY += FinalInputX * InputSensitivity * Time.deltaTime;
        RotationX += FinalInputZ * InputSensitivity * Time.deltaTime;

        RotationX = Mathf.Clamp(RotationX, -ClampAngleDown, ClampAngleUp);

        Quaternion localRotation = Quaternion.Euler(RotationX, RotationY, 0.0f);
        transform.rotation = localRotation;
    }
    void LateUpdate(){
    	CameraUpdater();

    }
    void CameraUpdater(){
    	Transform target = CameraFollowObject.transform;

    	float step = CameraMoveSpeed * Time.deltaTime;
    	transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
