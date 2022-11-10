using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{

    [SerializeField] private static float gravity = 9.8f; // global gravity var

    public static float getGravity()
    {

        return gravity;

    }

}
