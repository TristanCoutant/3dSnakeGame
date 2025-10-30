using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePoints : MonoBehaviour
{
    [Header("Snake Settings")]
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private ScoreTracker scoreTracker;

    private GameObject currentBonus;
    public static bool IsSnakeDead = false;

    private void Awake()
    {
        var existing = FindObjectsByType<GamePoints>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        AutoFindDependencies();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AutoFindDependencies();
        StartCoroutine(StartAfterSceneLoad());
    }

    private void AutoFindDependencies()
    {
        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();

        if (obstacleManager == null)
            obstacleManager = FindFirstObjectByType<ObstacleManager>();

        if (scoreTracker == null)
            scoreTracker = FindFirstObjectByType<ScoreTracker>();

        if (snakeHead == null)
        {
            Move move = FindFirstObjectByType<Move>();
            if (move != null)
                snakeHead = move.transform;
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(WaitAndSpawn());
    }

    private IEnumerator StartAfterSceneLoad()
    {
        yield return StartCoroutine(WaitAndSpawn());
    }

    private IEnumerator WaitAndSpawn()
    {
        // Wait until GridManager exists
        while (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
            yield return null;
        }

        // Wait until ObstacleManager exists
        while (obstacleManager == null)
        {
            obstacleManager = FindFirstObjectByType<ObstacleManager>();
            yield return null;
        }

        // Wait until obstacles are fully initialized
        while (!obstacleManager.IsInitialized)
            yield return null;

        // Small delay to ensure all objects are placed
        yield return new WaitForEndOfFrame();

        SpawnBonus();
    }

    private void Update()
    {
        if (IsSnakeDead || currentBonus == null || snakeHead == null) return;

        if (Vector3.Distance(snakeHead.position, currentBonus.transform.position) < 0.1f)
        {
            MoveBonus();

            if (scoreTracker != null)
                scoreTracker.score++;

            if (audioSource != null && winSound != null)
                audioSource.PlayOneShot(winSound);
        }
    }

    // ✅ Fixed SpawnBonus to prevent ghost apples
    private void SpawnBonus()
    {
        if (IsSnakeDead || bonusPrefab == null) return;

        // Ensure dependencies are ready
        if (gridManager == null || obstacleManager == null || !obstacleManager.IsInitialized)
        {
            StartCoroutine(RetrySpawnBonus());
            return;
        }

        Vector3 spawnPos = GetRandomFreePosition();

        if (spawnPos == Vector3.zero)
        {
            StartCoroutine(RetrySpawnBonus());
            return;
        }

        // Fix Y alignment with the snake
        spawnPos.y = snakeHead != null ? snakeHead.position.y : 0.5f;

        // Destroy any leftover ghost apples
        if (currentBonus != null)
            Destroy(currentBonus);

        currentBonus = Instantiate(bonusPrefab, spawnPos, Quaternion.identity);
    }

    private IEnumerator RetrySpawnBonus()
    {
        yield return new WaitForEndOfFrame();
        SpawnBonus();
    }

    private void MoveBonus()
    {
        if (IsSnakeDead || currentBonus == null) return;

        Vector3 newPos = GetRandomFreePosition();

        if (newPos == Vector3.zero)
        {
            StartCoroutine(RetrySpawnBonus());
            return;
        }

        newPos.y = snakeHead != null ? snakeHead.position.y : 0.5f;
        currentBonus.transform.position = newPos;
    }

    private Vector3 GetRandomFreePosition()
    {
        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();

        if (gridManager == null)
            return Vector3.zero;

        for (int tries = 0; tries < 1000; tries++)
        {
            int x = Random.Range(0, gridManager.width);
            int z = Random.Range(0, gridManager.height);

            bool occupied = false;

            if (obstacleManager != null && obstacleManager.IsInitialized)
            {
                try
                {
                    occupied = obstacleManager.IsTileOccupied(x, z);
                }
                catch { occupied = false; }
            }

            if (!occupied)
            {
                Vector3 pos = gridManager.PositionOfTile(x, z);
                return new Vector3(pos.x, 0.5f, pos.z);
            }
        }

        return Vector3.zero;
    }

    public Vector3 CurrentBonusPosition() =>
        IsSnakeDead || currentBonus == null ? Vector3.zero : currentBonus.transform.position;

    public void SnakeDead()
    {
        if (currentBonus != null) Destroy(currentBonus);
        if (snakeHead != null) Destroy(snakeHead.gameObject);

        if (audioSource != null && loseSound != null)
            audioSource.PlayOneShot(loseSound);

        IsSnakeDead = true;
    }
}
