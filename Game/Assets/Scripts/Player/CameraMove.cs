using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //based on week11 prac code.

    //following player parameters
    [SerializeField] private Transform player;
    [SerializeField] private float lerpValue = 1f;
    private Vector3 loc;
    
    //zoom scroll params. We don't have the Range Script installed, so it's two floats.
    //zoom isn't actually necessary according to the specification. If it's too jenk, just delete these.
    [SerializeField] private Transform cam;
    [SerializeField] private Transform vertPt;
    [SerializeField] private float scrollSeverity = 10f;
    [SerializeField] private float minZoom = -21;
    [SerializeField] private float maxZoom = -3;
    [SerializeField] private float zoom = -10;
    private float z;

    //horizontal
    private float x;
    public float X 
    {
        get
        {
            return x;
        }
    }

    [SerializeField] private float rotSmoothX = 1f;
    //vertical
    private float y;
    [SerializeField] private float rotSmoothY = 1f;
    [SerializeField] private float startY = 5.5f;

    // Start is called before the first frame update
    void Start()
    {

        //camera snaps to default values on Start()
        loc = player.transform.position;
        transform.eulerAngles = new Vector3(0, 0, 0);
        cam.transform.position = new Vector3(0, 0, zoom);
        vertPt.transform.localPosition = new Vector3(0, startY, 0);

    }

    // Update is called once per frame
    void Update()
    {

        //recieve values/inputs
        x = Input.GetAxis(InputAxes.MsX);
        y = Input.GetAxis(InputAxes.MsY);
        z = Input.GetAxis(InputAxes.Scroll);
        if (player != null)
        {
            loc = player.transform.position;
        }
        //snap to player position
        
        transform.position = Vector3.Lerp(transform.position, loc, lerpValue);
        //Move camera to scroll
        cam.transform.Translate(Vector3.forward * z * scrollSeverity, Space.Self);
        //Apply comstraints
        zoom = cam.transform.localPosition.z;
        zoom = Mathf.Min(zoom, maxZoom);
        zoom = Mathf.Max(zoom, minZoom);
        cam.transform.localPosition = new Vector3(vertPt.transform.localPosition.x, vertPt.transform.localPosition.y, zoom);

        //drag the mouse to rotate camera pivot.
            transform.eulerAngles +=  Vector3.Lerp(Vector3.zero, Vector3.up*x, rotSmoothX);
            transform.eulerAngles +=  Vector3.Lerp(Vector3.zero, Vector3.left*y, rotSmoothY);

    }

    public void setPlayer(Transform p)
    {

        player = p;

    }
}
