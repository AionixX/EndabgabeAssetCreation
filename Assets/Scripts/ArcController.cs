using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcController : MonoBehaviour
{
    [SerializeField] GameObject diePrefab = null;
    [SerializeField] GameObject collidePrefab = null;
    [SerializeField] int enemyLayer = 8;
    [SerializeField] float explosionForce = 1000f;
    // [SerializeField] float explosionRadius = 10f;
    float speed;
    float lifeTime;
    float explosionRadius;
    bool started = false;
    void Update()
    {
        if (!started) return;

        transform.position = transform.position + (transform.forward * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Die(diePrefab);
    }

    void Die(GameObject _diePrefab)
    {
        if (_diePrefab)
            Instantiate(_diePrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void Init(float _speed, float _lifetime, float _explosionRadius)
    {
        speed = _speed;
        lifeTime = _lifetime;
        explosionRadius = _explosionRadius;
        started = true;
    }

    void OnTriggerEnter(Collider other)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.one, explosionRadius, 1 << enemyLayer);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("EnemyArc"))
            {
                hit.collider.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
        }


        Die(collidePrefab);
    }
}
