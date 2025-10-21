using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoints : MonoBehaviour
{
    [SerializeField] private GridManager GridManager;
    [SerializeField] private GameObject bonusPrefab;  // Prefab to spawn
    [SerializeField] private Transform snakeHead;

    private GameObject currentBonus;

    void Start()
    {
        SpawnBonus();  // Spawn the bonus prefab once at start
    }

    void Update()
    {
        if (currentBonus != null && snakeHead.position == currentBonus.transform.position)
        {
            MoveBonus();
        }
    }

    private void SpawnBonus()
    {
        int x = Random.Range(0, GridManager.width);
        int z = Random.Range(0, GridManager.height);

        Vector3 spawnPos = GridManager.PositionOfTileBonus(x, z);
        spawnPos = new Vector3(spawnPos.x, 1f, spawnPos.z);

        currentBonus = Instantiate(bonusPrefab, spawnPos, Quaternion.identity);
    }

    private void MoveBonus()
    {
        int x = Random.Range(0, GridManager.width);
        int z = Random.Range(0, GridManager.height);

        Vector3 newPos = GridManager.PositionOfTileBonus(x, z);
        newPos = new Vector3(newPos.x, 1f, newPos.z);

        currentBonus.transform.position = newPos;
    }
}
