using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Button playButton;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayButtonPressed);
        else
            Debug.LogError("Le bouton n'est pas assigné dans l'inspecteur !");
    }

    private void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
