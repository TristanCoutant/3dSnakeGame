using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoints : MonoBehaviour
{
    [SerializeField] private GridManager GridManager;
    [SerializeField] private GameObject bonusPrefab; 
    [SerializeField] private Transform snakeHead;

    private GameObject currentBonus;

    public float Score = 0;
    public float HighScore = 0;
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
            Score = Score + 1;
        }
    }

    public void SpawnBonus()
    {
        if (IsSnakeDead == false)
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
        if (IsSnakeDead == false)
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
        if (Score > HighScore)
        {
            HighScore = Score;
        }
        Score = 0;
    }

    public Vector3 currentBonusPosition()
{
    if (!IsSnakeDead)
    {
        return currentBonus.transform.position;
    }
    else
    {
        print("Snake is dead");
        return Vector3.zero;
    }
}
    
    public void SnakeDead()
    {
        SetHighScore();
        Destroy(currentBonus);
        Destroy(snakeHead.gameObject);
        Application.Quit();
        IsSnakeDead = true;
    }

    
}
