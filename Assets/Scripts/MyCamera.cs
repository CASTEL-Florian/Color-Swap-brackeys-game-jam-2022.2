using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyCamera : MonoBehaviour
{
    [SerializeField] float shakeDuration;
    [SerializeField] AnimationCurve shakeCurve;
    private Camera cam;
    private Vector3 shakeOffset = Vector3.zero;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        transform.position = shakeOffset + new Vector3(0,0,transform.position.z);
    }


    public Vector3 MousePosition()
    {
        return cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public IEnumerator Shake(float intensity = 1)
    {
        float elapsed = 0;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            shakeOffset = Random.insideUnitSphere * shakeCurve.Evaluate(elapsed/shakeDuration) * intensity;
            shakeOffset.z = 0;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }
}
