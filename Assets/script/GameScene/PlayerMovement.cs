using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [SerializeField] private Transform snakeHead;
    [SerializeField] private Transform bodySegmentPrefab;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float interval;
    [SerializeField] private GamePoints gamePoints;
    [SerializeField] private Transform parentBodyObject;

    private float timer = 0f;
    public int i, j;
    private float direction = 2f;

    private List<Vector3> positions = new List<Vector3>();
    private List<Transform> bodySegments = new List<Transform>();

    void Start()
    {
        gridManager.GenerateGrid();
        InitializePosition();
        positions.Add(snakeHead.position);
    }

    private void InitializePosition()
    {
        i = gridManager.width / 4;
        j = gridManager.height / 2;
        snakeHead.position = gridManager.PositionOfTile(i, j);
        var p = snakeHead.position;
        snakeHead.position = new Vector3(p.x, 1f, p.z);
    }

    private void UpdatePosition()
    {
        if (direction == 1) j += 1;
        else if (direction == 2) i += 1;
        else if (direction == 3) j -= 1;
        else if (direction == 4) i -= 1;

        if (i < 0) i = gridManager.width - 1;
        else if (i == gridManager.width) i = 0;

        if (j < 0) j = gridManager.height - 1;
        else if (j == gridManager.height) j = 0;

        Vector3 newPos = gridManager.PositionOfTile(i, j);
        snakeHead.position = new Vector3(newPos.x, 1f, newPos.z);

        positions.Insert(0, snakeHead.position);

        int maxLength = gamePoints.score;
        while (positions.Count > maxLength + 1)
        {
            positions.RemoveAt(positions.Count - 1);
        }

        UpdateBodySegments();
    }

    private void UpdateBodySegments()
    {
        while (bodySegments.Count < gamePoints.score)
        {
            Transform newSegment = Instantiate(bodySegmentPrefab, parentBodyObject);
            bodySegments.Add(newSegment);
        }

        while (bodySegments.Count > gamePoints.score)
        {
            Destroy(bodySegments[bodySegments.Count - 1].gameObject);
            bodySegments.RemoveAt(bodySegments.Count - 1);
        }

        for (int k = 0; k < bodySegments.Count; k++)
        {
            if (k + 1 < positions.Count)
            {
                Vector3 targetPos = positions[k + 1];
                bodySegments[k].position = new Vector3(targetPos.x, 1f, targetPos.z);
            }
        }
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) direction = 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) direction = 2;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) direction = 3;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) direction = 4;
        }

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            UpdatePosition();
        }

        if (gamePoints.IsSnakeDead == true)
        {
            foreach (var segment in bodySegments)
            {
                Destroy(segment.gameObject);
            }
        }
    }
}