using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public int score;
    public int highScore;

    private static ScoreTracker instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public void CheckAndSaveHighscore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log($"New Highscore Saved: {highScore}");
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
}
