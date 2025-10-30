using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private Transform parentObject;

    private GridManager gridManager;
    private GamePoints gamePoints;

    private readonly List<GameObject> spawnedObstacles = new();
    private readonly List<GameObject> obstaclesToDelete = new();

    private float timer;
    private float maxObstacles;
    private const float SpawnInterval = 4f;

    public bool IsInitialized { get; private set; } = false;

    private void Awake()
    {
        AutoFindDependencies();
    }

    private void Start()
    {
        StartCoroutine(InitializeWhenReady());
    }

    private System.Collections.IEnumerator InitializeWhenReady()
    {
        while (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
            yield return null;
        }

        while (gamePoints == null)
        {
            gamePoints = FindFirstObjectByType<GamePoints>();
            yield return null;
        }

        maxObstacles = (gridManager.width * gridManager.height) / 2f;
        IsInitialized = true;
    }

    private void Update()
    {
        if (!IsInitialized || GamePoints.IsSnakeDead || snakeHead == null) return;

        timer += Time.deltaTime;
        if (timer >= SpawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }

        CheckCollision();
    }

    private void AutoFindDependencies()
    {
        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();

        if (gamePoints == null)
            gamePoints = FindFirstObjectByType<GamePoints>();

        if (snakeHead == null)
        {
            Move snake = FindFirstObjectByType<Move>();
            if (snake != null)
                snakeHead = snake.transform;
        }
    }

    private void SpawnObstacle()
    {
        if (!IsInitialized || gridManager == null || obstaclePrefab == null)
            return;

        if (spawnedObstacles.Count >= maxObstacles)
            return;

        int x = Random.Range(0, gridManager.width);
        int z = Random.Range(0, gridManager.height);

        if (IsTileOccupied(x, z))
            return;

        Vector3 pos = gridManager.PositionOfTile(x, z);
        pos.y = 1f;

        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity, parentObject);
        obstacle.name = $"Obstacle ({x}/{z})";
        spawnedObstacles.Add(obstacle);
        obstaclesToDelete.Add(obstacle);
    }

    public bool IsTileOccupied(int x, int z)
    {
        if (!IsInitialized || gridManager == null)
            return false;

        Vector3 checkPos = gridManager.PositionOfTile(x, z);

        if (snakeHead != null && Vector3.Distance(checkPos, snakeHead.position) < 0.1f)
            return true;

        if (gamePoints != null && Vector3.Distance(checkPos, gamePoints.CurrentBonusPosition()) < 0.1f)
            return true;

        foreach (GameObject obs in spawnedObstacles)
        {
            if (obs != null && Vector3.Distance(obs.transform.position, checkPos) < 0.1f)
                return true;
        }

        return false;
    }

    private void CheckCollision()
    {
        if (!IsInitialized || snakeHead == null)
            return;

        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obs = spawnedObstacles[i];
            if (obs == null) continue;

            if (Vector3.Distance(snakeHead.position, obs.transform.position) < 0.1f)
            {
                gamePoints?.SnakeDead();
                Destroy(obs);
                obstaclesToDelete.Remove(obs);
                spawnedObstacles.RemoveAt(i);
            }
        }

        spawnedObstacles.RemoveAll(item => item == null);
    }
}
