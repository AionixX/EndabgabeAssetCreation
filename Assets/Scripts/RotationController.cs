using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 speed;

    void Update()
    {
        target.Rotate(speed * Time.deltaTime);
    }
}
