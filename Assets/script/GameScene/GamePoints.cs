using System.IO;
using UnityEngine;

public class GamePoints : MonoBehaviour
{
    [Header("Snake Settings")]
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioSource audioSource;

    private GridManager gridManager;
    private ObstacleManager obstacleManager;
    private GameObject currentBonus;

    private string highScoreFilePath;

    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public static bool IsSnakeDead = false;

    private void Awake()
    {
        // Définir le chemin du fichier temporaire pour HighScore
        highScoreFilePath = Path.Combine(Application.temporaryCachePath, "highscore.txt");
        LoadHighScore(); // Charger HighScore au lancement
    }

    private void Start()
    {
        if (gridManager == null)
            gridManager = GridManager.Instance;

        if (gridManager == null)
        {
            Debug.LogError("GridManager non trouvé !");
            return;
        }

        SpawnBonus();
    }

    private void Update()
    {
        if (IsSnakeDead || currentBonus == null || snakeHead == null) return;

        if (Vector3.Distance(snakeHead.position, currentBonus.transform.position) < 0.1f)
        {
            MoveBonus();
            Score++;
            if (audioSource != null && winSound != null)
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

            if (obstacleManager == null || !obstacleManager.IsTileOccupied(x, z))
            {
                Vector3 pos = gridManager.PositionOfTile(x, z);
                return new Vector3(pos.x, 1f, pos.z);
            }
        }
    }

    public Vector3 CurrentBonusPosition() =>
        IsSnakeDead || currentBonus == null ? Vector3.zero : currentBonus.transform.position;

    public void SnakeDead()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            SaveHighScore(); 
        }

        Score = 0;

        if (currentBonus) Destroy(currentBonus);
        if (snakeHead) Destroy(snakeHead.gameObject);

        if (audioSource != null && loseSound != null)
            audioSource.PlayOneShot(loseSound);

        IsSnakeDead = true;
    }

    private void SaveHighScore()
    {
        try
        {
            File.WriteAllText(highScoreFilePath, HighScore.ToString());
            Debug.Log("HighScore saved to: " + highScoreFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save HighScore: " + e.Message);
        }
    }

    private void LoadHighScore()
    {
        try
        {
            if (File.Exists(highScoreFilePath))
            {
                string content = File.ReadAllText(highScoreFilePath);
                HighScore = int.Parse(content);
                Debug.Log("HighScore loaded: " + HighScore);
            }
            else
            {
                HighScore = 0;
                Debug.Log("No temporary HighScore found. Starting from 0.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load HighScore: " + e.Message);
            HighScore = 0;
        }
    }
}
