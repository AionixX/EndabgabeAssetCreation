using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] GameObject lightningPrefab;

    public void Init(int _lightningsLeft, float _lightningDistance = 0.1f, float _lightningNextTime = 0.1f)
    {
        if (_lightningsLeft > 0)
            StartCoroutine(WaitTillNext(_lightningsLeft, _lightningDistance, _lightningNextTime));
        else
            StartCoroutine(WaitTillDie());
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    IEnumerator WaitTillNext(int _lightningsLeft, float _lightningDistance, float _lightningNextTime)
    {
        yield return new WaitForSeconds(_lightningNextTime);
        LightningController controller = Instantiate(lightningPrefab, transform.position + (transform.forward * _lightningDistance), transform.rotation).GetComponent<LightningController>();
        controller.Init(--_lightningsLeft, _lightningDistance, _lightningNextTime);
        StartCoroutine(WaitTillDie());
    }
    IEnumerator WaitTillDie(float _time = 0.5f) {
        yield return new WaitForSeconds(_time);
        Die();
    }
}
