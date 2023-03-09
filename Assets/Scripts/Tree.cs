using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int HillPoints = 10;

    [SerializeField] private float _timeToGrow = 30f;
    
    [SerializeField] private Mesh _treeFruitMesh;
    [SerializeField] private Mesh _treeWithoutFruitMesh;
    [SerializeField] private Material[] _colorsTreeFruit;
    [SerializeField] private Material[] _colorsTreeWithoutFruit;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private bool _isCollected = false;

    public event EventHandler<int> FruitCollected;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    public void DeleteFruits()
    {
        if (_isCollected) return;

        ChangeMeshAndColors(_treeWithoutFruitMesh, _colorsTreeWithoutFruit);
        FruitCollected?.Invoke(this, HillPoints);
        _isCollected = true;

        StartCoroutine(AddFruit());
    }

    public IEnumerator AddFruit()
    {
        yield return new WaitForSeconds(_timeToGrow);
        
        ChangeMeshAndColors(_treeFruitMesh, _colorsTreeFruit);
        _isCollected = false;
    }

    private void ChangeMeshAndColors(Mesh mesh, Material[] materials)
    {
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
        _meshRenderer.materials = materials;
    }
}