using System;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int HillPoints = 10;

    [SerializeField] private Mesh _treeFruitMesh;
    [SerializeField] private Mesh _treeWithoutFruitMesh;
    [SerializeField] private Material[] _colorsTreeFruit;
    [SerializeField] private Material[] _colorsTreeWithoutFruit;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    public event EventHandler<int> FruitCollected;
    
    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    public void DeleteFruits()
    {
        ChangeMeshAndColors(_treeWithoutFruitMesh, _colorsTreeWithoutFruit);
        FruitCollected?.Invoke(this, HillPoints);
    }

    public void AddFruit()
    {
        ChangeMeshAndColors(_treeFruitMesh, _colorsTreeFruit);
    }

    private void ChangeMeshAndColors(Mesh mesh, Material[] materials)
    {
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
        _meshRenderer.materials = materials;
    }
}