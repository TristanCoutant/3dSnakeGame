using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public int score;
    public int highScore;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (GamePoints.IsSnakeDead)
        {
            if (score > highScore)
            {
                highScore = score;
            }
        }
    }
}
