using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bouncyness = 30f;

    private Vector3 bounce;
    private float heightOffset = 0.25f; // ensures player only hits from above
    void Start()
    {
        bounce = new Vector3(0, bouncyness, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == 7) // player
        {
            if (collision.transform.position.y > transform.position.y + heightOffset) // check is from above
            {
                collision.rigidbody.AddForce(bounce, ForceMode.Impulse);
            }
        }

    }

}
