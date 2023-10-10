using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : BaseMgr<CameraMgr>
{


    public bool isShake; //没必要同时发生多个震动
    public float shakeTime;
    public float shakeStrength;

    public float defaultDuration;
    public float defaultStrength;


    public void ShakeCamera()
    {
        if (!isShake)
        {
            StartCoroutine(Shake(defaultDuration, defaultStrength));
        }
    }
    public void ShakeCamera(float _duration, float _strength)
    {
        if (!isShake)
        {
            StartCoroutine(Shake(_duration, _strength));
        }

    }

    IEnumerator Shake(float _duration, float _strength)
    {
        isShake = true;
        Vector3 startPosition = this.transform.position;

        while (_duration > 0)
        {
            this.transform.position = Random.insideUnitSphere * _strength + startPosition;
            _duration -= Time.deltaTime;
            yield return null;
        }

        this.transform.position = startPosition;
        isShake = false;
    }
}
