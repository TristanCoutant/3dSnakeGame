using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float offsetX = -width / 2f;
        float offsetZ = -height / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var spawnedTile = Instantiate(tilePrefab, 
                    new Vector3(x + offsetX, 0, z + offsetZ), 
                    Quaternion.identity);
                spawnedTile.name = $"Tile ({x}/{z})";

                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0); 
                spawnedTile.Init(isOffset); 
            }
        }
    }
}
