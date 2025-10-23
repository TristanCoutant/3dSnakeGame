using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private GameObject StartScenePrefab;

    private void Start()
    {
        Instantiate(StartScenePrefab, Vector3.zero, Quaternion.identity);
    }
}