using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 3f;
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string startSceneName = "StartScene";

    [SerializeField] private Transform persistentObjectTransform;

    private bool isRestarting = false;

    private void Start()
    {
        if (persistentObjectTransform != null)
        {
            DontDestroyOnLoad(persistentObjectTransform.gameObject);
        }

        if (SceneManager.GetActiveScene().name == startSceneName)
        {
            StartCoroutine(LoadGameSceneAfterDelay());
        }
    }

    private void Update()
    {
        if (GamePoints.IsSnakeDead && !isRestarting)
        {
            isRestarting = true;
            StartCoroutine(RestartGameSequence());
            GamePoints.IsSnakeDead = false;
        }
    }

    private IEnumerator LoadGameSceneAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator RestartGameSequence()
    {
        SceneManager.LoadScene(startSceneName);

        yield return null;

        yield return new WaitForSeconds(delaySeconds);

        SceneManager.LoadScene(gameSceneName);

        isRestarting = false;
    }
}
