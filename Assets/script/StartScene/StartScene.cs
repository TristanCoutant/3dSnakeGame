using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartScene : MonoBehaviour
{
    [SerializeField] private ScoreTracker scoreTracker; // assigne en inspector si nécessaire

    private void Awake()
    {
        // Si un ScoreTracker existe déjà, on l'utilise
        if (scoreTracker == null)
            scoreTracker = FindFirstObjectByType<ScoreTracker>();

        // Eviter doublons
        var existing = FindObjectsByType<StartScene>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Affiche le score précédent et le highscore
        if (scoreTracker != null)
        {
            Debug.Log($"Dernier score : {scoreTracker.score}");
            Debug.Log($"Highscore : {scoreTracker.highScore}");
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
