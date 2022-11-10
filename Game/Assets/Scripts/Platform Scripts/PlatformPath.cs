using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPath : MonoBehaviour
{
    public int Length
    {
        get
        {
            return transform.childCount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        //Make list of Waypoint positions, based on Week 4 prac
        Vector3[] waypoints = new Vector3[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i).position;
        }

    }

    public Vector3 Waypoint(int i)
    {

        return transform.GetChild(i).position;

    }
 
    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;

        Vector3 last = transform.GetChild(0).position;
        for (int i = 1; i < transform.childCount; i++)
        {
            Vector3 next = transform.GetChild(i).position;
            Gizmos.DrawLine(last, next);
            last = next;
        }
        
    }
}
