using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color offsetColor;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Awake()
    {
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Init(bool isOffset)
    {
        meshRenderer.material.color = isOffset ? offsetColor : baseColor;
    }
}
