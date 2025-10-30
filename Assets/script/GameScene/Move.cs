using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public enum ControlMode { Keyboard, Mouse }

    [Header("Snake Settings")]
    [SerializeField] private Transform snakeHead;
    [SerializeField] private Transform bodySegmentPrefab;
    [SerializeField] private Transform parentBodyObject;
    [SerializeField] private float interval = 0.3f;
    [SerializeField] private ScoreTracker scoreTracker;

    [Header("Control Settings")]
    [SerializeField] private ControlMode controlMode = ControlMode.Keyboard;

    private GridManager gridManager;
    private GamePoints gamePoints;

    private readonly List<Vector3> positions = new();
    private readonly List<Transform> bodySegments = new();
    private float timer;
    private int i, j;

    // 1 = up, 2 = right, 3 = down, 4 = left
    private int direction = 2;
    private int lastDirection = 2;

    private void Awake()
    {
        if (scoreTracker == null)
            scoreTracker = FindFirstObjectByType<ScoreTracker>();
    }

    private void Start()
    {
        gridManager = GridManager.Instance;
        if (gridManager == null)
        {
            Debug.LogError("GridManager instance not found!");
            return;
        }

        gamePoints = FindFirstObjectByType<GamePoints>();
        if (gamePoints == null)
        {
            Debug.LogError("GamePoints not found in the scene!");
            return;
        }

        gridManager.GenerateGrid();
        ResetSnake();
    }

    private void Update()
    {
        if (GamePoints.IsSnakeDead) return;

        HandleInput();

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            MoveSnake();
        }
    }

    private void HandleInput()
    {
        switch (controlMode)
        {
            case ControlMode.Keyboard:
                HandleKeyboardInput();
                break;
            case ControlMode.Mouse:
                HandleMouseInput();
                break;
        }
    }

    private void HandleKeyboardInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && lastDirection != 3)
            direction = 1;
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame && lastDirection != 4)
            direction = 2;
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame && lastDirection != 1)
            direction = 3;
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame && lastDirection != 2)
            direction = 4;
    }

    private void HandleMouseInput()
    {
        if (Mouse.current == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Vector3 delta = hit.point - snakeHead.position;
        int newDirection = direction;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
            newDirection = delta.x > 0 ? 2 : 4;
        else
            newDirection = delta.z > 0 ? 1 : 3;

        if (!IsOppositeDirection(newDirection, lastDirection))
            direction = newDirection;
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
            case 1: j++; snakeHead.rotation = Quaternion.Euler(0, 0, 0); break;
            case 2: i++; snakeHead.rotation = Quaternion.Euler(0, 90, 0); break;
            case 3: j--; snakeHead.rotation = Quaternion.Euler(0, 180, 0); break;
            case 4: i--; snakeHead.rotation = Quaternion.Euler(0, 270, 0); break;
        }

        i = (i + gridManager.width) % gridManager.width;
        j = (j + gridManager.height) % gridManager.height;

        Vector3 pos = gridManager.PositionOfTile(i, j);
        snakeHead.position = new Vector3(pos.x, 1f, pos.z);

        // Check collision with body
        foreach (var bodyPos in positions.Skip(1))
        {
            if (Vector3.Distance(bodyPos, snakeHead.position) < 0.1f)
            {
                SnakeDead();
                return;
            }
        }

        positions.Insert(0, snakeHead.position);
        while (positions.Count > scoreTracker.score + 1)
            positions.RemoveAt(positions.Count - 1);

        UpdateBody();
        lastDirection = direction;
    }

    private void UpdateBody()
    {
        // Create missing segments
        while (bodySegments.Count < scoreTracker.score)
        {
            Transform newSeg = Instantiate(bodySegmentPrefab, parentBodyObject);
            bodySegments.Add(newSeg);
        }

        // Remove extra segments
        while (bodySegments.Count > scoreTracker.score)
        {
            Destroy(bodySegments[^1].gameObject);
            bodySegments.RemoveAt(bodySegments.Count - 1);
        }

        // Update positions
        for (int k = 0; k < bodySegments.Count; k++)
        {
            if (k + 1 < positions.Count)
            {
                Vector3 target = positions[k + 1];
                bodySegments[k].position = new Vector3(target.x, 1f, target.z);
            }
        }
    }

    public void SnakeDead()
    {
        GamePoints.IsSnakeDead = true; // This should work now
        if (gamePoints != null)
            gamePoints.SnakeDead();
        DestroySnakeBody();
    }

    private void DestroySnakeBody()
    {
        foreach (var seg in bodySegments)
            if (seg != null) Destroy(seg.gameObject);

        bodySegments.Clear();
        positions.Clear();
    }

    public void ResetSnake()
    {
        DestroySnakeBody();
        i = gridManager.width / 4;
        j = gridManager.height / 2;
        Vector3 pos = gridManager.PositionOfTile(i, j);
        snakeHead.position = new Vector3(pos.x, 1f, pos.z);
        positions.Add(snakeHead.position);
        direction = 2;
        lastDirection = 2;
        
        // Reset the game state through GamePoints
        if (gamePoints != null)
        {
            GamePoints.ResetGameState();
        }
        else
        {
            GamePoints.IsSnakeDead = false;
        }
    }
    
    // Reset when enabled
    private void OnEnable()
    {
        if (gridManager != null) // Only reset if gridManager is already set up
            ResetSnake();
    }
}