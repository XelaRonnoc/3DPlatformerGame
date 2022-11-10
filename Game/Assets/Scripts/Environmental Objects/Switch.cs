using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] Laser laser;
    [SerializeField] GameObject[] crystals = new GameObject[5];
    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;

    private void LateUpdate()
    { 
        
        foreach(GameObject obj in crystals)
        {
            if (laser.GetIsOn())
            {
                obj.GetComponent<Renderer>().material = onMaterial;
            }
            else
            {
                obj.GetComponent<Renderer>().material = offMaterial;
            } 
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == 7)
        {
            laser.ToggleOnOff();
        }

    }
}
