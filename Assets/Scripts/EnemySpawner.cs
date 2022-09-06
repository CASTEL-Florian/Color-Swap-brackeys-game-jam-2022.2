using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave{
    public List<GameObject> enemies;
    public List<float> timeBetweenSpawn;
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Wave> waves;
    [SerializeField] private Vector2 rectangleUpLeftCorner;
    [SerializeField] private Vector2 rectangleWidthHeight;
    private int currentWave = 0;

    public int SpawnWave()
    {
        if (currentWave >= waves.Count)
            return -1;
        currentWave += 1;
        StartCoroutine(SpawnWaveRoutine(currentWave - 1));
        return waves[currentWave - 1].enemies.Count;
    }
    private IEnumerator SpawnWaveRoutine(int waveId)
    {
        for (int i = 0; i < waves[waveId].enemies.Count; i++)
        {
            Vector3 spawnPoint = RandomSpawnPosition();
            Instantiate(waves[waveId].enemies[i], spawnPoint, Quaternion.identity);
            if (i < waves[waveId].timeBetweenSpawn.Count)
                yield return new WaitForSeconds(waves[waveId].timeBetweenSpawn[i]);
        }
        yield return null;
    }


    private Vector3 RandomSpawnPosition()
    {
        int side = Random.Range(0, 4);
        float x = 0;
        float y = 0;
        if (side % 2 == 0)
        {
            y = side == 0 ? rectangleUpLeftCorner.y : rectangleUpLeftCorner.y + rectangleWidthHeight.y;
            x = rectangleUpLeftCorner.x +  Random.Range(0, rectangleWidthHeight.x);
        }
        else
        {
            x = side == 1 ? rectangleUpLeftCorner.x : rectangleUpLeftCorner.x + rectangleWidthHeight.x;
            y = rectangleUpLeftCorner.y + Random.Range(0, rectangleWidthHeight.y);
        }
        return new Vector3(x, y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(rectangleUpLeftCorner, rectangleUpLeftCorner + new Vector2(rectangleWidthHeight.x, 0));
        Gizmos.DrawLine(rectangleUpLeftCorner, rectangleUpLeftCorner + new Vector2(0, rectangleWidthHeight.y));
        Gizmos.DrawLine(rectangleUpLeftCorner + rectangleWidthHeight, rectangleUpLeftCorner + new Vector2(rectangleWidthHeight.x, 0));
        Gizmos.DrawLine(rectangleUpLeftCorner + rectangleWidthHeight, rectangleUpLeftCorner + new Vector2(0, rectangleWidthHeight.y));
    }

    public int WaveCount()
    {
        return waves.Count;
    }
}
