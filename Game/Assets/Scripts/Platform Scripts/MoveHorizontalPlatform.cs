using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveHorizontalPlatform : MonoBehaviour
{

    [SerializeField] private PlatformPath path;
    [SerializeField] private float waitLength;
    [SerializeField] private AnimationCurve easingCurve;
    [SerializeField] private float Duration;

    private float startTime;
    private float waitTimer;

    private enum State
    {
        Moving,
        Stopped
    };
    private State state;

    private Rigidbody rb;
    private int nextWaypoint = 1;

    // Start is called before the first frame update
    void Start()
    {

        startTime = Time.time;
        state = State.Moving;
        rb = GetComponent<Rigidbody>();

        // Week 4 code
        transform.position = path.Waypoint(0);
        Vector3 waypoint = path.Waypoint(1);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float t = (Time.time - startTime) / Duration;
        t = easingCurve.Evaluate(t);

        switch (state)
        {
            case State.Moving:
                //find the location and how far from location
                Vector3 waypoint = path.Waypoint(nextWaypoint);

                float distanceToWaypoint = Vector3.Distance(waypoint, transform.position);

                if (distanceToWaypoint > 0.1f)
                {
                    rb.MovePosition(Vector3.Lerp(rb.position, waypoint, t)); // move to waypoint
                }
        
                if (distanceToWaypoint <= 0.1f)
                {
                    // reached the waypoint, start heading to the next one
                    rb.velocity = Vector3.zero;
                    //rb.position = waypoint;
                    NextWaypoint();
                    waitTimer = waitLength;
                    state = State.Stopped;
                }
                break;

            case State.Stopped:
                rb.velocity = Vector3.zero;
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    startTime = Time.time;
                    state = State.Moving;
                }
                break;
        }

    }

    private void NextWaypoint()
    {

        nextWaypoint++;

        // Aim for the first waypoint if we have reached the end of the path.
        if (nextWaypoint == path.Length)
        {
            nextWaypoint = 0;
        }
    }
}
