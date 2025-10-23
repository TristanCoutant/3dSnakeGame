using UnityEngine;

public class GamePoints : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Obstacle obstacleManager;

    private GameObject currentBonus;

    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public bool IsSnakeDead { get; private set; }

    private void Start()
    {
        SpawnBonus();
    }

    private void Update()
    {
        if (IsSnakeDead || currentBonus == null) return;

        if (Vector3.Distance(snakeHead.position, currentBonus.transform.position) < 0.1f)
        {
            MoveBonus();
            Score++;
            audioSource.PlayOneShot(winSound);
        }
    }

    private void SpawnBonus()
    {
        if (IsSnakeDead) return;

        Vector3 spawnPos = GetRandomFreePosition();
        currentBonus = Instantiate(bonusPrefab, spawnPos, Quaternion.identity);
    }

    private void MoveBonus()
    {
        if (IsSnakeDead) return;
        currentBonus.transform.position = GetRandomFreePosition();
    }

    private Vector3 GetRandomFreePosition()
    {
        while (true)
        {
            int x = Random.Range(0, gridManager.width);
            int z = Random.Range(0, gridManager.height);

            if (!obstacleManager.IsTileOccupied(x, z))
            {
                Vector3 pos = gridManager.PositionOfTile(x, z);
                return new Vector3(pos.x, 1f, pos.z);
            }
        }
    }

    public Vector3 CurrentBonusPosition() => IsSnakeDead || currentBonus == null ? Vector3.zero : currentBonus.transform.position;

    public void SnakeDead()
    {
        if (Score > HighScore) HighScore = Score;
        Score = 0;

        if (currentBonus) Destroy(currentBonus);
        if (snakeHead) Destroy(snakeHead.gameObject);

        audioSource.PlayOneShot(loseSound);
        IsSnakeDead = true;
    }
}
