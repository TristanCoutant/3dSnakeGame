using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartScene : MonoBehaviour
{
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private DigitDisplay digitDisplay;

    private void Awake()
    {
        if (scoreTracker == null)
            scoreTracker = FindFirstObjectByType<ScoreTracker>();

        var existing = FindObjectsByType<StartScene>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        scoreTracker.score = 0;
        if (digitDisplay == null)
            digitDisplay = FindFirstObjectByType<DigitDisplay>();

        if (scoreTracker != null)
        {

            Debug.Log($"Dernier score : {scoreTracker.score}");
            Debug.Log($"Highscore : {scoreTracker.highScore}");

            if (digitDisplay != null)
                digitDisplay.DisplayScores();
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            GamePoints.IsSnakeDead = false;
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
