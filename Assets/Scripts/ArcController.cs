using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcController : MonoBehaviour
{
    float speed;
    float lifeTime;
    float damage;
    bool started = false;
    void Update()
    {
        if(!started) return;

        transform.position = transform.position + (transform.forward * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0f)
            Die();
    }

    void Die() {
        Destroy(this.gameObject);
    }

    public void Init(float _damage, float _speed, float _lifetime) {
        damage = _damage;
        speed = _speed;
        lifeTime = _lifetime;
        started = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) {
            other.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        }

        Die();
    }
}
