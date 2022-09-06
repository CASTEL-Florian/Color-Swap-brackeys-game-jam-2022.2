using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private UnitColor color;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private AudioSource audio;
    private bool active = true;
    private float time = 0;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float maxX = Mathf.Infinity;
    [SerializeField] private float maxY = Mathf.Infinity;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > lifeTime || Mathf.Abs(transform.position.x) > maxX ||Mathf.Abs(transform.position.y) > maxY)
        {
            active = false;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitColor col;
        col = collision.GetComponent<UnitColor>();
        if (!active || !col || col.color != color.color)
            return;
        UnitHealth health = collision.GetComponent<UnitHealth>();
        if (health)
            health.TakeDamage(1);
        active = false;
        renderer.enabled = false;
        audio.Play();
        Destroy(gameObject, audio.clip.length);
    }
}
