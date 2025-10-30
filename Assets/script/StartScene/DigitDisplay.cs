// DigitDisplay.cs
using UnityEngine;
using System.Collections.Generic;

public class DigitDisplay : MonoBehaviour
{
    [Header("Digit Prefabs")]
    [SerializeField] private GameObject[] digitPrefabs; // Assign 0-9 in inspector
    
    [Header("Score Display")]
    [SerializeField] private Vector3 scoreStartPosition = new Vector3(-2f, 0f, 0f);
    [SerializeField] private float digitSpacing = 1.5f;
    [SerializeField] private float scoreSpacing = 3f; // Space between score and highscore
    
    [Header("References")]
    [SerializeField] private ScoreTracker scoreTracker;
    
    private List<GameObject> currentScoreDigits = new List<GameObject>();
    private List<GameObject> currentHighscoreDigits = new List<GameObject>();

    private void Start()
    {
        if (scoreTracker == null)
            scoreTracker = FindFirstObjectByType<ScoreTracker>();
            
        DisplayScores();
    }

    public void DisplayScores()
    {
        ClearDigits();

        if (scoreTracker == null) return;
        
        DisplayNumber(scoreTracker.score, scoreStartPosition, currentScoreDigits);
        
        Vector3 highscoreStartPos = scoreStartPosition + Vector3.right * scoreSpacing;
        DisplayNumber(scoreTracker.highScore, highscoreStartPos, currentHighscoreDigits);
    }

    private void DisplayNumber(int number, Vector3 startPosition, List<GameObject> digitList)
    {
        int displayNumber = Mathf.Clamp(number, 0, 99);
        int tens = displayNumber / 10;
        int ones = displayNumber % 10;

        Quaternion digitRotation = Quaternion.Euler(-90f, 180f, 0f);
        
        Vector3 digitScale = Vector3.one * 2f;

        if (digitPrefabs.Length > tens && digitPrefabs[tens] != null)
        {
            GameObject tensDigit = Instantiate(digitPrefabs[tens], 
                startPosition, digitRotation, transform);
            tensDigit.transform.localScale = digitScale;
            digitList.Add(tensDigit);
        }

        if (digitPrefabs.Length > ones && digitPrefabs[ones] != null)
        {
            GameObject onesDigit = Instantiate(digitPrefabs[ones], 
                startPosition + Vector3.right * digitSpacing, digitRotation, transform);
            onesDigit.transform.localScale = digitScale;
            digitList.Add(onesDigit);
        }
    }

    private void ClearDigits()
    {
        foreach (GameObject digit in currentScoreDigits)
        {
            if (digit != null) Destroy(digit);
        }
        currentScoreDigits.Clear();

        foreach (GameObject digit in currentHighscoreDigits)
        {
            if (digit != null) Destroy(digit);
        }
        currentHighscoreDigits.Clear();
    }

    public void RefreshDisplay()
    {
        DisplayScores();
    }
}