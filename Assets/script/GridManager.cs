using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform parentObject;

    private bool isGridGenerated = false;

    public void GenerateGrid()
    {
        if (isGridGenerated) return;

        Vector3 center = transform.position;

        float offsetX = -width / 2f + 0.5f;
        float offsetZ = -height / 2f + 0.5f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 spawnPos = new Vector3(x + offsetX, 0, z + offsetZ) + center;

                var spawnedTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, parentObject);
                spawnedTile.name = $"Tile ({x}/{z})";

                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);
                spawnedTile.Init(isOffset);
            }
        }

        cam.transform.position = center + new Vector3(0, 25, 0);
        isGridGenerated = true;
    }

    public Vector3 PositionOfTile(int x, int z)
    {
        Vector3 center = transform.position;
        float offsetX = -width / 2f + 0.5f;
        float offsetZ = -height / 2f + 0.5f;
        return new Vector3(x + offsetX, 0f, z + offsetZ) + center;
    }
}


