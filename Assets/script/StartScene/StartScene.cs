using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartScene : MonoBehaviour
{

    private void Awake()
    {
        if (transform.parent != null)
            transform.parent = null;

        var existing = Object.FindObjectsByType<StartScene>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
