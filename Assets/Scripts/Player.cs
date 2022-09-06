using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Shooting gun;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CharacterMovement controller;
    [SerializeField] private float dashCooldown = 1;
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip deathSound;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isDead = false;
    private MyCamera cam;

    private void Start()
    {
        cam = FindObjectOfType<MyCamera>();
    }
    public void SetGunActive(bool value)
    {
        if (isDead)
            gun.SetActive(false);
        else
            gun.SetActive(value);
    }

    public void RotateTowardMouse()
    {
        if (isDead)
            return;
        Vector3 dir = (cam.MousePosition() - transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    public void Move(Vector2 move)
    {
        if (isDead)
        {
            controller.Move(Vector2.zero);
            return;
        }
        if (!isDashing)
            controller.Move(move);
    }

    public IEnumerator DashCoroutine()
    {
        if (!canDash)
            yield return null;
        else
        {
            Vector3 dir = (cam.MousePosition() - controller.transform.position);
            Vector2 dir2d = new Vector2(dir.x, dir.y).normalized * dashSpeed;
            canDash = false;
            isDashing = true;
            rb.velocity = dir2d;
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }

    public void Damaged(bool dead)
    {
        if (dead)
        {
            audio.PlayOneShot(deathSound);
            StartCoroutine(cam.Shake(2));
            isDead = true;
        }
        else
        {
            audio.Play();
            StartCoroutine(cam.Shake());
        }
    }
}
