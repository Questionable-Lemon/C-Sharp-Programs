using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    private Vector3 dollyDirection;
    private Vector3 desiredCameraPosition;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    public float hitOffset = 0.5f;
    public float distance;

    private bool RaycastHit;

    void Awake()
    {
        distance = transform.localPosition.magnitude;
        dollyDirection = transform.localPosition.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        desiredCameraPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);

        RaycastHit hit;

        RaycastHit = Physics.Linecast(transform.parent.position, desiredCameraPosition, out hit);

        if(RaycastHit){
        	distance = Mathf.Clamp((hit.distance * hitOffset), minDistance, maxDistance);
        }
        else{
        	distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smooth);
    }
}

