using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum EnemyType { GoToPlayer, Random };
    [SerializeField] private CharacterMovement controller;
    [SerializeField] float speed = 200;
    [SerializeField] private UnitColor unitColor;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private EnemyType enemyType = EnemyType.GoToPlayer;
    [SerializeField] private Vector2 rectangleUpLeftCorner;
    [SerializeField] private Vector2 rectangleWidthHeight;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float activationTime;
    [SerializeField] private GameObject heart;
    [SerializeField] private float heartSpawnRate = 0.1f;
    private float t = 0;
    private Vector3 dest = Vector3.zero; 
    private GameManager gameManager;
    private PlayerManager playerManager;
    private string color; 
    private void Start()
    {
        color = unitColor.color;
        playerManager = FindObjectOfType<PlayerManager>();
        gameManager = GameManager.Instance;
        if (enemyType == EnemyType.Random)
        {
            RandomizeDest();
            if (Random.Range(0, 2) == 0)
                rotationSpeed *= -1;
        }
    }

    private void FixedUpdate()
    {
        if (t < activationTime)
        {
            t += Time.fixedDeltaTime;
            return;
        }    
        if (enemyType == EnemyType.GoToPlayer)
        {
            Transform playerTransform = playerManager.FindClosePlayer(transform, color);
            Vector3 move = (playerTransform.position - transform.position).normalized;
            controller.Move(new Vector2(move.x, move.y) * Time.fixedDeltaTime * speed);
        }
        else
        {
            if (Vector3.Distance(transform.position, dest) < 1)
            {
                RandomizeDest();
            }
            else
            {
                controller.Move(new Vector2(dest.x - transform.position.x, dest.y - transform.position.y).normalized * Time.fixedDeltaTime * speed);
            }
            transform.Rotate(Vector3.forward * Time.fixedDeltaTime * rotationSpeed);
        }
    }
 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<UnitColor>().color == color)
            {
                collision.GetComponent<UnitHealth>().TakeDamage(1);
            }
            
        }
    }

    private void RandomizeDest()
    {
        float x = Random.Range(rectangleUpLeftCorner.x, rectangleUpLeftCorner.x + rectangleWidthHeight.x);
        float y = Random.Range(rectangleUpLeftCorner.y, rectangleUpLeftCorner.y + rectangleWidthHeight.y);
        dest = new Vector3(x, y, 0);
    }

    public void EnemyDead()
    {
        gameManager.EnemyDead(transform.position);
        Instantiate(deathParticles, transform.position, transform.rotation);
        float rand = Random.Range(0f, 1f);
        if (rand < heartSpawnRate)
            Instantiate(heart, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
