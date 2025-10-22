using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GridManager GridManager;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private Transform parentObject;
    [SerializeField] private GamePoints GamePoints;

    private float interval = 5f;
    private float timer;
    private Vector3 currentBonus;

    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Update()
    {
        currentBonus = GamePoints.currentBonusPosition();
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            SpawnObstacle();
        }

        CheckIfSnakeIsDead();
    }

    private void SpawnObstacle()
    {
        if (!GamePoints.IsSnakeDead)
        {
            int x = Random.Range(0, GridManager.width);
            int z = Random.Range(0, GridManager.height);

            Vector3 spawnPos = GridManager.PositionOfTile(x, z);
            spawnPos = new Vector3(spawnPos.x, 1f, spawnPos.z);

            GameObject spawnedObstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, parentObject);
            spawnedObstacle.name = $"Obstacle ({x}/{z})";

            spawnedObstacles.Add(spawnedObstacle);
        }
    }

    private void CheckIfSnakeIsDead()
    {
        if (!GamePoints.IsSnakeDead)
        {
            for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
            {
                GameObject obstacle = spawnedObstacles[i];
                if (obstacle != null && snakeHead.position == obstacle.transform.position)
                {
                    GamePoints.SnakeDead();
                    Destroy(obstacle);
                    spawnedObstacles.RemoveAt(i); 
                    break;
                }
            }
        }
    }
}
