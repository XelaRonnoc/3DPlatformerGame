using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Flags]
    public enum RotationDirection
    {
        None,
        Horizontal = (1 << 0),
        Vertical = (1 << 1),
    }

    [SerializeField] private RotationDirection rotationDirections;
    [SerializeField] private Vector2 lookSensitivity;
    [SerializeField] private Vector2 acceleration;
    [SerializeField] private float inputLagPeriod;
    [SerializeField] private float maxVerticalAngleFromHorizon;

    private Vector2 lookRotation;
    private Vector2 velocity;
    private Vector2 lastInputEvent;
    private float inputLagTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 camRotationVelo = GetLookInput() * lookSensitivity;

        if((rotationDirections & RotationDirection.Horizontal) == 0){
            camRotationVelo.x = 0;
        }
        if((rotationDirections & RotationDirection.Vertical) == 0)
        {
            camRotationVelo.y = 0;
        }

        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, camRotationVelo.x, acceleration.x * Time.deltaTime),
            Mathf.MoveTowards(velocity.y, camRotationVelo.y, acceleration.y * Time.deltaTime)
            );
        lookRotation += velocity * Time.deltaTime;
        lookRotation.y = ClampVerticalAngle(lookRotation.y);

        transform.localEulerAngles = new Vector3(lookRotation.y, lookRotation.x, 0);
    }

    private Vector2 GetLookInput()
    {
        inputLagTimer += Time.deltaTime;
        Vector2 input = new Vector2(Input.GetAxis(InputAxes.MsX), Input.GetAxis(InputAxes.MsY));

        if ((Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) == false || inputLagTimer >= inputLagPeriod)
        {
            lastInputEvent = input;
            inputLagTimer = 0;
        }
        return lastInputEvent;

    }

    private float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxVerticalAngleFromHorizon, maxVerticalAngleFromHorizon);
    }

    private void OnEnable()
    {
        // reset the state 
        velocity = Vector2.zero;
        inputLagTimer = 0;
        lastInputEvent = Vector2.zero;

        // calculate the current rotation by getting the game object local euler angles
        Vector3 euler = transform.localEulerAngles;
        if(euler.x >= 180)
        {
            euler.x -= 360;
        }
        euler.x = ClampVerticalAngle(euler.x);

        transform.localEulerAngles = euler;
        lookRotation = new Vector2(euler.y, euler.x);

    }
}
