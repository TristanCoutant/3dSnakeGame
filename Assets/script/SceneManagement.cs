using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 10f;
    [SerializeField] private string gameSceneName; 
    [SerializeField] private string startSceneName;

    [SerializeField] private Transform persistentObjectTransform; 

    private void Awake()
    {
        if (persistentObjectTransform != null)
        {
            DontDestroyOnLoad(persistentObjectTransform.gameObject);
        }
        else
        {
            Debug.LogWarning("persistentObjectTransform non assigné !");
        }
    }

    private void Start()
    {
        Invoke(nameof(StartSceneMethod), delaySeconds);
    }

    private void Update()
    {
        if (GamePoints.IsSnakeDead == true)
        {
            LoadNextScene();
        }
    }

    private void StartSceneMethod()
    {
        if (!string.IsNullOrEmpty(startSceneName))
        {
            SceneManager.LoadScene(startSceneName);
        }
        else
        {
            Debug.LogError("startSceneName n'est pas défini !");
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("gameSceneName n'est pas défini !");
        }
    }
}
