using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay;
    public float shake_intensity;

    private void Awake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    private void Update()
    {
        if (shake_intensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
            shake_intensity -= shake_decay;

            if (shake_intensity <= 0)
            {
                transform.rotation = Quaternion.identity;
                transform.position = originPosition;
            }
        }
    }

    public void Shake(float intensity, float decay)
    {
        //originPosition = transform.position;
        //originRotation = transform.rotation;
        shake_intensity = intensity;//.3f;
        shake_decay = decay;//0.002f;
    }
}