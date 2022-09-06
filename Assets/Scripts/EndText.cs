using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndText : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    public void KillText()
    {
        Destroy(gameObject);
        Instantiate(deathParticles, transform.position, transform.rotation);
        GameManager.Instance.LetterDestroyed();
    }
}
