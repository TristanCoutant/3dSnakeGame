using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public float speed = 1f;

    void FixedUpdate()
    {
        float vertical = 0f;
        float horizontal = 0f;

        // WASD et les fleches
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += speed;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= speed;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= speed;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += speed;
        }

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        transform.Translate(movement * speed * Time.deltaTime, Space.Self);
    }
}