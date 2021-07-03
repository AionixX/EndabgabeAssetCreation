using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] GameObject lightningPrefab;
    float damage;
    int lightningsLeft;
    float lightningDistance;
    float lightningNextTime;

    public void Init(float _damage, int _lightningsLeft, float _lightningDistance = 0.1f, float _lightningNextTime = 0.1f)
    {
        damage = _damage;
        lightningsLeft = _lightningsLeft;
        lightningDistance = _lightningDistance;
        lightningNextTime = _lightningNextTime;

        if (_lightningsLeft > 0)
            StartCoroutine(WaitTillNext());
        else
            StartCoroutine(WaitTillDie());
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    IEnumerator WaitTillNext()
    {
        yield return new WaitForSeconds(lightningNextTime);
        LightningController controller = Instantiate(lightningPrefab, transform.position + (transform.forward * lightningDistance), transform.rotation).GetComponent<LightningController>();
        controller.Init(damage, --lightningsLeft, lightningDistance, lightningNextTime);
        StartCoroutine(WaitTillDie());
    }
    IEnumerator WaitTillDie(float _time = 0.5f) {
        yield return new WaitForSeconds(_time);
        Die();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyHammer")) {
            other.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        }
    }
}
