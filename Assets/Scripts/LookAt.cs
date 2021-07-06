using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] bool billboard = false;
    void Update()
    {
        transform.LookAt(target ? target.position : billboard ? Camera.main.gameObject.transform.position : transform.position + transform.forward);
    }
}
