using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private HealthDisplay healthBar;
    [SerializeField] private Player player;
    [SerializeField] private float invinsibilityTime = 0f;
    [SerializeField] private SimpleFlash flash;
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private UnityEvent deathEvents;
    [SerializeField] private float spawnInvincibility = 0f;
    private float t = 0;
    private int currentHealth;
    private bool alive = true;
    private bool isInvinsible = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (t < spawnInvincibility)
            t += Time.deltaTime;
    }
    public void TakeDamage(int value)
    {
        if (t < spawnInvincibility)
            return;
        if (!alive || isInvinsible)
            return;
        currentHealth -= value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthBar)
            healthBar.UpdateHp(currentHealth);
        if (currentHealth <= 0)
        {
            alive = false;
            deathEvents.Invoke();
            if (deathAudio)
                GameManager.Instance.PlayAudio(deathAudio);
        }
        if (flash)
            flash.Flash();
        if (player)
        {
            player.Damaged(!alive);
            StartCoroutine(setInvisible());
        }
    }

    public void Heal(int value)
    {
        if (!alive)
            return;
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthBar)
            healthBar.UpdateHp(currentHealth);
    }

    public bool isAlive()
    {
        return alive;
    }

    IEnumerator setInvisible()
    {
        isInvinsible = true;
        yield return new WaitForSeconds(invinsibilityTime);
        isInvinsible = false;
    }

    public void SetMaxHealth(int value) 
    {
        maxHealth = value;
        currentHealth = value;
    }
}
