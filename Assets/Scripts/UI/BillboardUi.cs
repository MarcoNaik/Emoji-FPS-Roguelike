using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BillboardUi : MonoBehaviour
{
    private Transform cam;

    private void Awake()
    {
        if (Camera.main != null) cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position+ cam.forward);
    }
}
