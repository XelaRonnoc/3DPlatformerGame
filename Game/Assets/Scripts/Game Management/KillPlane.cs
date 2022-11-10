using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    // OnTriggerEnter is called when a collider intersects.
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 7)
        {
            other.gameObject.GetComponent<ForcePlayerMove>().killPlayer();
        }

    }
    
}
