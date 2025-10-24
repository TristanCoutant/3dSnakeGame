using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    private bool isRestarting = false;
    private static bool hasStarted = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var managers = FindObjectsByType<SceneManagement>(FindObjectsSortMode.None);
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        }
    }

    private void Update()
    {
        if (GamePoints.IsSnakeDead && !isRestarting)
        {
            isRestarting = true;
            StartCoroutine(RestartSequence());
            GamePoints.IsSnakeDead = false;
        }
    }

    private IEnumerator RestartSequence()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        yield return null;
        isRestarting = false;
    }
}
