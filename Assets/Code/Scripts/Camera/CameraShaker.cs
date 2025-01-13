using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.2f;
    [SerializeField] private float dampingSpeed = 1.0f; 

    private Vector3 initialPosition; 
    private float currentShakeDuration = 0f;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void TriggerShake(float duration)
    {
        currentShakeDuration = (duration > 0) ? duration : shakeDuration;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        initialPosition = transform.localPosition;

        while (currentShakeDuration > 0)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = initialPosition + randomOffset;

            currentShakeDuration -= Time.deltaTime * dampingSpeed;

            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
