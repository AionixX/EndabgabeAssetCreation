using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldController : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] string opacityReference = "_Opacity";
    [SerializeField] bool playOnAwake = true;
    [SerializeField] Transform target;
    [SerializeField] int enemyLayer = 8;
    [SerializeField] float speed;
    [SerializeField]float lifeTime;
    bool started = false;
    float lifeTimeLeft;


    void Awake()
    {
        target = target != null ? target : transform;
        started = playOnAwake;
    }

    public void Init(float _speed, float _lifetime)
    {
        speed = _speed;
        lifeTime = _lifetime;
        lifeTimeLeft = lifeTime;
        started = true;
    }

    void Update()
    {
        if (!started) return;

        
        // transform.localScale = Vector3.one * speed * Time.deltaTime;
        transform.localScale += Vector3.one * speed * Time.deltaTime;
        meshRenderer.material.SetFloat(opacityReference, lifeTimeLeft / lifeTime);

        lifeTimeLeft -= Time.deltaTime;
        if (lifeTimeLeft <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("OnTriggerEnter " + other.name);
        if (other.gameObject.layer == enemyLayer){
            other.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
