using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoints : MonoBehaviour
{
    [SerializeField] private GridManager GridManager;
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private Transform snakeHead;
    [SerializeField] private AudioClip WinSound;
    [SerializeField] private AudioClip LoseSound;
    [SerializeField] private AudioSource AudioSource;

    private GameObject currentBonus;

    public int score = 0;
    public int HighScore = 0;
    public bool IsSnakeDead = false;

    void Start()
    {
        SpawnBonus(); 
    }

    void Update()
    {
        if (currentBonus != null && snakeHead.position == currentBonus.transform.position)
        {
            MoveBonus();
            score += 1;
            AudioSource.PlayOneShot(WinSound);
        }
    }

    public void SpawnBonus()
    {
        if (!IsSnakeDead)
        {
            int x = Random.Range(0, GridManager.width);
            int z = Random.Range(0, GridManager.height);

            Vector3 spawnPos = GridManager.PositionOfTile(x, z);
            spawnPos = new Vector3(spawnPos.x, 1f, spawnPos.z);

            currentBonus = Instantiate(bonusPrefab, spawnPos, Quaternion.identity);
        }        
    }

    private void MoveBonus()
    {
        if (!IsSnakeDead)
        {
            int x = Random.Range(0, GridManager.width);
            int z = Random.Range(0, GridManager.height);

            Vector3 newPos = GridManager.PositionOfTile(x, z);
            newPos = new Vector3(newPos.x, 1f, newPos.z);

            currentBonus.transform.position = newPos;
        }
    }

    public void SetHighScore()
    {
        if (score > HighScore)
        {
            HighScore = score;
        }
        score = 0;
    }

    public Vector3 currentBonusPosition()
    {
        if (!IsSnakeDead && currentBonus != null)
        {
            return currentBonus.transform.position;
        }
        else
        {
            Debug.Log("Snake is dead!");
            return Vector3.zero;
        }
    }

    public void SnakeDead()
    {
        SetHighScore();

        if (currentBonus != null)
            Destroy(currentBonus);

        if (snakeHead != null)
            Destroy(snakeHead.gameObject);

        AudioSource.PlayOneShot(LoseSound);

        IsSnakeDead = true;
    }
}
