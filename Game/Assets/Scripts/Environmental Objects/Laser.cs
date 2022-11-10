using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform laserBeam;
    [SerializeField] private float laserLength;
    [SerializeField] private bool isOn = false;

    private Vector3 laserScaler;
    private Vector3 beamHeight = new Vector3(0, 0.15f, 0);

    private const float offsetFromRayToBeam = 1f;
    // Start is called before the first frame update
    void Start()
    {

        laserScaler = new Vector3(0f, laserLength- offsetFromRayToBeam, 0f);
        laserBeam.Translate(laserScaler, Space.Self); // ensure laser beam is always at front of lasergun
        laserBeam.localScale = laserBeam.localScale + laserScaler; // scales the laser beam to specified length
        laserBeam.gameObject.SetActive(isOn); 

    }

    // Update is called once per frame
    void Update()
    {

        laserBeam.gameObject.SetActive(isOn);
        if (isOn)
        {
            RaycastHit laser;
            if(Physics.Raycast(transform.position + beamHeight, transform.forward,out laser, laserLength * 2))
            {
                if(laser.collider.gameObject.layer == 7) // player layer
                {

                    laser.collider.gameObject.GetComponent<ForcePlayerMove>().killPlayer();

                }  
            }
        }

    }

    public void ToggleOnOff()
    {

        if (isOn)
        {
            isOn = false;
        }
        else
        {
            isOn = true;
        }

    }

    public bool GetIsOn()
    {

        return isOn;

    }
}


