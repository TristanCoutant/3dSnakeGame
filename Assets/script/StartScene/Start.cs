using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 10f;
    [SerializeField] private string sceneToLoad = "GameScene";

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delaySeconds);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
