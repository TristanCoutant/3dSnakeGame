using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [SerializeField] private Transform snakeHead;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float interval;

    private float timer = 0f;
    public int i, j;
    private float direction = 2f; // 1: up, 2: right, 3: down, 4: left
    void Start()
    {
        gridManager.GenerateGrid();
        InitializePosition();
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

        if (i < 0)
        { i = gridManager.width-1; }

        else if (j < 0)
        { j = gridManager.height-1; }

        else if (i == gridManager.width)
        { i = 0; }

        else if (j == gridManager.height)
        { j = 0; }

        snakeHead.position = gridManager.PositionOfTile(i, j);
        var p = snakeHead.position;
        snakeHead.position = new Vector3(p.x, 1f, p.z);
        }
        
    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) { direction = 1; };
            
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) { direction = 2; };
                
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) { direction = 3; };
                
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) { direction = 4; };
                
            
                
        }
        
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            UpdatePosition();
        }
    }
}