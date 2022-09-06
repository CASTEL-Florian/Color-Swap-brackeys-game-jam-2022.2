using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float despawnTime = 6;
    [SerializeField] private float fadeTime = 2;
    private bool active = true;
    private bool despawning = false;
    private float time = 0;
    private void Update()
    {
        if (!active || despawning)
            return;
        time += Time.deltaTime;
        if (time > despawnTime)
        {
            despawning = true;
            StartCoroutine(DespawnRoutine());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" || !active)
            return;
        collision.GetComponent<UnitHealth>().Heal(1);
        audio.Play();
        renderer.enabled = false;
        active = false;
        StopAllCoroutines();
        Destroy(gameObject, audio.clip.length);
    }

    private IEnumerator DespawnRoutine()
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            Color col = renderer.color;
            col.a = 1 - t / fadeTime;
            renderer.color = col;
            yield return null;
        }
        active = false;
        Destroy(gameObject);
    }

}
