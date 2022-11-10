using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] Material teleporterMat;
    [SerializeField] GameObject teleporter;
    // Start is called before the first frame update
    void Start()
    {

        teleporter.GetComponent<Renderer>().material = teleporterMat;// sets teleporter material to designer specified material

    }

}
