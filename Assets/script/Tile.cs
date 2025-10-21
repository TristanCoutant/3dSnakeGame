using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private MeshRenderer _renderer;

    void Start()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<MeshRenderer>();
        }
    }

    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? offsetColor : baseColor;
}
}