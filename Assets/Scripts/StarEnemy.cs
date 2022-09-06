using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEnemy : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private List<Transform> firePoints;
    [SerializeField] private float laserSpeed;
    public void SpawnLasers()
    {
        foreach (Transform point in firePoints)
        {
            Rigidbody2D obj = Instantiate(laserPrefab, point.position, point.rotation).GetComponent<Rigidbody2D>();
            obj.velocity = laserSpeed * obj.transform.right;
        }
    }
}
