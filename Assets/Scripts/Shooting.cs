using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float rateOverTime;
    [SerializeField] private int numberOfBulletsPerShot = 1;
    [SerializeField] private float emissionAngle = 0;
    [SerializeField] private float bulletVelocity = 80f;
    [SerializeField] private AudioSource audio;


    private float clock = 0;
    private bool active = false;
    private void Update()
    {
        clock -= Time.deltaTime;
        if (clock < 0 && active)
        {
            clock = 1 / rateOverTime;
            Shoot(numberOfBulletsPerShot);
        }
    }

    private void Shoot(int numberOfBullets)
    {
        audio.Play();
        for (int i = 0; i < numberOfBullets; i++)
        {
            Quaternion rot = firePoint.rotation;
            float da = Random.Range(-0.5f, 0.5f) * emissionAngle;
            rot = Quaternion.Euler(0, -0, da) * rot;
            Rigidbody2D obj = Instantiate(bulletPrefab, firePoint.position, rot).GetComponent<Rigidbody2D>();
            obj.velocity = bulletVelocity * obj.transform.right;
        }
    }

    public void SetActive(bool value)
    {
        active = value;
    }
}
