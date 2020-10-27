using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float rotationSpeed = 3f;

    void Update()
    {
         transform.Rotate(0, -Time.deltaTime * rotationSpeed,0, Space.Self);
    }
}
