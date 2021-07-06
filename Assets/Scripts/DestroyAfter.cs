using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] float timeTillDie = 10f;
    [SerializeField] bool startOnAwake = false;
    bool timerStarted = false;

    void Awake()
    {
        timerStarted = startOnAwake;
    }

    void Update()
    {
        if(!timerStarted) return;

        timeTillDie -= Time.deltaTime;
        if(timeTillDie <= 0f)
            Destroy(this.gameObject);
    }
}
