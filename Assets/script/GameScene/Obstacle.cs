using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private Transform parentObject;
    [SerializeField] private GamePoints gamePoints;

    private readonly List<GameObject> spawnedObstacles = new();
    private float timer;
    private float maxObstacles;
    private const float SpawnInterval = 4f;

    private void Start()
    {
        maxObstacles = (gridManager.width * gridManager.height) / 2f;
    }

    private void Update()
    {
        if (gamePoints.IsSnakeDead) return;

        timer += Time.deltaTime;
        if (timer >= SpawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }

        CheckCollision();
    }

    private void SpawnObstacle()
    {
        if (spawnedObstacles.Count >= maxObstacles) return;

        int x = Random.Range(0, gridManager.width);
        int z = Random.Range(0, gridManager.height);

        if (IsTileOccupied(x, z)) return;

        Vector3 pos = gridManager.PositionOfTile(x, z);
        pos.y = 1f;

        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity, parentObject);
        obstacle.name = $"Obstacle ({x}/{z})";
        spawnedObstacles.Add(obstacle);
    }

    public bool IsTileOccupied(int x, int z)
    {
        Vector3 checkPos = gridManager.PositionOfTile(x, z);
        if (Vector3.Distance(checkPos, snakeHead.position) < 0.1f ||
            Vector3.Distance(checkPos, gamePoints.CurrentBonusPosition()) < 0.1f)
            return true;

        foreach (GameObject obs in spawnedObstacles)
            if (obs != null && Vector3.Distance(obs.transform.position, checkPos) < 0.1f)
                return true;

        return false;
    }

    private void CheckCollision()
    {
        foreach (GameObject obs in spawnedObstacles)
        {
            if (obs != null && Vector3.Distance(snakeHead.position, obs.transform.position) < 0.1f)
            {
                gamePoints.SnakeDead();
                Destroy(obs);
                spawnedObstacles.Remove(obs);
                break;
            }
        }
    }
}