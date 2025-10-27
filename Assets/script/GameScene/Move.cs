using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [Header("Snake Settings")]
    [SerializeField] private Transform snakeHead;
    [SerializeField] private Transform bodySegmentPrefab;
    [SerializeField] private Transform parentBodyObject;
    [SerializeField] private float interval = 0.3f;

    private GridManager gridManager;
    private GamePoints gamePoints;

    private readonly List<Vector3> positions = new();
    private readonly List<Transform> bodySegments = new();
    private float timer;
    private int i, j;

    // 1 = up, 2 = right, 3 = down, 4 = left
    private int direction = 2;
    private int lastDirection = 2;

    private void Start()
    {
        gridManager = GridManager.Instance;
        if (gridManager == null)
        {
            Debug.LogError("GridManager instance not found! Make sure it exists and uses DontDestroyOnLoad.");
            return;
        }

        gamePoints = GamePoints.FindFirstObjectByType<GamePoints>();
        if (gamePoints == null)
        {
            Debug.LogError("GamePoints not found in the scene!");
            return;
        }

        gridManager.GenerateGrid();

        InitializePosition();
        positions.Add(snakeHead.position);
    }

    private void InitializePosition()
    {
        i = gridManager.width / 4;
        j = gridManager.height / 2;
        Vector3 pos = gridManager.PositionOfTile(i, j);
        snakeHead.position = new Vector3(pos.x, 1f, pos.z);
    }

    private void Update()
    {
        if (GamePoints.IsSnakeDead)
        {
            foreach (var seg in bodySegments) Destroy(seg.gameObject);
            bodySegments.Clear();
            return;
        }

        HandleMouseInput();

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            MoveSnake();
        }
    }

    private void HandleMouseInput()
    {
        if (Mouse.current == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = hit.point;
            Vector3 delta = targetPos - snakeHead.position;

            int newDirection = direction;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
            {
                if (delta.x > 0) newDirection = 2; // Right
                else if (delta.x < 0) newDirection = 4; // Left
            }
            else
            {
                if (delta.z > 0) newDirection = 1; // Up
                else if (delta.z < 0) newDirection = 3; // Down
            }

            if (!IsOppositeDirection(newDirection, lastDirection))
            {
                direction = newDirection;
            }
        }
    }

    private bool IsOppositeDirection(int dir1, int dir2)
    {
        return (dir1 == 1 && dir2 == 3) ||
               (dir1 == 3 && dir2 == 1) ||
               (dir1 == 2 && dir2 == 4) ||
               (dir1 == 4 && dir2 == 2);
    }

    private void MoveSnake()
    {
        switch (direction)
        {
            case 1: j++; snakeHead.rotation = Quaternion.Euler(0, 0, 0); break;     // Up
            case 2: i++; snakeHead.rotation = Quaternion.Euler(0, 90, 0); break;    // Right
            case 3: j--; snakeHead.rotation = Quaternion.Euler(0, 180, 0); break;   // Down
            case 4: i--; snakeHead.rotation = Quaternion.Euler(0, 270, 0); break;   // Left
        }

        i = (i + gridManager.width) % gridManager.width;
        j = (j + gridManager.height) % gridManager.height;

        Vector3 pos = gridManager.PositionOfTile(i, j);
        snakeHead.position = new Vector3(pos.x, 1f, pos.z);

        foreach (var bodyPos in positions.Skip(1))
        {
            if (Vector3.Distance(bodyPos, snakeHead.position) < 0.1f)
            {
                gamePoints.SnakeDead();
                return;
            }
        }

        positions.Insert(0, snakeHead.position);
        while (positions.Count > gamePoints.Score + 1)
            positions.RemoveAt(positions.Count - 1);

        UpdateBody();
        lastDirection = direction;
    }

    private void UpdateBody()
    {
        while (bodySegments.Count < gamePoints.Score)
        {
            Transform newSeg = Instantiate(bodySegmentPrefab, parentBodyObject);
            bodySegments.Add(newSeg);
        }

        for (int k = 0; k < bodySegments.Count; k++)
        {
            if (k + 1 < positions.Count)
            {
                Vector3 target = positions[k + 1];
                bodySegments[k].position = new Vector3(target.x, 1f, target.z);
            }
        }
    }
}
